

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Data.DTO
{
    /// <summary>
    /// Сообщение от WS-сервера
    /// </summary>
    public class Message
    {
        [DisplayName("Id")]
        [JsonPropertyName("Id")]
        public int? Id { get; set; }


        /// <summary>
        /// Номер сообщения.
        /// Задаёться клиентом-отправителем
        /// </summary>
        [DisplayName("Номер")]
        [JsonPropertyName("Number")]
        public int? Number { get; set; }

        /// <summary>
        /// Время отправки
        /// </summary>
        [DisplayName("Время")]
        [JsonPropertyName("ArrivalTime")]
        public DateTime? ArrivalTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Содержимое
        /// </summary>
        [DisplayName("Содержимое")]
        [JsonPropertyName("Kontent")]
        public string? Kontent { get; set; } = string.Empty;


        public bool Valid() 
        {
            if (
                 (Id == null || Id < 0)
                 || (Number == null || Number < 0)
                 || (ArrivalTime == null)
                 || (string.IsNullOrEmpty(Kontent))
                ) 
            {
                return false; 
            }

            return true;
        }


    }
}
