

namespace BusinessLogic.IServices
{
    public interface IEnvironmentsProvider
    {

        /// <summary>
        /// http-адресс сервера
        /// </summary>
        public string ServerHttpUrl { get; }

        public string LogsPath { get; }

    }
}
