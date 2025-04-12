using EasyPoll.Infra.Services;

namespace EasyPoll.Infra;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection sc, IConfiguration config)
    {
        var postgresConn = config.GetConnectionString("Postgres")
                           ?? throw new ApplicationException("Conn string for Postgres not configured in appsettings");

        var redisConn = config.GetConnectionString("Redis")
                        ?? throw new ApplicationException("Conn string for Redis not configured in appsettings");

        sc.AddDbContext<AppDbContext>(options => options.UseNpgsql(postgresConn));
        sc.AddStackExchangeRedisCache(options => options.Configuration = redisConn);

        sc.AddScoped<IAppDbContext, AppDbContext>();
        sc.AddSingleton<RedisClient>(_ => new RedisClient(redisConn));
        sc.AddSingleton<ICacheService, CacheService>();

        sc.AddScoped<IPollService, PollService>();
    }
}