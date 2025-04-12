namespace EasyPoll.Core.Entities;

public sealed class Poll
{
    private readonly List<string> _options = [];

    public Poll(
        string question,
        List<string> options,
        DateTime createdAt,
        DateTime expiresAt,
        PollStatus status,
        bool showResultsAfterVoteOnly)
    {
        Question = question;
        _options = options;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
        Status = status;
        ShowResultsAfterVoteOnly = showResultsAfterVoteOnly;
    }

#pragma warning disable CS8618, CS9264
    private Poll()
#pragma warning restore CS8618, CS9264
    {
    }

    public Guid Id { get; private set; }
    public string Question { get; private set; }
    public IReadOnlyList<string> Options => _options.AsReadOnly();
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public PollStatus Status { get; private set; }
    public bool ShowResultsAfterVoteOnly { get; private set; }
}