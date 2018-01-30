using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using TestAutomationEssentials.Common;

namespace MVCForumAutomation
{
    public class DiscussionHeader
    {
        private readonly IWebElement _topicRow;
        private readonly IWebDriver _webDriver;

        public DiscussionHeader(IWebElement topicRow, IWebDriver webDriver)
        {
            _topicRow = topicRow;
            _webDriver = webDriver;
        }

        public string Title
        {
            get
            {
                var titleElement = _topicRow.FindElement(By.TagName("h3"));
                return titleElement.Text;
            }
        }

        public Discussion OpenDiscussion()
        {
            using (Logger.StartSection($"Opening discussion '{Title}'"))
            {
                var link = _topicRow.FindElement(By.CssSelector("h3 a"));
                link.Click();

                var driver = _webDriver;
                return new Discussion(driver);
            }
        }
    }
}