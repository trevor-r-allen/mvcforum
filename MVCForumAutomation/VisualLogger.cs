using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using AventStack.ExtentReports;
using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Reporter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using TestAutomationEssentials.Common;

namespace MVCForumAutomation
{
    internal class VisualLogger : ICustomLogger
    {
        private static ExtentReports s_reports;
        private static ExtentTest s_testLog;

        private int _indentationLevel;
        private readonly Stack<string> _sections = new Stack<string>();

        public static void Initialize(string reportPath)
        {
            s_reports = new ExtentReports();
            var htmlReporter = new ExtentHtmlReporter(reportPath);
            s_reports.AttachReporter(htmlReporter);
            Logger.Initialize(new VisualLogger());
        }

        public static void StartTest(string testName)
        {
            s_testLog = s_reports.CreateTest(testName);
        }

        void ICustomLogger.WriteLine(DateTime timestamp, string message)
        {
            WriteLine(message);
        }

        private void WriteLine(string message)
        {
            s_testLog.Info(new IndentMarkup(_indentationLevel, message));
        }

        void ICustomLogger.StartSection(DateTime timestamp, string message)
        {
            ((ICustomLogger) this).WriteLine(timestamp, message);
            _indentationLevel++;
            _sections.Push(message);
        }

        void ICustomLogger.EndSection(DateTime timestamp)
        {
            _indentationLevel--;
            WriteLine($"[Done: {_sections.Pop()}]");
        }

        private class IndentMarkup : IMarkup
        {
            private readonly int _indentation;
            private readonly string _message;

            public IndentMarkup(int indentation, string message)
            {
                _indentation = indentation;
                _message = message;
            }

            public string GetMarkup()
            {
                var encodedMessage = HttpUtility.HtmlEncode(_message);
                return $"<span style='padding-left:{_indentation}cm'>{encodedMessage}</span>";
            }
        }

        public static void AddScreenshot(Screenshot screenshot)
        {
            var directoryInfo = Directory.CreateDirectory("Screenshots");
            var filename = Path.Combine(directoryInfo.Name, $"{Guid.NewGuid()}.jpg");
            screenshot.SaveAsFile(filename);
            s_testLog.AddScreenCaptureFromPath(filename);
        }

        public static void ReportOutcome(TestContext testContext, ITakesScreenshot screenshotProvider)
        {
            if (testContext.CurrentTestOutcome == UnitTestOutcome.Passed)
                s_testLog.Pass("Test Passed");
            else
            {
                s_testLog.Fail("Test outcome=" + testContext.CurrentTestOutcome);
                var screenshot = screenshotProvider.GetScreenshot();
                AddScreenshot(screenshot);
            }

            s_reports.Flush();
        }
    }
}
