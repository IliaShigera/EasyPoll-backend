namespace EasyPoll.Core.Entities;

public sealed class Vote
{
    private Vote(Guid pollId, int optionIndex, string voterFingerprint, DateTime timestamp)
    {
        PollId = pollId;
        OptionIndex = optionIndex;
        VoterFingerprint = voterFingerprint;
        Timestamp = timestamp;
    }

#pragma warning disable CS8618, CS9264
    private Vote()
#pragma warning restore CS8618, CS9264
    {
    }

    public Guid PollId { get; private set; }
    public int OptionIndex { get; private set; }
    public string VoterFingerprint { get; private set; }
    public DateTime Timestamp { get; private set; }
}