using Messenger.Application.IServices;
using Microsoft.Extensions.Configuration;
using NLog;


namespace Messenger.Application.Services
{
    public class EnvironmentProvider : IEnvironmentProvider
    {
        IConfiguration _config;

        #region fields
        private readonly string _serverConnectionString;
        private readonly string _dbConnectionString;
        private readonly string _logsPath;
        #endregion fields

        private static readonly ILogger logger = LogManager.GetLogger(nameof(EnvironmentProvider));

        public EnvironmentProvider(IConfiguration config)
        {
            _config = config;

            _serverConnectionString = GetConnectionString(_config, "ServerConnection");
            _dbConnectionString = GetConnectionString(_config, "DbConnection");
            _logsPath = GetLogsPath(_config);

        }


        #region props

        /// <summary>
        /// адресс подключения к БД
        /// </summary>
        public string DbConnectionString {
            get { return _dbConnectionString; }
        }

        /// <summary>
        /// адресс подключения к серверу БД
        /// </summary>
        public string DbServerConnectionString 
        {
            get { return _serverConnectionString; }
        }

        /// <summary>
        /// месторасположение корневой папки логов
        /// </summary>
        public string LogsPath 
        {
            get { return _logsPath; }
        }

        #endregion props

        #region methods

        private string GetConnectionString(IConfiguration config, string connectionName) 
        {
            string? connection = Environment.GetEnvironmentVariable(connectionName);
            if (string.IsNullOrEmpty(connection)) 
            {
                connection = config.GetConnectionString(connectionName);
                if (string.IsNullOrEmpty(connection)) 
                {
                    logger.Error($"Не удалось получить значение '{connectionName}'.");
                    return string.Empty;
                }
            }

            return connection; 
        }

      
        private string GetLogsPath(IConfiguration config)
        {
            var tmpString = Environment.GetEnvironmentVariable("LogsPath");
            if (string.IsNullOrEmpty(tmpString))
            {
                tmpString = config["Logging:LogsPath"];
                if (string.IsNullOrEmpty(tmpString))
                {
                    logger.Error($"Не удалось получить месторасположение корневой папки логов.");
                    return string.Empty;
                }
            }


            return tmpString;

        }


        #endregion methods



    }
}
