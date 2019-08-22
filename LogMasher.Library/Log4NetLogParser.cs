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
            Thread,
            Category,
            Body
        };
        
        public IEnumerable<LogEntry> Parse(IEnumerable<string> logFile)
        {
            return logFile.Select(ParseLine).ToList();
        }

        public LogEntry ParseLine(string line)
        {
                var split = Tokenize(line);
                var tokens = split.ToList().GetRange(0, 4);
                ManuallyParse(split[4], tokens);

                return GetLogEntry(tokens);
        }

        private static string[] Tokenize(string line)
        {
            const string pattern = @"(\d{4}-\d{2}-\d{2}) (\d{2}:\d{2}:\d{2}\.\d{3}) (\w+)";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            var split = regex.Split(line);
            return split;
        }

        private static void ManuallyParse(string unparsed, List<string> tokens)
        {
            ParseCategory(unparsed, tokens);
            unparsed = unparsed.Substring(unparsed.IndexOf(']') + 2);
            var nextSpace = ParseThread(unparsed, tokens);
            ParseBody(tokens, unparsed, nextSpace);
        }

        private static void ParseBody(List<string> tokens, string unparsed, int nextSpace)
        {
            tokens.Add(unparsed.Substring(nextSpace));
        }

        private static int ParseThread(string unparsed, List<string> tokens)
        {
            var nextSpace = unparsed.IndexOf(' ');
            var thread = unparsed.Substring(0, nextSpace);
            tokens.Add(thread);
            return nextSpace;
        }

        private static void ParseCategory(string unparsed, List<string> tokens)
        {
            var category = unparsed.Substring(unparsed.IndexOf('[') + 1, unparsed.IndexOf(']') - 2);
            tokens.Add(category);
        }

        private static LogEntry GetLogEntry(IReadOnlyList<string> split)
        {
            var entry = new LogEntry();
            entry.DateTime = DateTime.Parse(split[(int) Tokens.Date] + " " + split[(int) Tokens.Time]);
            entry.LogLevel = ParseLogLevel(split);
            entry.Thread = split[(int) Tokens.Thread];
            entry.Category = split[(int) Tokens.Category];
            entry.Body = TrimLeadingSpacingAndPunctuation(split[(int) Tokens.Body]);
            return entry;
        }

        private static string TrimLeadingSpacingAndPunctuation(string body)
        {
            var firstAlphaCharIndex = 0;
            foreach (var ch in body)
            {
                if (char.IsLetter(ch))
                {
                    firstAlphaCharIndex = body.IndexOf(ch);
                    break;
                }
            }
            return body.Substring(firstAlphaCharIndex, body.Length - firstAlphaCharIndex);
        }

        private static LogEntry.LogLevels ParseLogLevel(IReadOnlyList<string> split)
        {
            var logLevelString = split[(int)Tokens.LogLevel];
            return Enum.Parse<LogEntry.LogLevels>(logLevelString);
        }
    }
}