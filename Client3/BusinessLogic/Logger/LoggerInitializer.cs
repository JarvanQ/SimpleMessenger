using BusinessLogic.IServices;
using BusinessLogic.Services;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;


namespace BusinessLogic.Logger
{
    /// <summary>
    /// Сервис для настрофки NLoger-а
    /// </summary>
    public class LoggerInitializer : ILoggerInitializer, IDisposable
    {
        private readonly IEnvironmentsProvider _environmentsProvider;
        public LoggerInitializer(IEnvironmentsProvider environmentsProvider)
        {
            _environmentsProvider = environmentsProvider;

        }

        public void ConfigureNLogger()
        {
            LogManager.Configuration = CreateLoggingConfiguration();
        }

        private LoggingConfiguration CreateLoggingConfiguration()
        {
            var loggingConfiguration = new LoggingConfiguration();

            loggingConfiguration.AddRule(FileTraceRule());
            loggingConfiguration.AddRule(FileDebugRule());
            loggingConfiguration.AddRule(FileInfoRule());
            loggingConfiguration.AddRule(FileWarnRule());
            loggingConfiguration.AddRule(FileErrorRule());
            loggingConfiguration.AddRule(FileFatalRule());
            loggingConfiguration.AddRule(ConsoleRule());

            return loggingConfiguration;
        }

        #region file
        private LoggingRule FileTraceRule()
        {

            var today = DateTime.UtcNow.ToString("dd.MM.yyyy:HH");

            var appLogFileTarget = new FileTarget
            {

                FileName = Path.Combine(_environmentsProvider.LogsPath, "trace", $"{today}ч.trace.log")
            };

            var asyncTargetWrapper = new AsyncTargetWrapper(appLogFileTarget)
            {
                TimeToSleepBetweenBatches = 2000
            };

            var loggingRule = new LoggingRule("*", LogLevel.Trace, LogLevel.Trace, asyncTargetWrapper);

            return loggingRule;
        }

        private LoggingRule FileDebugRule()
        {
            var today = DateTime.UtcNow.ToString("dd.MM.yyyy:HH");
            var appLogFileTarget = new FileTarget
            {

                FileName = Path.Combine(_environmentsProvider.LogsPath, "debug", $"{today}ч.debug.log")
            };

            var asyncTargetWrapper = new AsyncTargetWrapper(appLogFileTarget)
            {
                TimeToSleepBetweenBatches = 2000
            };

            var loggingRule = new LoggingRule("*", LogLevel.Debug, LogLevel.Debug, asyncTargetWrapper);

            return loggingRule;
        }

        private LoggingRule FileInfoRule()
        {
            var today = DateTime.UtcNow.ToString("dd.MM.yyyy:HH");
            var appLogFileTarget = new FileTarget
            {

                FileName = Path.Combine(_environmentsProvider.LogsPath, "info", $"{today}ч.info.log")
            };

            var asyncTargetWrapper = new AsyncTargetWrapper(appLogFileTarget)
            {
                TimeToSleepBetweenBatches = 2000
            };

            var loggingRule = new LoggingRule("*", LogLevel.Info, LogLevel.Info, asyncTargetWrapper);

            return loggingRule;
        }

        private LoggingRule FileWarnRule()
        {
            var today = DateTime.UtcNow.ToString("dd.MM.yyyy:HH");
            var appLogFileTarget = new FileTarget
            {

                FileName = Path.Combine(_environmentsProvider.LogsPath, "warning", $"{today}ч.warning.log")
            };

            var asyncTargetWrapper = new AsyncTargetWrapper(appLogFileTarget)
            {
                TimeToSleepBetweenBatches = 2000
            };

            var loggingRule = new LoggingRule("*", LogLevel.Warn, LogLevel.Warn, asyncTargetWrapper);

            return loggingRule;
        }

        private LoggingRule FileErrorRule()
        {
            var today = DateTime.UtcNow.ToString("dd.MM.yyyy:HH");
            var appLogFileTarget = new FileTarget
            {

                FileName = Path.Combine(_environmentsProvider.LogsPath, "error", $"{today}ч.error.log")
            };

            var asyncTargetWrapper = new AsyncTargetWrapper(appLogFileTarget)
            {
                TimeToSleepBetweenBatches = 2000
            };

            var loggingRule = new LoggingRule("*", LogLevel.Error, LogLevel.Error, asyncTargetWrapper);

            return loggingRule;
        }

        private LoggingRule FileFatalRule()
        {
            var today = DateTime.UtcNow.ToString("dd.MM.yyyy:HH");
            var appLogFileTarget = new FileTarget
            {

                FileName = Path.Combine(_environmentsProvider.LogsPath, "fatal", $"{today}ч.fatal.log")
            };

            var asyncTargetWrapper = new AsyncTargetWrapper(appLogFileTarget)
            {
                TimeToSleepBetweenBatches = 2000
            };

            var loggingRule = new LoggingRule("*", LogLevel.Fatal, LogLevel.Fatal, asyncTargetWrapper);

            return loggingRule;
        }

        #endregion file


        private LoggingRule ConsoleRule()
        {
            ColoredConsoleTarget target = new ColoredConsoleTarget();
            var loggingRule = new LoggingRule("*", LogLevel.Info, LogLevel.Fatal, target);

            return loggingRule;
        }

        private LoggingRule DebugRule()
        {
            DebugTarget debugTarget = new DebugTarget();
            var loggingRule = new LoggingRule("*", LogLevel.Debug, LogLevel.Fatal, debugTarget);

            return loggingRule;
        }

        public void Dispose()
        {
            LogManager.Flush();
            LogManager.Shutdown();
        }
    }
}

