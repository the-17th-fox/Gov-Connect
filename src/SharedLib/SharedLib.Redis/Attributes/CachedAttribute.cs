using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedLib.Redis.Configurations;
using SharedLib.Redis.Interfaces;

namespace SharedLib.Redis.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CachedAttribute : Attribute, IAsyncActionFilter
{
    private short? _timeToLiveSeconds;

    public CachedAttribute(short timeToLiveSeconds)
    {
        _timeToLiveSeconds = timeToLiveSeconds;
    }

    public CachedAttribute() {}

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var cacheSettings = GetRedisCacheConfiguration(context.HttpContext.RequestServices);

        if (!cacheSettings.IsEnabled)
        {
            await next();
            return;
        }

        // If the parameterless constructor was used ==> TTL is unassigned (null)
        _timeToLiveSeconds ??= cacheSettings.TimeToLiveSeconds;
        
        var redisService = GetRedisService(context.HttpContext.RequestServices);

        var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

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

    private RedisCacheConfiguration GetRedisCacheConfiguration(IServiceProvider serviceProvider)
    {
        var cacheConfig = serviceProvider.GetRequiredService<IOptions<RedisCacheConfiguration>>().Value;

        return cacheConfig ?? throw new ArgumentNullException(nameof(cacheConfig));
    }

    private IRedisService GetRedisService(IServiceProvider serviceProvider)
    {
        var redisService = serviceProvider.GetRequiredService<IRedisService>();

        return redisService ?? throw new ArgumentException(nameof(redisService));
    }

    private string GenerateCacheKeyFromRequest(HttpRequest request)
    {
        var keyBuilder = new StringBuilder();

        keyBuilder.Append($"{request.Path}");

        foreach (var (key, value) in request.Query.OrderBy(p => p.Key))
        {
            keyBuilder.Append($"|[{key}]-[{value}]");
        }

        return keyBuilder.ToString();
    }

    private void SetCachedResponse(ActionExecutingContext context, string cachedResponse)
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