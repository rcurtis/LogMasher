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
            ERROR,
            FATAL
        }

        public DateTime DateTime { get; set; }
        public LogLevels LogLevel { get; set; }
        public int ThreadNumber { get; set; }
        public string Category { get; set; }
        public string Body { get; set; }

        public override string ToString()
        {
            return $"{DateTime.ToString("yyyy-MM-dd HH:mm:ss.FFFF")} {LogLevel.ToString().ToUpper()} [{ThreadNumber}] {Category} - {Body}";
        }

        protected bool Equals(LogEntry other)
        {
            return DateTime.Equals(other.DateTime) 
                   && LogLevel == other.LogLevel 
                   && ThreadNumber == other.ThreadNumber 
                   && string.Equals(Category, other.Category) 
                   && string.Equals(Body, other.Body);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LogEntry) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DateTime.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) LogLevel;
                hashCode = (hashCode * 397) ^ ThreadNumber;
                hashCode = (hashCode * 397) ^ (Category != null ? Category.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Body != null ? Body.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}