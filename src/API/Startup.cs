namespace EasyPoll.API;

internal static partial class Program
{
    private static void ConfigureServices(IServiceCollection sc, IConfiguration config)
    {
        sc.AddSerilog();

        sc.AddInfrastructure(config);
    }

    private static void ConfigureHttpPipeline(WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
        }


        app.UseHttpsRedirection();
    }

    private static void ConfigureAppSettings(ConfigurationManager manager)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                  ?? throw new ApplicationException("App env is not specified");

        manager
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }

    private static void ConfigureLogger(ILoggingBuilder logging, IConfiguration config)
    {
        logging.ClearProviders();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();

        logging.AddSerilog(Log.Logger);
    }
}