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
            ProcessLine(format1, formatedLog, path);
            ProcessLine(format2, formatedLog, path);
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
        private static void ProcessLine(string line, List<string> output, string errorPath)
        {
            var parsed = ParseLog(line, errorPath);
            if (parsed != null)
            {
                string method = parsed.Method ?? "DEFAULT";
                output.Add($"{parsed.Date}\t{parsed.Time}\t{parsed.Level}\t{method}\t{parsed.Message}");
            }
        }

        public static class LogParserFactory
        {
            private static readonly ILogParser[] Parsers =
                {
                    new Format1Parser(),
                    new Format2Parser()
                };

            public static ILogParser? GetParser(string logLine)
            {
                return Parsers.FirstOrDefault(p => p.CanParse(logLine));
            }
        }

        public static FormatedLog? ParseLog(string log, string pathForErrors)
        {
            var parser = LogParserFactory.GetParser(log);
            if (parser == null)
            {
                ErrorLog(log, pathForErrors);
                return null;
            }

            try
            {
                return parser.Parse(log);
            }
            catch (Exception)
            {
                ErrorLog(log, pathForErrors);
                return null;
            }
        }

        private static void ErrorLog(string log, string path)
        {
            File.AppendAllText(path, log + Environment.NewLine);
        }
    }
}
