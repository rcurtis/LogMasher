using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LogMasher.Library
{
    public class Log4NetLogParser : ILogParser
    {
        private enum Tokens
        {
            Date = 1,
            Time,
            LogLevel,
            ThreadNumber,
            Category,
            Body
        };
        
        public IEnumerable<LogEntry> Parse(IEnumerable<string> logFile)
        {
            return logFile.Select(ParseLine).ToList();
        }

        public LogEntry ParseLine(string line)
        {
            const string linePattern = @"(\d{4}-\d{2}-\d{2}) (\d{2}:\d{2}:\d{2}\.\d{3}) (\w+) (\[\d\]) (\w+)";
            var regex = new Regex(linePattern, RegexOptions.IgnoreCase);
            var split = regex.Split(line);

            return GetLogEntry(split);
        }

        private static LogEntry GetLogEntry(IReadOnlyList<string> split)
        {
            var entry = new LogEntry();
            entry.DateTime = DateTime.Parse(split[(int) Tokens.Date] + " " + split[(int) Tokens.Time]);
            entry.LogLevel = ParseLogLevel(split);
            entry.ThreadNumber = int.Parse(split[(int) Tokens.ThreadNumber].Substring(1, 1));
            entry.Category = split[(int) Tokens.Category];
            entry.Body = split[(int) Tokens.Body];
            return entry;
        }

        private static LogEntry.LogLevels ParseLogLevel(IReadOnlyList<string> split)
        {
            var logLevelString = split[(int)Tokens.LogLevel];
            return Enum.Parse<LogEntry.LogLevels>(logLevelString);
        }
    }
}