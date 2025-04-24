

namespace Messenger.Application.DTO
{
    /// <summary>
    /// Id сообщения в БД
    /// </summary>
    public class MessageIdDTO
    {

        public MessageIdDTO(int? _id) 
        {
            if (_id==null) { Id = String.Empty; }
            else { Id = ((int)_id).ToString(); }
        }
        public string Id { get; set; }=String.Empty;
    }
}
