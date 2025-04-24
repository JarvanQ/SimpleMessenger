
using System.Text.Json.Serialization;

namespace Messenger.Application.DTO
{
    public class MessageDTO
    {
        [JsonPropertyName("Id")]
        public int? Id { get; set; }

        [JsonPropertyName("Number")]
        public int Number { get; set; }

        [JsonPropertyName("ArrivalTime")]
        public DateTime ArrivalTime { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("Kontent")]
        public string Kontent { get; set; } = string.Empty;
    }
}
