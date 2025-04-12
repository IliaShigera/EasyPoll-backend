namespace EasyPoll.API.Controllers;

[ApiController]
[Route("api/polls")]
public sealed class PollController : Controller
{
    private readonly IPollService _pollService;
    private readonly IHubContext<VoteHub> _hubContext;

    public PollController(IPollService pollService, IHubContext<VoteHub> hubContext)
    {
        _pollService = pollService;
        _hubContext = hubContext;
    }

    [HttpPost("")]
    public async Task<ActionResult> CreatePollAsync([FromBody] CreatePollRequest request, CancellationToken ct)
    {
        var poll = await _pollService.CreatePollAsync(
            request.Question,
            request.Options,
            request.ExpiresAt,
            request.ShowResultsAfterVoteOnly,
            ct);

        return Ok(new { poll, url = $"{Request.Host}/poll/{poll.Id}" });
    }

    [HttpGet("{pollId:guid}")]
    public async Task<ActionResult> GetPollAsync(Guid pollId, CancellationToken ct)
    {
        var poll = await _pollService.GetPollAsync(pollId, ct);
        return Ok(poll);
    }

    [HttpGet("{pollId:guid}/results")]
    public async Task<ActionResult> GetPollResultsAsync(Guid pollId, CancellationToken ct)
    {
        var poll = await _pollService.GetPollAsync(pollId, ct);
        var counts = await _pollService.GetVoteCountsAsync(pollId, ct);

        return Ok(new { poll, VoteCounts = counts });
    }

    [HttpPost("{pollId:guid}/vote")]
    public async Task<ActionResult> VoteAsync(Guid pollId, [FromBody] VoteRequest request, CancellationToken ct)
    {
        var fingerprint = FingerprintGenerator.Generate(Request);
        await _pollService.VoteAsync(pollId, request.OptionIndex, fingerprint, ct);

        var voteCounts = await _pollService.GetVoteCountsAsync(pollId, ct);
        await _hubContext.Clients
            .Group(pollId.ToString())
            .SendAsync("ReceiveVoteUpdate", pollId, voteCounts, ct);

        return Ok();
    }
}