using AutoMapper;
using Messenger.Application.DTO;
using Messenger.Application.IServices;
using Messenger.Core;
using Messenger.Core.Entitys;
using NLog;

namespace Messenger.Application.Services
{
    public class MessageService : IMessageService
    {
        private static readonly ILogger logger = LogManager.GetLogger(nameof(MessageService));
        private readonly IDatabaseService _databaseService;
        private readonly IMapper _mapper;
        private readonly IEnvironmentProvider _environmentProvider;

        public MessageService(IDatabaseService databaseService, IMapper mapper, IEnvironmentProvider environmentProvider) 
        {
            _databaseService = databaseService;
            _mapper = mapper;
            _environmentProvider = environmentProvider;
        }

        public async Task<List<MessageDTO>> GetMessagesByDates(DateTime start, DateTime end)
        {
            List<Message> messages = await _databaseService.MessageRepository.GetListByTimeInterval(start, end);
            try
            {
                List<MessageDTO> messageDTOs = _mapper.Map<List<MessageDTO>>(messages);

                return messageDTOs;
            }
            catch (Exception ex)
            {
                logger.Error("Не удалось считать сообщения за отрезок времени");
                return null; 
            }
        }

        public async Task<MessageIdDTO> PostMessage(NewMessageDTO message)
        {
            Message _message = _mapper.Map<Message>(message);
            int? result = await _databaseService.MessageRepository.Add(_message);
            MessageIdDTO _messageIdDTO = new MessageIdDTO(result);
            return _messageIdDTO;
        }

    }
}
