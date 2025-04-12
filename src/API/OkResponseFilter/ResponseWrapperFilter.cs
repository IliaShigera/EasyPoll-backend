namespace EasyPoll.API.OkResponseFilter;

internal sealed class ResponseWrapperFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var (data, statusCode) = context.Result switch
        {
            ObjectResult objectResult when IsSuccessfulStatusCode(objectResult.StatusCode)
                => (objectResult.Value, objectResult.StatusCode),

            StatusCodeResult statusCodeResult when IsSuccessfulStatusCode(statusCodeResult.StatusCode)
                => (null, statusCodeResult.StatusCode),

            _ => (null, null)
        };

        if (statusCode.HasValue)
        {
            var response = new OkResponse
            {
                Ok = true,
                Data = data,
                StatusCode = statusCode!.Value
            };

            context.Result = new JsonResult(response) { StatusCode = statusCode };
        }
    }


    private static bool IsSuccessfulStatusCode(int? statusCode) =>
        statusCode is >= 200 and < 300;
}