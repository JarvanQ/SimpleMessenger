using BusinessLogic.DTO;

namespace BusinessLogic.IServices
{
    public interface IMessageService
    {

        /// <summary>
        /// Получить сообщения за последние 10 минут
        /// </summary>
        /// <returns></returns>
        public Task<List<MessageDTO>> GetMessagesFromLast10minutes();


        /// <summary>
        /// Получить сообщения за последние 10 минут
        /// </summary>
        /// <returns>ViewModel</returns>
        public Task<MessagesViewModel> GetMessagesFromLast10minutesVM();

    }
}
