using System;
using System.Collections.Generic;

namespace LogMasher.Library
{
    public class Log
    {
        private List<LogEntry> _entries = new List<LogEntry>();

        public int EntriesCount => _entries.Count;
        public string Name { get; private set; }

        public Log(string logName)
        {
            Name = logName;
        }

        public void AddEntry(LogEntry line)
        {
            if(line == null)
                throw new ArgumentNullException(nameof(line));

            _entries.Add(new LogEntry());   
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