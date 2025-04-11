namespace EasyPoll.Infra.Data;

internal sealed class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Poll> Polls { get; private set; }
    public DbSet<Vote> Votes { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PollEntityTypeConfig());
        modelBuilder.ApplyConfiguration(new VoteEntityTypeConfig());
        base.OnModelCreating(modelBuilder);
    }

    public async Task CommitChangesAsync(CancellationToken ct = default)
    {
        await SaveChangesAsync(ct);
    }
}