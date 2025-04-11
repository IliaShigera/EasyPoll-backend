namespace EasyPoll.API;

internal static partial class Program
{
    private static void ConfigureServices(IServiceCollection sc, IConfiguration config)
    {
    }

    private static void ConfigureHttpPipeline(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
        }

        app.UseHttpsRedirection();
    }
}