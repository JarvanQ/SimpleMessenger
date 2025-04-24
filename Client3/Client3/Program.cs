using BusinessLogic.IServices;
using BusinessLogic.Logger;
using BusinessLogic.Services;
using NLog.Web;
using System.Net.Http.Headers;

namespace Client3
{
    public class Program
    {
        public static IEnvironmentsProvider environmentsProvider;
        public static IServiceProvider _serviceProvider;
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new AutomapperProfile());
            });

            IConfiguration? Configuration = builder.Configuration;
            environmentsProvider = new EnvironmentsProvider(Configuration);


            builder.Services.AddControllersWithViews();
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            
            ConfigureServices(builder.Services);
            ConfigureLogger(builder.Services);

            var app = builder.Build();

            app.UseExceptionHandler("/error");

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "docs";
            });

            
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }


        private static void ConfigureServices(IServiceCollection services)
        {

            services.AddSwaggerGen(options =>
            {

                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Description = "Web API",
                    Title = "Test work. Client #3",
                    Version = "v1",

                });
            });


            services.AddSingleton(environmentsProvider);
            services.AddSingleton<ILoggerInitializer, LoggerInitializer>();

            services.AddSingleton<HttpLoggingHandler>();

            services.AddHttpClient("ClientSender")
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri(environmentsProvider.ServerHttpUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                })
                 .AddHttpMessageHandler<HttpLoggingHandler>(); 

            services.AddSingleton<IHttpClientService>(new HttpClientService(services.BuildServiceProvider()));


            services.AddTransient<IMessageService, MessageService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        private static void ConfigureLogger(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var _loggerInitializer = serviceProvider.GetService<ILoggerInitializer>();
            _loggerInitializer?.ConfigureNLogger();
        }



    }
}
