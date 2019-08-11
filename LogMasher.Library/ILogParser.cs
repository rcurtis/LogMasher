using System.Collections.Generic;

namespace LogMasher.Library
{
    public interface ILogParser
    {
        IEnumerable<LogEntry> Parse(IEnumerable<string> logFile);
        LogEntry ParseLine(string line);
    }
}