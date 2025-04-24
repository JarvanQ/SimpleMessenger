

namespace BusinessLogic.DTO
{
    public class MessagesViewModel
    {
        public MessagesViewModel()
        {
            Messages = new List<MessageDTO>();
        }

        public bool ResultFlag { get; set; }
        public List<MessageDTO> Messages { get; set; }

    }
}
