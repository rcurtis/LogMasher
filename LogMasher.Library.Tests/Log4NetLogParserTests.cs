﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogMasher.Library.Tests
{
    [TestClass]
    public class Log4NetLogParserTests
    {
        private static ILogParser GetParser()
        {
            return new Log4NetLogParser();
        }

        private static string Input => "2019-08-11 00:05:05.519 DEBUG [1] PreGameShareResponse - Response from";
        private static string NamedThreadInput => "2019-08-11 03:27:31.164 INFO [Platform] Machine.Idle - New bingo card requested: 12...";
        private static string PeriodInLoggerName => "2019-08-11 03:27:31.164 INFO [Platform] Machine.Idle - New bingo card requested: 12...";

        [TestMethod]
        public void Creation()
        {
            var logParser = GetParser();
        }

        [TestMethod]
        public void ParseLine_ReadsLog4NetLine()
        {
            var logParser = GetParser();
            var result = logParser.ParseLine(Input);

            Assert.AreEqual(2019 ,result.DateTime.Year);
            Assert.AreEqual(8 ,result.DateTime.Month);
            Assert.AreEqual(11 ,result.DateTime.Day);
            Assert.AreEqual(0 ,result.DateTime.Hour);
            Assert.AreEqual(5 ,result.DateTime.Minute);
            Assert.AreEqual(5 ,result.DateTime.Second);
            Assert.AreEqual(519 ,result.DateTime.Millisecond);
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
                Assert.AreEqual(11, result.DateTime.Day);
                Assert.AreEqual(0, result.DateTime.Hour);
                Assert.AreEqual(5, result.DateTime.Minute);
                Assert.AreEqual(5, result.DateTime.Second);
                Assert.AreEqual(519, result.DateTime.Millisecond);
            }
        }

        [TestMethod]
        public void Parse_WhereThreadHasAName_NotNumber()
        {
            var logParser = GetParser();
            var result = logParser.ParseLine(NamedThreadInput);
            //Assert.AreEqual(LogEntry.LogLevels.DEBUG, result.LogLevel);
        }

        [TestMethod]
        public void Parse_FindsLogLevel()
        {
            var logParser = GetParser();
            var result = logParser.ParseLine(Input);
            Assert.AreEqual(LogEntry.LogLevels.DEBUG, result.LogLevel);
        }

        [TestMethod]
        public void Parse_FindsThreadNumber()
        {
            var logParser = GetParser();
            var result = logParser.ParseLine(Input);
            Assert.AreEqual("1", result.Thread);
        }

        [TestMethod]
        public void Parse_FindsCategory()
        {
            var logParser = GetParser();
            var result = logParser.ParseLine(Input);
            Assert.AreEqual("PreGameShareResponse", result.Category);
        }

        [TestMethod]
        public void Parse_FindsBody()
        {
            var logParser = GetParser();
            var result = logParser.ParseLine(Input);
            Assert.AreEqual("Response from", result.Body);
        }

        [TestMethod]
        public void ToString_ReturnsUnifiedOutput()
        {
            var logParser = GetParser();
            var expected = "2019-08-11 00:05:05.519 DEBUG [1] PreGameShareResponse - Response from";
            var result = logParser.ParseLine(Input);
            Assert.AreEqual(expected, result.ToString());
        }
    }
}
