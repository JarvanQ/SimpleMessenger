using Messenger.Application.IServices;
using Microsoft.AspNetCore.Http;
using NLog;

namespace Messenger.Application.Middleware
{
    /// <summary>
    /// не используется 
    /// </summary>
    public class MyWebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebSocketService _webSocketService;
        private static readonly NLog.ILogger Logger = LogManager.GetLogger(nameof(MyWebSocketMiddleware));
        public MyWebSocketMiddleware(RequestDelegate next, IWebSocketService webSocketService)
        {
            _next = next;
            _webSocketService = webSocketService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Logger.Info($"Был запрос {context.Request.Path}");
            if (!context.WebSockets.IsWebSocketRequest) 
            {
                Logger.Info($"Был запрос 2 {context.Request.Path}");
                await _next(context); 
            }

            else if(context.Request.Path.StartsWithSegments(new PathString("/ws/sender")) )
            {
                await _webSocketService.HandleConnectionAsync(context);
            }
            else if (context.Request.Path.StartsWithSegments(new PathString("/ws/subscriber")))
            {
                await _webSocketService.HandleSubscriberConnectionAsync(context);
            }

        }


    }
}
