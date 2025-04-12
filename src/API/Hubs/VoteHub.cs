namespace EasyPoll.API.Hubs;

public sealed class VoteHub : Hub
{
    public async Task JoinPollAsync(string pollId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, pollId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}