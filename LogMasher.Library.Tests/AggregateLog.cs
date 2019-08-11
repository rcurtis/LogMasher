using System;
using System.Collections.Generic;
using System.Text;

namespace LogMasher.Library.Tests
{
    public class AggregateLog
    {
        public class LogNameEntry
        {
            public string LogName { get; set; }
            public LogEntry Entry { get; set; }
        }

        public List<LogNameEntry> NamedEntries { get; } = new List<LogNameEntry>();

        public void MergeLog(Log log)
        {
            foreach (var entry in log.GetEntries)
            {
                var nameEntry = new LogNameEntry();
                nameEntry.LogName = log.Name;
                nameEntry.Entry = entry;
                NamedEntries.Add(nameEntry);
            }
            NamedEntries.Sort((e1, e2) => DateTime.Compare(e1.Entry.DateTime, e2.Entry.DateTime));
        }

        public override string ToString()
        {
           var builder = new StringBuilder();
           foreach (var entry in NamedEntries)
           {
               builder.Append($"{entry.LogName}::{entry.Entry}{Environment.NewLine}");
           }

           return builder.ToString();
        }
    }
}
