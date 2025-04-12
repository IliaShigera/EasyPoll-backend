namespace EasyPoll.Infra.Services;

internal sealed class CacheService : ICacheService
{
    private readonly IDatabase _redis;

    public CacheService(RedisClient client)
    {
        _redis = client.GetDatabase();
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiry)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        await _redis.StringSetAsync(key, JsonConvert.SerializeObject(value), expiry);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        var value = await _redis.StringGetAsync(key);
        return value.HasValue
            ? JsonConvert.DeserializeObject<T>(value!)
            : default;
    }

    public async Task<bool> KeyExistsAsync(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        return await _redis.KeyExistsAsync(key);
    }

    public async Task<long> IncrementHashAsync(string key, string field, long value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentException.ThrowIfNullOrWhiteSpace(field);

        return await _redis.HashIncrementAsync(key, field, value);
    }

    public async Task<Dictionary<string, long>> GetHashFieldsAsync(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        var entries = await _redis.HashGetAllAsync(key);
        return entries.ToDictionary(
            entry => entry.Name.ToString(),
            entry => (long)entry.Value,
            StringComparer.OrdinalIgnoreCase);
    }
}