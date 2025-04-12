namespace EasyPoll.Infra.Redis;

internal sealed class RedisClient
{
    private readonly IConnectionMultiplexer _redis;

    public RedisClient(string conn)
    {
        _redis = ConnectionMultiplexer.Connect(conn);
    }

    internal IDatabase GetDatabase()
    {
        return _redis.GetDatabase();
    }
}