using NLog;

namespace BusinessLogic.Services
{
    public class HttpLoggingHandler : DelegatingHandler
    {
        private readonly ILogger _logger;
        public HttpLoggingHandler() 
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        protected override async Task<HttpResponseMessage> SendAsync(
                                        HttpRequestMessage request,
                                        CancellationToken cancellationToken)
        {

            try
            {
                _logger.Info($"Отправка запроса {request.RequestUri}.");

                var result = await base.SendAsync(request, cancellationToken);

                result.EnsureSuccessStatusCode();

                _logger.Info($"Успешно. Запрос {request.RequestUri}.");

                return result;
            }
            catch (Exception e)
            {
                _logger.Error(e, $"HTTP request {request.RequestUri} failed");

                throw;
            }
        }
    }
}
