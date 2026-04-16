using System.Text.RegularExpressions;
using static Task3.FormatedLog;

namespace Task3
{
    public class Format1Parser : ILogParser
    {
        private static readonly Regex DateRegex =
            new(@"^\d{2}\.\d{2}\.\d{4} \d{2}:\d{2}:\d{2}\.\d{3}", RegexOptions.Compiled);

        public bool CanParse(string logLine) => DateRegex.IsMatch(logLine);
        private const char Delimiter = ' ';
        private const int ExpectedTokensCount = 4;

        public FormatedLog? Parse(string logLine)
        {
            string[] tokens = logLine.Split(Delimiter);
            if (tokens.Length < ExpectedTokensCount)
                return null;
            string date = ChangeDateFormat(tokens[0]);
            string time = tokens[1];
            LogLevel level = LogLevelHelper.Parse(tokens[2].Trim());
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
    }
}