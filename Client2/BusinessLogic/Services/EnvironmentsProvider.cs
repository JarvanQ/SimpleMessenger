using BusinessLogic.IServices;
using Microsoft.Extensions.Configuration;


namespace BusinessLogic.Services
{
    public class EnvironmentsProvider : IEnvironmentsProvider
    {
        #region fields
        private readonly IConfiguration _config;
        private readonly string _logsPath;
        private readonly string _serverWsUrl;
        #endregion fields


        public EnvironmentsProvider(IConfiguration config) 
        {
            _config = config;
            _logsPath = GetLogPath(_config);
            _serverWsUrl = GetUrlFromConfig(_config, "ServerWsUrl");
        }


        #region methods

        private string GetLogPath(IConfiguration config)
        {
            string? logsPath = Environment.GetEnvironmentVariable("LogsPath");
            if (string.IsNullOrEmpty(logsPath))
            {
                logsPath = config["Logging:LogsPath"];
                if (string.IsNullOrEmpty(logsPath))
                {
                    //TODO : logging
                    return string.Empty;
                }
            }

            return logsPath;

        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="config"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        private string GetUrlFromConfig(IConfiguration config, string connectionName)
        {
            string? connection = Environment.GetEnvironmentVariable(connectionName);
            if (string.IsNullOrEmpty(connection))
            {
                connection = config[connectionName];
                if (string.IsNullOrEmpty(connection))
                {
                    //TODO : logging
                    return string.Empty;
                }
            }

            return connection;

        }

        #endregion methods



        #region props
        public string LogsPath 
        {
            get { return _logsPath; }
        }

        /// <summary>
        /// WebSocket-адресс сервера
        /// </summary>
        public string ServerWsUrl
        {
            get { return _serverWsUrl; }
        }

        #endregion props



    }
}
