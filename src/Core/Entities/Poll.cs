namespace EasyPoll.Core.Entities;

public sealed class Poll
{
    private readonly List<string> _options = [];

    private Poll(
        string question,
        DateTime createdAt,
        DateTime expiresAt,
        bool showResultsAfterVoteOnly)
    {
        Question = question;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
        Status = PollStatus.Active;
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