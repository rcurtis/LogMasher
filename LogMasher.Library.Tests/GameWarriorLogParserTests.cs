using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogMasher.Library.Tests
{
    [TestClass]
    public class GameWarriorLogParserTests
    {
        private static ILogParser GetParser()
        {
            return new GameWarriorLogParser();
        }

        private static string Input =>
            "[2864] [2019-08-10 19:00:07.482] [persist] [info] Change CreditsCents delta: 332 balance: 1120";

        [TestMethod]
        public void Creation()
        {
            var logParser = GetParser();
        }

        [TestMethod]
        public void ParseLine_GameWarriorLogLine()
        {
            var logParser = GetParser();
            var result = logParser.ParseLine(Input);

            Assert.AreEqual(2019 ,result.DateTime.Year);
            Assert.AreEqual(8 ,result.DateTime.Month);
            Assert.AreEqual(10 ,result.DateTime.Day);
            Assert.AreEqual(19 ,result.DateTime.Hour);
            Assert.AreEqual(0 ,result.DateTime.Minute);
            Assert.AreEqual(7 ,result.DateTime.Second);
            Assert.AreEqual(482 ,result.DateTime.Millisecond);
        }

        [TestMethod]
        public void Parse_CanParseSplitLog4NetLines()
        {
            var input = new []{ Input, Input };
            var logParser = GetParser();
            var results = logParser.Parse(input);

            foreach (var result in results)
            {
                Assert.AreEqual(2019, result.DateTime.Year);
                Assert.AreEqual(8, result.DateTime.Month);
                Assert.AreEqual(10, result.DateTime.Day);
                Assert.AreEqual(19, result.DateTime.Hour);
                Assert.AreEqual(0, result.DateTime.Minute);
                Assert.AreEqual(7, result.DateTime.Second);
                Assert.AreEqual(482, result.DateTime.Millisecond);
            }
        }

        [TestMethod]
        public void Parse_FindsLogLevel()
        {
            var logParser = GetParser();
            var result = logParser.ParseLine(Input);
            Assert.AreEqual(LogEntry.LogLevels.INFO, result.LogLevel);
        }

        [TestMethod]
        public void Parse_FindsThreadNumber()
        {
            var logParser = GetParser();
            var result = logParser.ParseLine(Input);
            Assert.AreEqual("2864", result.Thread);
        }

        [TestMethod]
        public void Parse_FindsCategory()
        {
            var logParser = GetParser();
            var result = logParser.ParseLine(Input);
            Assert.AreEqual("persist", result.Category);
        }

        [TestMethod]
        public void Parse_FindsBody()
        {
            var logParser = GetParser();
            var result = logParser.ParseLine(Input);
            Assert.AreEqual("Change CreditsCents delta: 332 balance: 1120", result.Body);
        }

        [TestMethod]
        public void ToString_ReturnsUnifiedOutput()
        {
            var logParser = GetParser();
            var expected = "2019-08-10 19:00:07.482 INFO [2864] persist - Change CreditsCents delta: 332 balance: 1120";
            var result = logParser.ParseLine(Input);
            Assert.AreEqual(expected, result.ToString());
        }
    }
}
