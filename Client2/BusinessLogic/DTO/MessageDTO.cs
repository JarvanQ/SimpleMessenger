
using System.ComponentModel;
using System.Text.Json.Serialization;


namespace BusinessLogic.DTO
{
    public class MessageDTO
    {
        [DisplayName("Id")]
        [JsonPropertyName("Id")]
        public int? Id { get; set; }

        [DisplayName("Номер")]
        [JsonPropertyName("Number")]
        public int Number { get; set; }

        [DisplayName("Время")]
        [JsonPropertyName("ArrivalTime")]
        public DateTime ArrivalTime { get; set; } = DateTime.UtcNow;

        [DisplayName("Содержимое")]
        [JsonPropertyName("Kontent")]
        public string Kontent { get; set; } = string.Empty;
    }
}
