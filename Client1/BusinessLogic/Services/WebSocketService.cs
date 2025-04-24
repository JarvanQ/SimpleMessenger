using BusinessLogic.IServices;
using System.Net.WebSockets;
using System.Text;
using NLog;

namespace BusinessLogic.Services
{
    public class WebSocketService : IWebSocketService
    {
        ILogger _logger = LogManager.GetCurrentClassLogger();
        private ClientWebSocket _webSocketClient = new ClientWebSocket();

        private readonly IEnvironmentsProvider _environmentsProvider;

        public WebSocketService(IEnvironmentsProvider environmentsProvider) 
        {
            _environmentsProvider = environmentsProvider;
        }


        public WebSocketState _webSocketState { get { return _webSocketClient.State; } }

        public async Task ConnectAsync(string url)
        {
            try
            {

                if ((_webSocketClient == null)||(_webSocketClient.State!= WebSocketState.None)) 
                { 
                    _webSocketClient = new ClientWebSocket();
                }

                await _webSocketClient.ConnectAsync(new Uri(url), CancellationToken.None);
            }
            catch (Exception ex) 
            {
                _logger.Fatal(ex,$"Не удалось подключиться к WS '{url}' !!!!!");
            }
        }

        public async Task DisconnectAsync()
        {
            if (_webSocketClient != null)
            {
                try
                {
                    await _webSocketClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                }
                finally 
                {
                    _webSocketClient.Dispose();
                }
               
            }
        }

        /// <summary>
        /// Получить ответ от WS
        /// </summary>
        /// <returns></returns>
        public async Task<string> ReceiveAsync()
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await _webSocketClient.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            return Encoding.UTF8.GetString(buffer, 0, result.Count);
        }

        public async Task SendMessageAsync(string message)
        {
            if (_webSocketClient.State == WebSocketState.Open)
            {
                
                var buffer = Encoding.UTF8.GetBytes(message);
                try
                {
                    _logger.Info("Отправка сообщения");
                    await _webSocketClient.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch (WebSocketException xx) 
                {
                    _logger.Error(xx, $"Ошибка отправки сообщения на WS '{_environmentsProvider.ServerWsUrl}'");
                    if ( xx.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                    {
                        _logger.Error( $"WS '{_environmentsProvider.ServerWsUrl}' разорвал соединение. ");
                        await DisconnectAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Ошибка отправки сообщения на WS '{_environmentsProvider.ServerWsUrl}'");
                    await DisconnectAsync();
                }

            }
        }

    }

}
