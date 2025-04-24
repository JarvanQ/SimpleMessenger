
using BusinessLogic.IServices;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;
using NLog;


namespace BusinessLogic.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory? _clientFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        private HttpClient client;

        public HttpClientService(IServiceProvider serviceProvider) 
        {
            _logger = LogManager.GetCurrentClassLogger();

            _serviceProvider = serviceProvider;
            _clientFactory = _serviceProvider.GetService<IHttpClientFactory>();
            InitHttpClient(_clientFactory);
        }

        public void InitHttpClient(IHttpClientFactory? _clientFactory)
        {
            if (_clientFactory==null) 
            {
                _logger.Fatal("Не удалось получить фабрику HTTP-клиентов !!!!!!!");
                return;
            }

            try
            {
                client = _clientFactory.CreateClient("ClientSender");
            }
            catch (Exception ex) 
            {
                _logger.Fatal(ex,"Не удалось получить HTTP-клиента !!!!!!!");
            }


        }

        /// <summary>
        /// Извлечь из ответа объект указанного класса
        /// </summary>
        /// <typeparam name="T">требуемый класс</typeparam>
        /// <param name="response">http-ответ</param>
        /// <returns></returns>
        private async Task<T?> GetObjectFromPostResponse<T>(HttpResponseMessage response) where T : class
        {
            var resultContent = await response.Content.ReadAsStringAsync();

            if ( string.IsNullOrEmpty(resultContent)) 
            {
                _logger.Info($"{response.RequestMessage}: получили пустой отвевет. ");
                return null;
            }
            try
            {
                var result = JsonSerializer.Deserialize<T>(resultContent);
                return result;

            }
            catch(Exception ex) 
            {
                _logger.Error(ex,$"{response.RequestMessage}: не удалось десериализовать обьект. ");
                return null;
            }
        }

        #region CRUD

        public async Task<T> SendGetRequestAsync<T>(string url, string Id = null) where T : class
        {
            if (Id != null) { url = url + "/" + Id; }

            var getResult = await client.GetAsync(url);
            var obj = await GetObjectFromPostResponse<T>(getResult);

            return obj;

        }

        public async Task<T> SendPostRequestAsync<T>(string url, object data) where T : class
        {
            var jsonData = JsonSerializer.Serialize(data);
            _logger.Info($"АДРЕСС СЕРВЕРА = {url}");
            try
            {
                var getResult = await client.PostAsync(url, new StringContent(jsonData, Encoding.UTF8, "application/json"));
                var obj = await GetObjectFromPostResponse<T>(getResult);

                return obj;
            }
            catch (Exception ex) 
            {
                return null;
            }


           
        }


        #endregion CRUD

    }
}
