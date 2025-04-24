using BusinessLogic.IServices;
using Microsoft.Extensions.Configuration;
using NLog;



namespace BusinessLogic.Services
{
    public class EnvironmentsProvider : IEnvironmentsProvider
    {
        #region fields
        private readonly IConfiguration _config;
        private readonly string _serverHttpUrl;
        private readonly string _logsPath;
        #endregion fields


        public EnvironmentsProvider(IConfiguration config)
        {
            _config = config;

            _serverHttpUrl = GetUrlFromConfig(_config, "ServerHttpUrl");
            _logsPath = GetLogPath(_config);
        }

        #region props

        /// <summary>
        /// Http-адресс сервера
        /// </summary>
        public string ServerHttpUrl
        {
            get { return _serverHttpUrl; }
        }

        /// <summary>
        /// Место расположеия логов
        /// </summary>
        public string LogsPath
        {
            get { return _logsPath; }
        }

        #endregion props


        #region func

        /// <summary>
        /// Получить адресс подключения из файла конфигурации
        /// </summary>
        /// <param name="config"></param>
        /// <param name="connectionName">название параметра</param>
        /// <returns></returns>
        private string GetUrlFromConfig(IConfiguration config, string connectionName)
        {
            string? connection = Environment.GetEnvironmentVariable(connectionName);
            if (string.IsNullOrEmpty(connection))
            {
                connection = config[connectionName];
                if (string.IsNullOrEmpty(connection))
                {
                    return string.Empty;
                }
            }

            return connection;
        }

        private string GetLogPath(IConfiguration config)
        {
            string? logsPath = Environment.GetEnvironmentVariable("LogsPath");
            if (string.IsNullOrEmpty(logsPath))
            {
                logsPath = config["Logging:LogsPath"];
                if (string.IsNullOrEmpty(logsPath))
                {
                    return string.Empty;
                }
            }
            return logsPath;
        }

        #endregion func


    }
}
