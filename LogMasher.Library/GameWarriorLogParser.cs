using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LogMasher.Library
{
    public class GameWarriorLogParser : ILogParser
    {
        private enum Tokens
        {
            ThreadNumber,
            DateTime,
            Category,
            LogLevel,
            Body
        };
        
        public IEnumerable<LogEntry> Parse(IEnumerable<string> logFile)
        {
            return logFile.Select(ParseLine).ToList();
        }

        public LogEntry ParseLine(string line)
        {
            //const string linePattern = @"(\d{4}-\d{2}-\d{2}) (\d{2}:\d{2}:\d{2}\.\d{3}) (\w+) (\[\d\]) (\w+)";
            const string linePattern = @"(\[\d+\]) (\[\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3}\]) (\[\w+\]) (\[\w+\]) ";
            var regex = new Regex(linePattern, RegexOptions.IgnoreCase);
            var split = regex.Split(line).ToList();

            split = TrimBracketsOut(split);

            return GetLogEntry(split);
        }

        private List<string> TrimBracketsOut(IEnumerable<string> split)
        {
            var tokens = new List<string>();
            foreach (var token in split)
            {
                if(string.IsNullOrEmpty(token))
                    continue;
                tokens.Add(token.Trim('[', ']'));
            }

            return tokens;
        }

        private static LogEntry GetLogEntry(IReadOnlyList<string> split)
        {
            var entry = new LogEntry();
            entry.DateTime = DateTime.Parse(split[(int)Tokens.DateTime]);
            entry.LogLevel = ParseLogLevel(split);
            entry.ThreadNumber = int.Parse(split[(int) Tokens.ThreadNumber]);
            entry.Category = split[(int) Tokens.Category];
            entry.Body = split[(int) Tokens.Body];
            return entry;
        }

        private static LogEntry.LogLevels ParseLogLevel(IReadOnlyList<string> split)
        {
            var logLevelString = split[(int)Tokens.LogLevel];
            return Enum.Parse<LogEntry.LogLevels>(logLevelString.ToUpper());
        }
    }
}