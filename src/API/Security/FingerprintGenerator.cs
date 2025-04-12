namespace EasyPoll.API.Security;

public static class FingerprintGenerator
{
    public static string Generate(HttpRequest request)
    {
        var ip = request.HttpContext.Connection.RemoteIpAddress!.ToString();
        var userAgent = request.Headers.UserAgent.ToString();
        return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes($"{ip}:{userAgent}")));
    }
}