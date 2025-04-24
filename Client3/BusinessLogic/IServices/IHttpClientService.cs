
namespace BusinessLogic.IServices
{
    public interface IHttpClientService
    {
        /// <summary>
        /// Получить HTTP-клиент через фабрику
        /// </summary>
        /// <param name="_clientFactory"></param>
        void InitHttpClient(IHttpClientFactory? _clientFactory);

        /// <summary>
        /// Отправка Get-запросов
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<T> SendGetRequestAsync<T>(string url, string Id = null) where T : class;


        /// <summary>
        /// Отправить Post-запрос
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<T> SendPostRequestAsync<T>(string url, object data) where T : class;

    }
}
