namespace EasyPoll.Core.Interfaces;

public interface IAppDbContext
{
    DbSet<Poll> Polls { get; }
    DbSet<Vote> Votes { get; }

    Task CommitChangesAsync(CancellationToken ct = default);
}