

namespace Messenger.Application.DTO
{
    /// <summary>
    /// Фильтр для получения списка сообщений
    /// </summary>
    public class MessageFilterModel
    {
        /// <summary>
        /// Дата "С"
        /// </summary>
        public DateTime DateFrom { get; set; }


        /// <summary>
        /// Дата "По"
        /// </summary>
        public DateTime DateTo { get; set; }

    }
}
