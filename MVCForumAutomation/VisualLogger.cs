using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;
using AventStack.ExtentReports;
using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Reporter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.Extensions;
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
            s_testLog?.Info(new IndentMarkup(_indentationLevel, message));
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
            var filename = GetUniqueImageFilename();
            screenshot.SaveAsFile(filename);
            AddImage(filename);
        }

        private static string GetUniqueImageFilename()
        {
            var directoryInfo = Directory.CreateDirectory("Screenshots");
            var filename = Path.Combine(directoryInfo.Name, $"{Guid.NewGuid()}.jpg");
            return filename;
        }

        private static void AddImage(Image image)
        {
            var filename = GetUniqueImageFilename();
            image.Save(filename);
            AddImage(filename);
        }

        private static void AddImage(string filename)
        {
            s_testLog?.Info(new ImageMarkup(filename));
        }

        private class ImageMarkup : IMarkup
        {
            private readonly string _filename;

            public ImageMarkup(string filename)
            {
                _filename = filename;
            }

            public string GetMarkup()
            {
                return $"<img width='20%' style='margin-left:2cm' src='{_filename}' data-src='{_filename}' data-featherlight='{_filename}'>";
            }
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

        public static void RegisterWebDriverEvents(EventFiringWebDriver eventFiringDriver)
        {
            eventFiringDriver.ElementClicking += EventFiringDriver_ElementClicking;
            eventFiringDriver.ElementValueChanged += EventFiringDriver_ElementValueChanged;
            eventFiringDriver.Navigating += EventFiringDriver_Navigating;
            eventFiringDriver.Navigated += EventFiringDriver_Navigated;
        }

        private static void EventFiringDriver_Navigated(object sender, WebDriverNavigationEventArgs e)
        {
            var webDriver = e.Driver;
            Logger.WriteLine($"Navigated to '{webDriver.Title}' ({webDriver.Url})");
            AddScreenshot(webDriver.TakeScreenshot());
        }

        private static void EventFiringDriver_Navigating(object sender, WebDriverNavigationEventArgs e)
        {
            Logger.WriteLine($"Navigating to {e.Url}");
        }

        private static void EventFiringDriver_ElementValueChanged(object sender, WebElementEventArgs e)
        {
            var name = e.Element.GetAttribute("name");
            var id = e.Element.GetAttribute("id");
            var elementDescription = name ?? id ?? "(unknown)";
            var newValue = e.Element.GetAttribute("value");
            Logger.WriteLine($"Changed value of '{elementDescription}' to '{newValue}'");
            AddScreenshot(e.Driver.TakeScreenshot());
        }

        private static void EventFiringDriver_ElementClicking(object sender, WebElementEventArgs e)
        {
            var text = e.Element.Text;
            Logger.WriteLine($"Clicking on '{text}'");
            var screenshot = e.Driver.TakeScreenshot();
            var image = HighlightElement(screenshot, e.Element);
            AddImage(image);
        }

        private static Image HighlightElement(Screenshot screenshot, IWebElement element)
        {
            var image = Image.FromStream(new MemoryStream(screenshot.AsByteArray));
            using (var g = Graphics.FromImage(image))
            {
                var redPen = Pens.Red;
                g.DrawRectangle(redPen, new Rectangle(element.Location, element.Size));
            }
            return image;
        }

        public static void AddScreenshot(ITakesScreenshot screenshotProvider)
        {
            AddScreenshot(screenshotProvider.GetScreenshot());
        }
    }
}
