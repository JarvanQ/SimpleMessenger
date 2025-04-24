
using Messenger.Application.ModelsHelpful;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace Messenger.Application.Services
{
    public class WebSocketProvider
    {
        private readonly ConcurrentDictionary<Guid, WebSocketPlus> _connections = new ();


        /// <summary>
        /// Добавление в пул сокета
        /// </summary>
        /// <param name="webSocket">сокет</param>
        /// <param name="isSubscriber">true-получатель рассылок, false - отправитель</param>
        /// <returns></returns>
        public Guid AddConnection(WebSocket webSocket, bool isSubscriber)
        {

            if (!IsWebSocketExists(webSocket))
            {
                WebSocketPlus webSocketPlus = new WebSocketPlus(webSocket,isSubscriber);
                Guid connectionKey = Guid.NewGuid();
                _connections.TryAdd(connectionKey, webSocketPlus);

                return connectionKey;
            }
            return Guid.Empty;
        }


        /// <summary>
        /// Удалить из пула сокет
        /// </summary>
        /// <param name="connectionId"></param>
        public void RemoveConnection(Guid connectionId)
        {
            _connections.TryRemove(connectionId, out _);
        }

       
        /// <summary>
        /// Отправка по вебсокету сообщения конкретному клиенту
        /// </summary>
        /// <param name="connectionId">Ключь в словаре сокетов</param>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        public async Task SendMessageToClientAsync(Guid connectionId, string message)
        {
            if (_connections.TryGetValue(connectionId, out var webSocketPlus) && webSocketPlus.WebSocket.State == WebSocketState.Open)
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                await webSocketPlus.WebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        /// <summary>
        /// Рассылка сообщения всем подписчкам
        /// </summary>
        /// <param name="messageJson"></param>
        /// <returns></returns>
        public async Task BroadcastMessageAsync(string messageJson)
        {
            if (!_connections.Values.Any(x => x.WebSocket.State == WebSocketState.Open && x.IsSubscriber == true))
            { return; }

            var buffer = Encoding.UTF8.GetBytes(messageJson);

            foreach (var connection in _connections.Values)
            {
                if (connection.WebSocket.State == WebSocketState.Open && connection.IsSubscriber == true)
                {
                    await connection.WebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        /// <summary>
        /// Проверка есть ли в нашем пуле такой WS
        /// </summary>
        /// <param name="webSocket"></param>
        /// <returns></returns>
        private bool IsWebSocketExists(WebSocket webSocket)
        {
            foreach (var connection in _connections.Values)
            {
                if (connection.WebSocket == webSocket)
                {
                    return true; 
                }
            }
            return false; 
        }
    }

    



}
