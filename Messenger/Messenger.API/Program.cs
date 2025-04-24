
using Messenger.Application.Services;
using Messenger.Application.IServices;
using Messenger.DB;
using Messenger.Core;
using Messenger.Application.Middleware;
using NLog.Web;
using Microsoft.AspNetCore.Rewrite;

internal class Program
{
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            var basePath = AppContext.BaseDirectory;

            var xmlPath = Path.Combine(basePath, "MessengerAPI.xml");
            options.IncludeXmlComments(xmlPath,true);

            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
            {
                Description = "Web API",
                Title = "Test Work",
                Version = "v1",
                
            });


        });

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile(new AutomapperProfile());
        });

        builder.Logging.ClearProviders();
        builder.Host.UseNLog();


        ConfigureServices(builder.Services);
        ConfigureLogger(builder.Services);

        var app = builder.Build();

        IServiceProvider serviceProvider = app.Services;
        EnvironmentProvider? environmentDependentOptions = serviceProvider.GetService<EnvironmentProvider>();

        IDatabaseService? databaseService = serviceProvider.GetService<IDatabaseService>();
        if (databaseService == null) { app.StopAsync(); }
        var _loggerInitializer = serviceProvider.GetService<ILoggerInitializer>();

        IWebSocketService webSocketService = serviceProvider.GetService<IWebSocketService>();

        var result = databaseService.InitDB();

        if (!result)
        {
            app.Logger.LogCritical("Не удалось инициализировать БД. Приложение остановлено.");
            return;
        }


        app.UseSwagger();
        app.UseSwaggerUI();

        var option = new RewriteOptions();
        option.AddRedirect("^$", "swagger");
        app.UseRewriter(option);


        app.UseAuthorization();

        app.MapControllers();

        var webSocketOptions = new WebSocketOptions()
        {
            KeepAliveInterval = TimeSpan.FromSeconds(120),
            ReceiveBufferSize = 4 * 1024
        };


        app.UseWebSockets(webSocketOptions);

        app.MapMyHub();

        
        //app.UseMiddleware<MyWebSocketMiddleware>();


        app.Run();

    }


    private static void ConfigureServices(IServiceCollection services) 
    {
        services.AddSingleton<ILoggerInitializer, LoggerInitializer>();
        services.AddSingleton<IEnvironmentProvider,EnvironmentProvider>();

        services.AddSingleton<WebSocketProvider>();
        services.AddSingleton<IWebSocketService, WebSocketService>(); 

        services.AddTransient<IDatabaseService,DatabaseService>();
        services.AddTransient<IMessageService, MessageService>();

    }

    private static void ConfigureLogger(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var _loggerInitializer = serviceProvider.GetService<ILoggerInitializer>();
        _loggerInitializer?.ConfigureNLogger();
    }

}