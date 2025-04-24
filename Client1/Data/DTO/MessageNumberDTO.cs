
using System.Text.Json.Serialization;


namespace Data.DTO
{
    /// <summary>
    /// Номер сообщения. 
    /// </summary>
    public class MessageNumberDTO
    {
        public MessageNumberDTO(string number)
        {
            Number = number;
        }

        [JsonPropertyName("Number")]
        public string Number { get; set; }


        public bool Valid()
        {
            if (string.IsNullOrEmpty(Number) || string.IsNullOrWhiteSpace(Number) ||
                !int.TryParse(Number, out int number) || number < 0)
            {
                return false;
            }
            return true;

        }

    }
}
