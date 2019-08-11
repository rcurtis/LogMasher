using System;

namespace LogMasher.Library
{
    public class LogEntry
    {
        public enum LogLevels
        {
            DEBUG,
            INFO,
            WARN,
            FATAL
        }

        public DateTime DateTime { get; set; }
        public LogLevels LogLevel { get; set; }
        public int ThreadNumber { get; set; }
        public string Category { get; set; }
        public string Body { get; set; }
    }
}