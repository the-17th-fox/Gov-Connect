using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedLib.Redis.Configurations;
using SharedLib.Redis.Interfaces;
using SharedLib.Redis.Utilities;

namespace SharedLib.Redis.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CachedAttribute : Attribute, IAsyncActionFilter
{
    private short? _timeToLiveSeconds;

    public CachedAttribute(short timeToLiveSeconds)
    {
        _timeToLiveSeconds = timeToLiveSeconds;
    }

    /// <summary>
    /// If no TTL has been provided - uses DefaultTTL from appsettings.
    /// </summary>
    public CachedAttribute() {}

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var cacheSettings = GetRedisCacheConfiguration(context.HttpContext.RequestServices);

        if (!cacheSettings.IsEnabled)
        {
            await next();
            return;
        }

        // If the parameterless constructor has been used ==> TTL is unassigned (null)
        _timeToLiveSeconds ??= cacheSettings.DefaultTTLSeconds;
        
        var redisService = GetRedisService(context.HttpContext.RequestServices);

        var cacheKey = CacheBuilderHelper.GenerateCacheKey(context.HttpContext.Request);

        var cachedResponse = await redisService.TryGetFromCacheAsync(cacheKey);

        if (!string.IsNullOrWhiteSpace(cachedResponse))
        {
            SetCachedResponse(context, cachedResponse);
            return;
        }

        var executedContext = await next();

        if (executedContext.Result is OkObjectResult okObjectResult)
        {
            await redisService.SetCacheAsync(cacheKey, okObjectResult.Value, (short)_timeToLiveSeconds);
        }
    }

    private static RedisCacheConfiguration GetRedisCacheConfiguration(IServiceProvider serviceProvider)
    {
        var cacheConfig = serviceProvider.GetRequiredService<IOptions<RedisCacheConfiguration>>().Value;

        return cacheConfig ?? throw new ArgumentNullException(nameof(cacheConfig));
    }

    private static IRedisService GetRedisService(IServiceProvider serviceProvider)
    {
        var redisService = serviceProvider.GetRequiredService<IRedisService>();

        return redisService ?? throw new ArgumentException(nameof(redisService));
    }

    private static void SetCachedResponse(ActionExecutingContext context, string cachedResponse)
    {
        var contentResult = new ContentResult()
        {
            Content = cachedResponse,
            ContentType = "application/json",
            StatusCode = StatusCodes.Status200OK
        };

        context.Result = contentResult;
    }
}