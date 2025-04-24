using BusinessLogic.IServices;
using BusinessLogic.Services;
using BusinessLogic.Logger;
using BusinessLogic.Services.Providers;
using NLog.Web;

internal class Program
{

    public static IEnvironmentsProvider environmentsProvider;
    private static void Main(string[] args)
    {

        #pragma warning disable ASP0000 
        var builder = WebApplication.CreateBuilder(args);
        IConfiguration?  Configuration = builder.Configuration;
        environmentsProvider = new EnvironmentsProvider(Configuration);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Logging.ClearProviders();
        builder.Host.UseNLog();


        ConfigureServices(builder.Services);
        ConfigureLogger(builder.Services);

        var app = builder.Build();

        var serviceProvider = builder.Services.BuildServiceProvider();

        app.Run();

    #pragma warning restore ASP0000 

    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(environmentsProvider);
        services.AddSingleton<ILoggerInitializer,LoggerInitializer>();
        services.AddSingleton<IWebSocketService,WebSocketService>();
        services.AddSingleton<MessengerServiceWSprovider>();
        services.AddHostedService<MessengerServiceWS>();
    }

    private static void ConfigureLogger(IServiceCollection services) 
    {
        var serviceProvider = services.BuildServiceProvider();
        var _loggerInitializer = serviceProvider.GetService<ILoggerInitializer>();
        _loggerInitializer?.ConfigureNLogger();
    }

}