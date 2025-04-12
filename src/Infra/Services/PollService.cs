namespace EasyPoll.Infra.Services;

internal sealed class PollService : IPollService
{
    private const int MinOptionsCount = 2;
    private const int MaxOptionsCount = 10;
    private const int MaxExpiryInDays = 90;

    private readonly IAppDbContext _dbContext;
    private readonly ICacheService _cache;

    public PollService(IAppDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<Poll> CreatePollAsync(
        string question,
        List<string> options,
        DateTime expiresAt,
        bool showResultsAfterVoteOnly,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(question)
            || options.Count < MinOptionsCount
            || options.Count > MaxOptionsCount
            || expiresAt < DateTime.UtcNow
            || expiresAt > DateTime.UtcNow.AddDays(MaxExpiryInDays))
        {
            throw new ArgumentException("Invalid poll data provided");
        }

        var poll = new Poll(
            question,
            options,
            createdAt: DateTime.UtcNow,
            expiresAt,
            PollStatus.Active,
            showResultsAfterVoteOnly);

        await _dbContext.Polls.AddAsync(poll, ct);
        await _dbContext.CommitChangesAsync(ct);

        await _cache.SetAsync($"poll:{poll.Id}", poll, TimeSpan.FromMilliseconds(expiresAt.Millisecond));

        return poll;
    }

    public async Task<Poll> GetPollAsync(Guid pollId, CancellationToken ct = default)
    {
        var cachedPoll = await _cache.GetAsync<Poll>($"poll:{pollId}");
        if (cachedPoll is not null)
            return cachedPoll;

        var poll = await _dbContext.Polls.FirstOrDefaultAsync(p => p.Id == pollId, ct);
        if (poll is null)
            throw new EntityNotFoundException(nameof(Poll), nameof(pollId), pollId.ToString());

        await _cache.SetAsync($"poll:{pollId}", poll, TimeSpan.FromMilliseconds(poll.ExpiresAt.Millisecond));
        return poll;
    }

    public async Task VoteAsync(Guid pollId, int optionIndex, string voterFingerprint, CancellationToken ct = default)
    {
        var poll = await _dbContext.Polls.FirstOrDefaultAsync(p => p.Id == pollId, ct)
                   ?? throw new EntityNotFoundException(nameof(Poll), nameof(pollId), pollId.ToString());

        if (poll.Status == PollStatus.Expired || poll.ExpiresAt < DateTime.UtcNow)
            throw new LogicBrokenException("Poll is expired");

        if (optionIndex < 0 || optionIndex > poll.Options.Count)
            throw new LogicBrokenException("Invalid option");

        var voterKey = $"poll:{pollId}:voter:{voterFingerprint}";
        if (await _cache.KeyExistsAsync(voterKey))
            throw new LogicBrokenException("Already voted");

        var vote = new Vote(pollId, optionIndex, voterFingerprint, DateTime.UtcNow);
        await _dbContext.Votes.AddAsync(vote, ct);
        await _dbContext.CommitChangesAsync(ct);

        await _cache.SetAsync(voterKey, "1", TimeSpan.FromMilliseconds(poll.ExpiresAt.Millisecond));
        await _cache.IncrementHashAsync($"poll:{pollId}:counts", optionIndex.ToString(), 1);
    }

    public async Task<Dictionary<int, long>> GetVoteCountsAsync(Guid pollId, CancellationToken ct = default)
    {
        var countKey = $"poll:{pollId}:counts";
        var counts = await _cache.GetHashFieldsAsync(countKey);
        return counts.ToDictionary(kv => int.Parse(kv.Key), kv => kv.Value);
    }
}