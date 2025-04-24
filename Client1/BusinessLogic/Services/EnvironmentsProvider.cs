using BusinessLogic.IServices;
using Microsoft.Extensions.Configuration;

namespace BusinessLogic.Services
{
    public class EnvironmentsProvider:IEnvironmentsProvider
    {

        #region fields
        private readonly IConfiguration _config;
        private readonly string _serverHttpUrl;
        private readonly string _serverWsUrl;
        private readonly string _logsPath;
        private readonly string _messageNumberStorage;

        #endregion fields


        public EnvironmentsProvider(IConfiguration config)
        {
            _config = config;
            _serverHttpUrl = GetUrlFromConfig(_config, "ServerHttpUrl");
            _serverWsUrl = GetUrlFromConfig(_config, "ServerWsUrl");
            _logsPath = GetLogPath(_config);
            _messageNumberStorage = GetMessageNumberStorage(_config);

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
        /// WebSocket-адресс сервера
        /// </summary>
        public string ServerWsUrl
        {
            get { return _serverWsUrl; }
        }

        public string LogsPath 
        {
            get { return _logsPath; }
        }

        public string MessageNumberStorage 
        {
            get { return _messageNumberStorage; }
        }

        #endregion props

        #region func
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
        
        private string GetMessageNumberStorage(IConfiguration config)
        {
            string? messageNumberStorage = Environment.GetEnvironmentVariable("NumberStorage");
            if (string.IsNullOrEmpty(messageNumberStorage))
            {
                messageNumberStorage = config["NumberStorage"];
                if (string.IsNullOrEmpty(messageNumberStorage))
                {
                    //TODO : logging
                    return "messageNumber.txt";
                }
            }
            
            return messageNumberStorage;
        }

        #endregion func


    }

}
