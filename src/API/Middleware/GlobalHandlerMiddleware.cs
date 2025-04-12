namespace EasyPoll.API.Middleware;

public sealed class GlobalHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalHandlerMiddleware> _logger;

    public GlobalHandlerMiddleware(RequestDelegate next, ILogger<GlobalHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var response = ex switch
        {
            EntityNotFoundException _ => BuildErrorResponseBody(
                StatusCodes.Status400BadRequest,
                ex.Message,
                ex.StackTrace),

            LogicBrokenException _ => BuildErrorResponseBody(
                StatusCodes.Status400BadRequest,
                ex.Message,
                ex.StackTrace),

            _ => BuildErrorResponseBody(
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred.",
                ex.StackTrace)
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = response.Status;
        return context.Response.WriteAsJsonAsync(response);
    }

    private static ErrorResponse BuildErrorResponseBody(int status, string message, string? stackTrace)
    {
        var errorResponse = new ErrorResponse
        {
            Ok = false,
            Status = status,
            Error = message,
            StackTrace = IsDevelopment ? stackTrace : null
        };

        return errorResponse;
    }

    private static bool IsDevelopment => string.Equals(
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
        "Development",
        StringComparison.OrdinalIgnoreCase);
}