using BusinessLogic.IServices;
using BusinessLogic.Models.UsefulModels;
using System.Net.WebSockets;
using System.Text;
using NLog;


namespace BusinessLogic.Services
{
    public class WebSocketService : IWebSocketService
    {
        private ClientWebSocket _webSocketClient = new ClientWebSocket();
        private readonly ILogger _logger;


        public WebSocketService() 
        {
            _logger = LogManager.GetCurrentClassLogger(); 
        }


        public WebSocketState _webSocketState { get { return _webSocketClient.State; } }

        public async Task ConnectAsync(string url)
        {
            try
            {

                if ((_webSocketClient == null) || (_webSocketClient.State != WebSocketState.None))
                { _webSocketClient = new ClientWebSocket(); }

                await _webSocketClient.ConnectAsync(new Uri(url), CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex,$"Ошибка подключения к WS '{url}' .");
            }
        }

        /// <summary>
        /// Получить ответ от WS
        /// </summary>
        /// <returns></returns>
        public async Task<WebSocketResult> ReceiveAsync()
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await _webSocketClient.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

            WebSocketResult webSocketResult = new WebSocketResult(result, message);


            return webSocketResult;
        }

    }
}
