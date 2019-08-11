using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogMasher.Library.Tests
{
    [TestClass]
    public class AggregateLogTests
    {
        #region Test Data
        private List<LogEntry> _entries = new List<LogEntry>
        {
            new LogEntry
            {
                Body = "Test Body 1",
                Category = "PreProcessor",
                DateTime = DateTime.Now,
                LogLevel = LogEntry.LogLevels.DEBUG,
                ThreadNumber = 1,
            },
            new LogEntry
            {
                Body = "Test Body 2",
                Category = "Jackpot",
                DateTime = DateTime.Now.AddMilliseconds(2),
                LogLevel = LogEntry.LogLevels.INFO,
                ThreadNumber = 1
            },
            new LogEntry
            {
                Body = "Test Body 3",
                Category = "Update",
                DateTime = DateTime.Now.AddHours(1),
                LogLevel = LogEntry.LogLevels.WARN,
                ThreadNumber = 1
            },
            new LogEntry
            {
                Body = "Test Body 4",
                Category = "87fhjasdf",
                DateTime = DateTime.Now.AddDays(5),
                LogLevel = LogEntry.LogLevels.FATAL,
                ThreadNumber = 1
            },
        };
        #endregion

        [TestMethod]
        public void Creation()
        {
            var agg = new AggregateLog();
        }

        [TestMethod]
        public void MergeLog_TakesAllEntries()
        {
            var log = new Log("GameWarrior");
            log.AddEntries(new []{_entries[0], _entries[1]});

            var agg = new AggregateLog();
            agg.MergeLog(log);
            Assert.AreEqual(2, agg.NamedEntries.Count);
        }

        [TestMethod]
        public void ToString_PrependsOriginLogName()
        {
            var log = new Log("GameWarrior");
            log.AddEntries(new[] { _entries[0], _entries[1] });

            var agg = new AggregateLog();
            agg.MergeLog(log);

            var result = agg.ToString();
            StringAssert.StartsWith(result, "GameWarrior");
        }

        [TestMethod]
        public void MergeLog_FromDifferentLoggers()
        {
            var log1 = new Log("GameWarrior");
            log1.AddEntries(new[] { _entries[0], _entries[1] });

            var log2 = new Log("MGP");
            log2.AddEntries(new[] { _entries[2], _entries[3] });

            var agg = new AggregateLog();
            agg.MergeLog(log1);
            agg.MergeLog(log2);
            Assert.AreEqual(4, agg.NamedEntries.Count);
            var str = agg.ToString();
            StringAssert.Contains(str, "GameWarrior");
            StringAssert.Contains(str, "MGP");
        }

        [TestMethod]
        public void MergeLog_OrdersByDateTime()
        {
            var log1 = new Log("GameWarrior");
            log1.AddEntries(new[] { _entries[3], _entries[0], _entries[2], _entries[1] });

            var agg = new AggregateLog();
            agg.MergeLog(log1);
            var result = agg.NamedEntries;
            Assert.AreEqual(result[0].Entry, _entries[0]);
            Assert.AreEqual(result[1].Entry, _entries[1]);
            Assert.AreEqual(result[2].Entry, _entries[2]);
            Assert.AreEqual(result[3].Entry, _entries[3]);
        }
    }
}
