namespace EasyPoll.Infra.Data;

internal sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var conn = args[0];
        ArgumentException.ThrowIfNullOrWhiteSpace(conn);

        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseNpgsql(conn);

        return new AppDbContext(builder.Options);
    }
}