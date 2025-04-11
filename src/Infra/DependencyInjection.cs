namespace EasyPoll.Infra;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection sc, IConfiguration config)
    {
        var conn = config.GetConnectionString("Postgres")
                   ?? throw new ApplicationException("Conn string for Postgres not configured in appsettings.json");

        sc.AddDbContext<AppDbContext>(options => options.UseNpgsql(conn));
    }
}