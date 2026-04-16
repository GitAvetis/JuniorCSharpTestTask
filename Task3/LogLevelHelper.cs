using static Task3.FormatedLog;
namespace Task3
{
    public static class LogLevelHelper
    {
        public static LogLevel Parse(string logLevel)
        {
            switch (logLevel)
            {
                case "INFO":
                case "INFORMATION":
                    return LogLevel.INFO;
                case "WARN":
                case "WARNING":
                    return LogLevel.WARN;
                case "ERROR":
                    return LogLevel.ERROR;
                case "DEBUG":
                    return LogLevel.DEBUG;
                default:
                    return LogLevel.PARSE_ERROR;
            }
        }
    }
}