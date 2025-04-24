
using BusinessLogic.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Client3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController:  ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService) 
        {
            _messageService = messageService;
        }

        /// <summary>
        /// Получить сообщения за последние 10 минут
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMessagesFromLast10minutes")]
        public async Task<IActionResult> GetMessages()
        {
            var result = await _messageService.GetMessagesFromLast10minutes();
            return Ok(result);
        }

    }

    

}
