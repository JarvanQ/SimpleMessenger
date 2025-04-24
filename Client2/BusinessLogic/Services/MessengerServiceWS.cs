using BusinessLogic.IServices;
using Data.DTO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using NLog;
using System.Net.WebSockets;
using System.Text.Json;


namespace BusinessLogic.Services
{
    public class MessengerServiceWS: BackgroundService
    {
        private readonly IEnvironmentsProvider _environmentsProvider;
        private readonly IWebSocketService _webSocketService;
        private readonly IHubContext<ChatHub> _hubContext;
        private static readonly ILogger _logger = LogManager.GetLogger(nameof(MessengerServiceWS));

        public MessengerServiceWS(IEnvironmentsProvider environmentsProvider,IWebSocketService webSocketService
            , IHubContext<ChatHub> hubContext
            )
        {
            _environmentsProvider = environmentsProvider;
            _webSocketService = webSocketService;
            _hubContext = hubContext;
        }

        public async Task ConnectWebSocket()
        {
            string conectionStr = _environmentsProvider.ServerWsUrl;
            await _webSocketService.ConnectAsync(conectionStr);
        }



        private async Task<bool> ReceiveHandler(string answerJson)
        {
            if (!JsonValidation(answerJson)) { return false; }

            Message? message = JsonSerializer.Deserialize<Message>(answerJson);
            if (message== null || !message.Valid() )
            { 

                return false;
            }

            try
            {
                await _hubContext.Clients.All.SendAsync("Receive", message);
                return true;
            }
            catch
            {
                _logger.Error("Ошибка рассылки сообщения по SignalR!!!!!");
                return false;
            }

        }


        private bool JsonValidation(string answerJson)
        {
            if (
                string.IsNullOrEmpty(answerJson) ||
                string.IsNullOrWhiteSpace(answerJson)
                ) 
            {
                _logger.Warn("Получили пустое сообщение.");
                return false;
            }
            return true;
        }


        #region BackgroundService

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_webSocketService._webSocketState != WebSocketState.Open)
                {
                    await ConnectWebSocket();

                    if (_webSocketService._webSocketState == WebSocketState.Open) { 
                            while (true)
                            {
                                var result = await _webSocketService.ReceiveAsync();
                                if (result.WebSocketReceiveResult.MessageType == WebSocketMessageType.Close)
                                {
                                   _logger.Warn("WS разорвал соединение.");
                                    break;
                                }
                                await ReceiveHandler(result.MessageFromBuffer);

                            }
                    }

                }

                await Task.Delay(20000, stoppingToken);
            }
            await Task.CompletedTask;
        }

        #endregion BackgroundService





    }
}
