
namespace Data.DTO
{
    public class MessageIdDTO
    {
        public MessageIdDTO() 
        {

        }
        public MessageIdDTO(int? _id)
        {
            Id = (_id != null) ? ((int)_id).ToString() : string.Empty;
        }
        public string Id { get; set; } = String.Empty;

        public bool Valid()
        {
            if (string.IsNullOrEmpty(Id) || string.IsNullOrWhiteSpace(Id) ||
                !int.TryParse(Id, out int number) || number < 0)
            {
                return false;
            }
            return true;

        }

    }
}
