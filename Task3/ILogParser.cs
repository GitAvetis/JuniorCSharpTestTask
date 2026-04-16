namespace Task3
{
    public interface ILogParser
    {
        bool CanParse(string logLine);
        FormatedLog? Parse(string logLine);
    }
}