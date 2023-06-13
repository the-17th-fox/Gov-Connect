using System.Globalization;
using SharedLib.Redis.Interfaces;
using StackExchange.Redis;

namespace SharedLib.Redis.Implementation;

public class RedisService : IRedisService
{
    private readonly IDatabase _redis;

    public RedisService(IConnectionMultiplexer multiplexer)
    {
        CheckMultiplexerConnection(multiplexer);
        
        _redis = multiplexer.GetDatabase();
    }

    public async Task<string> TryGetFromCacheAsync(string cacheKey)
    {
        CheckStorageConnection(cacheKey);

        string? response = await _redis.StringGetAsync(cacheKey);

        return response ?? string.Empty;
    }

    public async Task<TaskStatus> SetCacheAsync<TValue>(string cacheKey, TValue cacheValue, short timeToLiveSeconds)
    {
        CheckStorageConnection(cacheKey);

        var parsedValue = Convert.ToString(cacheValue, CultureInfo.InvariantCulture);

        return await SetCacheAsync(cacheKey, parsedValue!, timeToLiveSeconds);
    }

    public async Task<TaskStatus> SetCacheAsync(string cacheKey, string cacheValue, short timeToLiveSeconds)
    {
        if (string.IsNullOrWhiteSpace(cacheValue))
        {
            throw new ArgumentNullException(nameof(cacheValue));
        }

        CheckStorageConnection(cacheKey);

        var transaction = _redis.CreateTransaction(new());

        _ = transaction.StringSetAsync(cacheKey, cacheValue);
        _ = transaction.KeyExpireAsync(cacheKey, DateTime.UtcNow.AddMinutes(timeToLiveSeconds));

        await transaction.ExecuteAsync();

        return TaskStatus.RanToCompletion;
    }

    private void CheckMultiplexerConnection(IConnectionMultiplexer connectionMultiplexer)
    {
        if (connectionMultiplexer is null)
        {
            throw new ArgumentNullException(nameof(connectionMultiplexer));
        }

        if (!connectionMultiplexer.IsConnected)
        {
            throw new RedisConnectionException(ConnectionFailureType.UnableToConnect, string.Empty);
        }
    }

    private void CheckStorageConnection(string key)
    {
        if (!_redis.IsConnected(key))
        {
            throw new RedisConnectionException(ConnectionFailureType.ConnectionDisposed, $"Key:[{key}]");
        }
    }
}