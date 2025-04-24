namespace Messenger.Application.IServices
{
    public interface IEnvironmentProvider
    {

        /// <summary>
        /// адресс подключения к БД
        /// </summary>
        public string DbConnectionString { get; }

        /// <summary>
        /// адресс подключения к серверу БД
        /// </summary>
        public string DbServerConnectionString { get; }
   

        /// <summary>
        /// путь к корневому каталогу логов
        /// </summary>
        string LogsPath { get;  }
    }
}