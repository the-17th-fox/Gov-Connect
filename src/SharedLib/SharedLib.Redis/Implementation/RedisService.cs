using SharedLib.Redis.Interfaces;
using StackExchange.Redis;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
        var parsedValue = JsonSerializer.Serialize(cacheValue);

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
        _ = transaction.KeyExpireAsync(cacheKey, DateTime.UtcNow.AddSeconds(timeToLiveSeconds));

        await transaction.ExecuteAsync();

        return TaskStatus.RanToCompletion;
    }

    private void CheckMultiplexerConnection(IConnectionMultiplexer connectionMultiplexer)
    {
        if (connectionMultiplexer is null)
        {
            throw new ArgumentNullException(nameof(connectionMultiplexer));
        }
    }

    private void CheckStorageConnection(string key)
    {
        if (!_redis.Multiplexer.IsConnected)
        {
            throw new RedisConnectionException(ConnectionFailureType.ConnectionDisposed, $"Key:[{key}]");
        }
    }
}