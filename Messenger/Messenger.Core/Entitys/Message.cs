
using System.ComponentModel.DataAnnotations;


namespace Messenger.Core.Entitys
{
    /// <summary>
    /// Сообщения, приходящие от клиентов
    /// </summary>
    public class Message: EntityBase
    {
        public int Number { get; set; }
        public DateTime ArrivalTime { get; set; } = DateTime.UtcNow;

        [MaxLength(128)]
        public string Kontent { get; set; } = string.Empty;

    }
}


