
using System.Text.Json.Serialization;


namespace Messenger.Application.DTO
{
    /// <summary>
    /// Номер сообщения. 
    /// Приходит от первого клиента
    /// </summary>
    public class MessageNumberDTO
    {
        public MessageNumberDTO(string number) 
        {
            Number = number;
        }

        [JsonPropertyName("Number")]
        public string Number { get; set; }
    }
}
