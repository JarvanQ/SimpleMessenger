using System.Net.WebSockets;


namespace BusinessLogic.Models.UsefulModels
{
    /// <summary>
    /// ответ от WS
    /// </summary>
    public class WebSocketResult
    {
        private readonly WebSocketReceiveResult _webSocketReceiveResult;
        private readonly string _messageFromBuffer;

        public WebSocketResult(WebSocketReceiveResult webSocketReceiveResult, string messageFromBuffer) 
        {
            _webSocketReceiveResult = webSocketReceiveResult;
            _messageFromBuffer = messageFromBuffer;
        }

        /// <summary>
        /// ответ от WS
        /// </summary>
        public WebSocketReceiveResult WebSocketReceiveResult { get { return _webSocketReceiveResult; }  }

        /// <summary>
        /// расшифрованное сообщение от WS
        /// </summary>
        public string MessageFromBuffer { get { return _messageFromBuffer; }  }

    }
}
