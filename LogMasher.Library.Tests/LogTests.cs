using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogMasher.Library.Tests
{
    [TestClass]
    public class LogTests
    {
        public static Log GetLog()
        {
            return new Log("GameWarrior");
        }

        [TestMethod]
        public void Log_HasName()
        {
            var log = GetLog();
            Assert.AreEqual("GameWarrior", log.Name);
        }

        [TestMethod]
        public void Log_HasLogEntries()
        {
            var log = GetLog();
            log.AddEntry(new LogEntry());
            Assert.AreEqual(1, log.EntriesCount);
        }

        [TestMethod]
        public void AddLog_NullArg_Throws()
        {
            var log = GetLog();
            Assert.ThrowsException<ArgumentNullException>(() => log.AddEntry(null));
        }

        [TestMethod]
        public void AddEntries_TakesMultipleEntries()
        {
            var log = GetLog();
            log.AddEntries(new List<LogEntry> { new LogEntry(), new LogEntry() });
        }

        [TestMethod]
        public void AddEntries_AnyNullArgs_Throws()
        {
            var log = GetLog();
            Assert.ThrowsException<ArgumentNullException>(() => 
                log.AddEntries(new List<LogEntry> { null, null })
                );
        }
    }
}
