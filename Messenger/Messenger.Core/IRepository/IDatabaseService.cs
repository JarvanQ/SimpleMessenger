using Messenger.Core.IRepository;

namespace Messenger.Core
{
    public interface IDatabaseService
    {
       
        public IMessageRepository MessageRepository { get; }

        /// <summary>
        /// Инициализация БД и таблицы
        /// </summary>
        /// <returns></returns>
        public bool InitDB();

    }
}
