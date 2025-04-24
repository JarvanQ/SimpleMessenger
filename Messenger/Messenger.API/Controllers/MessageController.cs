using Messenger.Application.DTO;
using Messenger.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Messenger.API.Controllers
{
    [ApiController]
   // [Route("[controller]")]
    [Route("api/[controller]")]

    public class MessageController : ControllerBase
    {

        private readonly IMessageService _messageService;
        private static readonly NLog.ILogger Logger = LogManager.GetLogger(nameof(MessageController));
        public MessageController( IMessageService messageService) 
        {
            _messageService = messageService;
        }

        /// <summary>
        /// Получить сообщения за указанный отрезок времени
        /// </summary>
        /// <param name="form">Модель с-по</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetMessagesByDate")]
        public async Task<IActionResult> GetMessages([FromBody] MessageFilterModel form)
        {
            try
            {
                Logger.Info("Получили запрос на получение сообщений");
                List<MessageDTO> result = await _messageService.GetMessagesByDates(form.DateFrom, form.DateTo);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return Ok(null);
            }
        }

        /// <summary>
        /// Отправить новое сообщение
        /// </summary>
        /// <param name="message">обьект сообщения</param>
        /// <returns></returns>
        [HttpPost]
        [Route("PostMessage")]
        public async Task<IActionResult> PostMessage([FromBody] NewMessageDTO message)
        {
            Logger.Info($"Получили сообщение {message.Number}");
            MessageIdDTO result = await _messageService.PostMessage(message);

            bool flag = int.TryParse(result.Id, out var messageId);


            return (flag) ? Ok(result) : BadRequest(result);

        }




    }
}
