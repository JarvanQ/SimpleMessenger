

namespace BusinessLogic.DTO
{
    /// <summary>
    /// Фильтр для получения списка сообщений
    /// </summary>
    public class MessageFilterModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

    }
}
