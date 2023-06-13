namespace SharedLib.Redis.Configurations;

public class RedisCacheConfiguration
{
    /// <summary>
    /// Uses if another expiration span hasn't been provided
    /// </summary>
    public short TimeToLiveSeconds { get; set; }
    public bool IsEnabled { get; set; }
}