using AutoMapper;
using BusinessLogic.DTO;
using BusinessLogic.Helpers;
using BusinessLogic.IServices;
using NLog;


namespace BusinessLogic.Services
{
    public class MessageService : IMessageService
    {
        private readonly IHttpClientService _httpClient;
        private readonly IEnvironmentsProvider _environmentsProvider;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public MessageService(IHttpClientService httpClient, IEnvironmentsProvider environmentsProvider, IMapper mapper) 
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _environmentsProvider = environmentsProvider;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public async Task<List<MessageDTO>> GetMessagesFromLast10minutes()
        {
            string url = Constants.Strings.ServerUrls.MessageController.getMessagesByDate;

            MessageFilterModel filterModel = new MessageFilterModel();
            filterModel.DateTo = DateTime.UtcNow;
            filterModel.DateFrom = filterModel.DateTo.AddMinutes(-10);

            var res = await _httpClient.SendPostRequestAsync<List<MessageDTO>>(url, filterModel);

            return res;
        }

        public async Task<MessagesViewModel> GetMessagesFromLast10minutesVM() 
        {
            MessagesViewModel result = new MessagesViewModel();
            try
            {
                List<MessageDTO> messageDTOs = await GetMessagesFromLast10minutes();
                result.Messages = messageDTOs;
                result.ResultFlag = true;
            }
            catch (Exception ex) 
            {
                _logger.Error(ex, $"Ошибка получения сообщений.");
                result.ResultFlag = false;
            }

            return result;
        }










    }
}
