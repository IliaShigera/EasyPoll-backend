namespace EasyPoll.Core.Interfaces;

public interface ICacheService
{
    Task SetAsync<T>(string key, T value, TimeSpan expiry);
    Task<T?> GetAsync<T>(string key);
    Task<bool> KeyExistsAsync(string key);
    Task<long> IncrementHashAsync(string key, string field, long value);
    Task<Dictionary<string, long>> GetHashFieldsAsync(string key);
}