using Messenger.Application.DTO;


namespace Messenger.Application.IServices
{
    public interface IMessageService
    {
        /// <summary>
        /// Отправить на сервер сообщение.
        /// Время устанавливает сервер. Порядковый номер - отправитель.
        /// Если введённый порядковый номер некорректный - поправляем.  
        /// </summary>
        /// <param name="message"></param>
        public Task<MessageIdDTO> PostMessage(NewMessageDTO message);

        /// <summary>
        /// Получить список сообщений за диапазон дат
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Task<List<MessageDTO>> GetMessagesByDates(DateTime start, DateTime end);




    }
}
