namespace EasyPoll.Core.Interfaces;

public interface IPollService
{
    Task<Poll> CreatePollAsync(
        string question,
        List<string> options,
        DateTime expiresAt,
        bool showResultsAfterVoteOnly,
        CancellationToken ct = default);

    Task<Poll> GetPollAsync(Guid pollId, CancellationToken ct = default);
    
    Task VoteAsync(
        Guid pollId,
        int optionIndex,
        string voterFingerprint,
        CancellationToken ct = default);

    Task<Dictionary<int, long>> GetVoteCountsAsync(Guid pollId, CancellationToken ct = default);
}