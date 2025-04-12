namespace EasyPoll.API.ApiResponse;

internal sealed class ErrorResponse
{
    public required bool Ok { get; init; } = false;

    public required int Status { get; init; }

    public required string Error { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? StackTrace { get; init; } = null;
}