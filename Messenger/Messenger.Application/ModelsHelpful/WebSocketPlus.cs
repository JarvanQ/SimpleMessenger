using System.Net.WebSockets;

namespace Messenger.Application.ModelsHelpful
{
    /// <summary>
    /// Вспомагательная модель для работы с WS
    /// </summary>
    public class WebSocketPlus
    {
        public WebSocketPlus(WebSocket WebSocket, bool IsSubscriber)
        {
            this.WebSocket = WebSocket;
            this.IsSubscriber = IsSubscriber;
        }

        public WebSocket WebSocket { get; set; }
        public bool IsSubscriber { get; set; }
    }
}
