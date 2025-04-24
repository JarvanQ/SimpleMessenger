using Microsoft.AspNetCore.Http;


namespace Messenger.Application.IServices
{
    public interface IWebSocketService
    {

        /// <summary>
        /// "Рукопожатие" с WS-клиентом отправителем
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task HandleConnectionAsync(HttpContext context);


        /// <summary>
        /// "Рукопожатие" с WS-клиентом получателем рассылок
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task HandleSubscriberConnectionAsync(HttpContext context);
        
    }
}
