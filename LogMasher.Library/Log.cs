using System;
using System.Collections.Generic;

namespace LogMasher.Library
{
    public class Log
    {
        private List<LogEntry> _entries = new List<LogEntry>();

        public int EntriesCount => _entries.Count;
        public string Name { get; private set; }
        public IEnumerable<LogEntry> GetEntries => _entries;

        public Log(string logName)
        {
            Name = logName;
        }

        public Log(string logName, ILogParser parser, IInput input) : this(logName)
        {
            var data = input.GetLines();
            var parsed = parser.Parse(data);
            AddEntries(parsed);
        }

        public void AddEntry(LogEntry line)
        {
            if(line == null)
                throw new ArgumentNullException(nameof(line));

            _entries.Add(line);   
        }

        public void AddEntries(IEnumerable<LogEntry> lines)
        {
            foreach (var line in lines)
            {
                AddEntry(line);
            }
        }
    }
}