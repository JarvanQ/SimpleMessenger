
namespace BusinessLogic.IServices
{
    public interface IEnvironmentsProvider
    {

        /// <summary>
        /// http-адресс сервера
        /// </summary>
        public string ServerHttpUrl { get; }

        /// <summary>
        /// webSocket-адресс сервера
        /// </summary>
        public string ServerWsUrl { get; }

        public string LogsPath { get; }

        public string MessageNumberStorage { get; }

    }
}
