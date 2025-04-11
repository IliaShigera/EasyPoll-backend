namespace EasyPoll.API;

internal static partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureLogger(builder.Logging, builder.Configuration);
        ConfigureServices(builder.Services, builder.Configuration);
        
        var app = builder.Build();
        
        AutoMigrator.MigrateDatabase(app.Services);
        ConfigureHttpPipeline(app); 
        
        app.Run();
    }
}