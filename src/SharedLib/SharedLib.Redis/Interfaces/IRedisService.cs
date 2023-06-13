namespace SharedLib.Redis.Interfaces;

public interface IRedisService
{
    public Task<string> TryGetFromCacheAsync(string cacheKey);
    public Task<TaskStatus> SetCacheAsync<TValue>(string cacheKey, TValue cacheValue, short timeToLiveSeconds);
    public Task<TaskStatus> SetCacheAsync(string cacheKey, string cacheValue, short timeToLiveSeconds);
}