using Messenger.Core.Entitys;


namespace Messenger.Core.IRepository
{
    public interface IMessageRepository
    {
        /// <summary>
        /// Добавление записи в таблицу messages
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<int?> Add(Message message);

        /// <summary>
        /// Получение списка сообщений за отрезок времени
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Task<List<Message>> GetListByTimeInterval(DateTime start, DateTime end);
            
    }
}
