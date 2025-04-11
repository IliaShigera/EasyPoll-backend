namespace EasyPoll.API;

internal static partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        ConfigureServices(builder.Services, builder.Configuration);
        
        var app = builder.Build();
        ConfigureHttpPipeline(app);
        app.Run();
    }
}