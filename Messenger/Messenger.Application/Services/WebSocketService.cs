using AutoMapper;
using Messenger.Application.DTO;
using Messenger.Application.IServices;
using Messenger.Core;
using Messenger.Core.Entitys;
using Microsoft.AspNetCore.Http;
using NLog;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Messenger.Application.Services
{
    public class WebSocketService : IWebSocketService
    {

        private readonly WebSocketProvider _webSocketProvider;
        private static readonly ILogger logger = LogManager.GetLogger(nameof(WebSocketService));
        private readonly IDatabaseService _databaseService;
        private readonly IMapper _mapper;

        public WebSocketService(WebSocketProvider socketProvider, IDatabaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
            _webSocketProvider = socketProvider;
        }


        /// <summary>
        /// Обработчик входящих сообщений
        /// </summary>
        /// <param name="connectionKey"></param>
        /// <param name="messageJson"></param>
        /// <returns></returns>
        private async Task ProcessMessageAsync(Guid connectionKey, string messageJson)
        {

            NewMessageDTO? newMessageDTO = JsonSerializer.Deserialize<NewMessageDTO>(messageJson);
            if (newMessageDTO == null || !newMessageDTO.Valid())
            {
                logger.Warn("Пришло некорректное сообщение!");
                return;
            }

            Message _message = _mapper.Map<Message>(newMessageDTO);
            int? resultId = await _databaseService.MessageRepository.Add(_message);
            if (resultId == null) 
            {
                logger.Warn("Не удалось сохранить сообщение!");
                return; 
            }
            _message.Id = (int)resultId;


            MessageNumberDTO messageNumberDTO = new MessageNumberDTO(newMessageDTO.Number);

            string json = JsonSerializer.Serialize(messageNumberDTO);

            #pragma warning disable CS4014
            _webSocketProvider.SendMessageToClientAsync(connectionKey, json);

            string broadcastMessageJson = JsonSerializer.Serialize(_message);
            _webSocketProvider.BroadcastMessageAsync(broadcastMessageJson);

            #pragma warning restore CS4014

        }


        public async Task HandleConnectionAsync(HttpContext context)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();

            var buffer = new byte[1024 * 4];

            var connectionKey = _webSocketProvider.AddConnection(webSocket,false);

            try
            {
                while (true)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }

                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    await ProcessMessageAsync(connectionKey, message);

                }
            }
            finally
            {
                _webSocketProvider.RemoveConnection(connectionKey);
            }
        }

        public async Task HandleSubscriberConnectionAsync(HttpContext context) 
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();

            var buffer = new byte[1024 * 4];

            var connectionKey = _webSocketProvider.AddConnection(webSocket,true);

            try
            {
                while (true)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }

                }
            }
            finally
            {
                _webSocketProvider.RemoveConnection(connectionKey);
            }

        }



    }
}
