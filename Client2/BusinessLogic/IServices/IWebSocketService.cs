using BusinessLogic.Models.UsefulModels;
using System.Net.WebSockets;


namespace BusinessLogic.IServices
{
    public interface IWebSocketService
    {
        WebSocketState _webSocketState { get; }
        Task ConnectAsync(string url);

        Task<WebSocketResult> ReceiveAsync();

    }
}
