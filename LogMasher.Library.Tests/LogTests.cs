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

        [TestMethod]
        public void Log_ReadFromMemoryInput()
        {
            var input = new MemoryInput(new List<string>
            {
                "[2864] [2019-08-10 19:00:07.482] [persist] [info] Change CreditsCents delta: 332 balance: 1120",
                "[2864] [2019-08-10 21:00:07.482] [persist] [info] Change CreditsCents delta: 332 balance: 1120"
            });
            var log = new Log("GameWarrior", new GameWarriorLogParser(), input);
            Assert.AreEqual(2, log.EntriesCount);
        }

        [TestMethod]
        public void Log_ReadGWLogsFromFileInput()
        {
            var input = new FileInput("GW_TestLog1.log");
            var log = new Log("GameWarrior", new GameWarriorLogParser(), input);
            Assert.AreEqual(5, log.EntriesCount);
        }

        [TestMethod]
        public void Log_ReadMGPLogsFromFileInput()
        {
            var input = new FileInput("MGP_TestLog1.log");
            var log = new Log("GameWarrior", new Log4NetLogParser(), input);
            Assert.AreEqual(13, log.EntriesCount);
        }
    }
}
