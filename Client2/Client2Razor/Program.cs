using BusinessLogic.IServices;
using BusinessLogic.Logger;
using BusinessLogic.Services;
using NLog.Web;

internal class Program
{
    public static IConfiguration? Configuration { get; set; }
    public static IEnvironmentsProvider environmentsProvider;

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        IConfiguration? Configuration = builder.Configuration;
        environmentsProvider = new EnvironmentsProvider(Configuration);

        builder.Services.AddRazorPages();
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();

        ConfigureServices(builder.Services);
        ConfigureLogger(builder.Services);

        var serviceProvider = builder.Services.BuildServiceProvider();

        var app = builder.Build();


        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        #pragma warning disable ASP0014
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<ChatHub>("/chat");
        });

        app.MapRazorPages();

        var _loggerInitializer = serviceProvider.GetService<ILoggerInitializer>();
        app.Run();
    }


    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddSignalR();

        var serviceProvider = services.BuildServiceProvider();

        services.AddSingleton(environmentsProvider);
        services.AddSingleton<ILoggerInitializer, LoggerInitializer>();
        services.AddSingleton<IWebSocketService, WebSocketService>();

        services.AddHostedService<MessengerServiceWS>();

    }

    private static void ConfigureLogger(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var _loggerInitializer = serviceProvider.GetService<ILoggerInitializer>();
        _loggerInitializer?.ConfigureNLogger();
    }
}