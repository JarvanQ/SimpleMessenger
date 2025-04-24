

namespace BusinessLogic.DTO
{
    //не используеться
    public class NewMessageDTO
    {
        public string Number { get; set; } = string.Empty;
        public string Kontent { get; set; } = string.Empty;

        public bool Valid()
        {
            if (string.IsNullOrEmpty(Kontent) || string.IsNullOrWhiteSpace(Kontent) ||
                !int.TryParse(Number, out int number) || number < 0)
            {
                return false;
            }
            return true;

        }

    }
}
