using BusinessLogic.IServices;
using Data.DTO;
using System.Text.Json;
using BusinessLogic.Services.Providers;
using Microsoft.Extensions.Hosting;
using System.Net.WebSockets;
using NLog;

namespace BusinessLogic.Services
{
    public partial class MessengerServiceWS : BackgroundService, IMessengerService
    {
        private readonly IEnvironmentsProvider _environmentsProvider;
        private readonly IWebSocketService _webSocketService;
        private readonly MessengerServiceWSprovider _provider;
        private static readonly ILogger _logger = LogManager.GetLogger(nameof(MessengerServiceWS));

        public MessengerServiceWS(IEnvironmentsProvider environmentsProvider, 
            IWebSocketService webSocketService, MessengerServiceWSprovider provider) 
        {
            _environmentsProvider = environmentsProvider;
            _provider = provider;
            _webSocketService = webSocketService;
        }

        public async Task ConnectWebSocket() 
        {
            string conectionStr = _environmentsProvider.ServerWsUrl;
            await _webSocketService.ConnectAsync(conectionStr);
        }

        //отпраавить сообщение на WS
        public async Task SendMessageWS(MessageDTO message)
        {
            string json = JsonSerializer.Serialize(message);

            await _webSocketService.SendMessageAsync(json);
            
            var result = ReceiveFromWS();
        }

        /// <summary>
        /// Получение ответа от WS-сервера
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ReceiveFromWS()
        {
            _logger.Info($"Получили сообщение от {_environmentsProvider.ServerWsUrl}.");
            string receive = await _webSocketService.ReceiveAsync();
            bool receiveResult = await ReceiveHandler(receive);

            return receiveResult;
        }


        public async Task<bool> SendMessage()
        {
            bool result = false;
            SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
            await semaphoreSlim.WaitAsync();

            try 
            {
                var message = GenerateRandomMessage();
                await SendMessageWS(message);
            }
            finally
            {
                semaphoreSlim.Release();
            }

            return result;
        }


        /// <summary>
        /// Обработчик ответа от WS-сервера
        /// </summary>
        /// <param name="answerJson">ответ</param>
        /// <returns></returns>
        private async Task<bool> ReceiveHandler(string answerJson) 
        {
            if (!JsonValidation(answerJson)) 
            { 
                return false;
            }

            try
            {
                MessageNumberDTO? messageNumberDTO = JsonSerializer.Deserialize<MessageNumberDTO>(answerJson);
                if (messageNumberDTO == null || !messageNumberDTO.Valid())
                { return false; }

                var result = await _provider.SaveMessageNumberСarefully(messageNumberDTO.Number);
                return result;
            }
            catch { 
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
                _logger.Warn("Получили пустое ссобщение.");
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
                    _logger.Info("Подключаемся к WS");
                    await ConnectWebSocket();
                }

                else if (_webSocketService._webSocketState == WebSocketState.Open)
                {
                    await SendMessage();
                }
                await Task.Delay(20000, stoppingToken);
            }
            await Task.CompletedTask;
        }

        #endregion BackgroundService



    }
}
