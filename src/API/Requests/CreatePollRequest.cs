namespace EasyPoll.API.Requests;

public sealed class CreatePollRequest
{
    public required string Question { get; init; }
    public required List<string> Options { get; init; }
    public required DateTime ExpiresAt { get; init; }
    public required bool ShowResultsAfterVoteOnly { get; init; }
}