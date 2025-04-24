
namespace Data.DTO
{
    /// <summary>
    /// Сообщение
    /// </summary>
    public class MessageDTO
    {
        /// <summary>
        /// Номер сообщения. Задаётся клиентом.
        /// </summary>
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Kontent { get; set; } = string.Empty;


    }
}
