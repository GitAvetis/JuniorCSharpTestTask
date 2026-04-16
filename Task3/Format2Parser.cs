using System.Text.RegularExpressions;
using static Task3.FormatedLog;

namespace Task3
{
    public class Format2Parser : ILogParser
    {
        private const char Delimiter = '|';
        private const int ExpectedTokensCount = 5;

        private static readonly Regex DateRegex =
            new(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{4,}", RegexOptions.Compiled);
        public bool CanParse(string logLine) => DateRegex.IsMatch(logLine);

        public FormatedLog? Parse(string logLine)
        {
            string[] tokens = logLine.Split(Delimiter);
            if (tokens.Length < ExpectedTokensCount)
                return null;

            var dateTimeParts = tokens[0].Trim().Split(' ');
            if (dateTimeParts.Length != 2)
                return null;

            string date = dateTimeParts[0];
            string time = dateTimeParts[1];

            LogLevel level = LogLevelHelper.Parse(tokens[1].Trim());
            if (level == LogLevel.PARSE_ERROR)
            {
                return null;
            }

            string method = tokens[3].Trim();
            string message = tokens.Length > 5
                ? string.Join(" ", tokens.Skip(4)).Trim()
                : tokens[4].Trim();

            return new FormatedLog(date, time, level, method, message);
        }
    }
}