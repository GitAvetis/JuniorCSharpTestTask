using System.Text.RegularExpressions;
using static Task3.FormatedLog;

namespace Task3
{
    public class Task3Prog
    {
        static void Main(string[] args)
        {
            var formatedLog = new List<string>();
            string root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
            string path = Path.Combine(root, "problems.txt");
            string format1 = "10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'";
            string format2 = "2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'";
            ParseLog(format1, formatedLog, path);
            ParseLog(format2, formatedLog, path);
            foreach (var message in formatedLog)
            {
                Console.WriteLine(message);
            }
        }

        public enum Format
        {
            FORMAT1,
            FORMAT2,
            UNKNOWN
        }

        private static readonly Regex Format1DateRegex =
            new Regex(@"^\d{2}\.\d{2}\.\d{4} \d{2}:\d{2}:\d{2}\.\d{3}", RegexOptions.Compiled);
        private static readonly Regex Format2DateRegex =
            new Regex(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{4,}", RegexOptions.Compiled);

        public static Format LogFormat(string log)
        {
            if (string.IsNullOrWhiteSpace(log))
                return Format.UNKNOWN;

            if (Format2DateRegex.IsMatch(log))
                return Format.FORMAT2;
            if (Format1DateRegex.IsMatch(log))
                return Format.FORMAT1;

            return Format.UNKNOWN;
        }

        public static FormatedLog? ParseLog(string log, List<string> formatedLog, string path)
        {
            Format format = LogFormat(log);
            char? delimiter = Delimiter(format);

            if (delimiter == null || format == Format.UNKNOWN)
            {
                ErrorLog(log, path);
                return null;
            }

            string[]? tokens = InputTokens(log, delimiter, format);
            if (tokens == null)
            {
                ErrorLog(log, path);
                return null;
            }
            else
            {
                FormatedLog? formated = format == Format.FORMAT1 ? ParseFormat1(tokens) : ParseFormat2(tokens);

                if (formated != null)
                {
                    string method = formated.Method == null ? "DEFAULT" : formated.Method;
                    formatedLog.Add($"{formated.Date}\t{formated.Time}\t{formated.Level}\t{method}\t{formated.Message}");
                    return formated;
                }
                else
                    ErrorLog(log, path);
                return null;
            }
        }

        private static void ErrorLog(string log, string path)
        {
            File.AppendAllText(path, log + Environment.NewLine);
        }

        public static string[]? InputTokens(string inputLog, char? delimiter, Format format)
        {
            if (format == Format.UNKNOWN || delimiter == null)
                return null;

            string[] tokens = inputLog.Split(delimiter.Value);
            if ((format == Format.FORMAT1 && tokens.Length < 4) || (format == Format.FORMAT2 && tokens.Length < 5))
            {
                return null;
            }
            else
            {
                return tokens;
            }
        }

        private static char? Delimiter(Format format)
        {
            switch (format)
            {
                case Format.FORMAT1:
                    return ' ';
                case Format.FORMAT2:
                    return '|';
                default:
                    return null;
            }
        }

        public static LogLevel TypeOfLog(string logLevel)
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

        public static FormatedLog? ParseFormat1(string[] tokens)
        {
            string date = ChangeDateFormat(tokens[0]);
            string time = tokens[1];
            LogLevel level = TypeOfLog(tokens[2].Trim());
            if (level == LogLevel.PARSE_ERROR)
            {
                return null;
            }
            string? method = null;
            string message = string.Join(" ", tokens.Skip(3));
            return new FormatedLog(date, time, level, method, message);
        }

        private static string ChangeDateFormat(string date)
        {
            if (date.Contains('-'))
            {
                return date;
            }
            var dateParts = date.Split('.');
            if (dateParts.Length == 3)
            {
                return $"{dateParts[2]}-{dateParts[1]}-{dateParts[0]}";
            }
            return date;
        }

        public static FormatedLog? ParseFormat2(string[] tokens)
        {
            var dateTimePart = tokens[0].Split(' ');
            string date = dateTimePart[0].Trim();
            string time = dateTimePart[1].Trim();
            LogLevel level = TypeOfLog(tokens[1].Trim());
            if (level == LogLevel.PARSE_ERROR)
            {
                return null;
            }
            string? method = tokens[3].Trim();
            string message = string.Join(" ", tokens.Skip(4));

            return new FormatedLog(date, time, level, method, message.Trim());
        }
    }
}
