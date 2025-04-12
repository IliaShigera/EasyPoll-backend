namespace EasyPoll.API.ApiResponse;

internal sealed class OkResponse
{
    public required bool Ok { get; init; }
    public required int StatusCode { get; init; }
    public required object? Data { get; init; }
}