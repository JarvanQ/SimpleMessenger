using Messenger.Application.IServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Application.Middleware
{
    public static class MyHub
    {
        public static WebApplication MapMyHub(this WebApplication app)
        {
            app.Map("/ws/sender", async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    await app.Services.GetRequiredService<IWebSocketService>().HandleConnectionAsync(context);
                }

            });
            app.Map("/ws/subscriber", async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    await app.Services.GetRequiredService<IWebSocketService>().HandleSubscriberConnectionAsync(context);
                }

            });

            return app;
        }
    }
}
