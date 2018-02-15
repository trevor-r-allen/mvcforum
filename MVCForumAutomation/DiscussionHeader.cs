using OpenQA.Selenium;
using TestAutomationEssentials.Common;

namespace MVCForumAutomation
{
    public class DiscussionHeader : DiscussionIdentifier
    {
        private readonly IWebElement _topicRow;
        private readonly IWebDriver _webDriver;

        public DiscussionHeader(IWebElement topicRow, IWebDriver webDriver)
            : base(GetTitle(topicRow))
        {
            _topicRow = topicRow;
            _webDriver = webDriver;
        }

        private static string GetTitle(IWebElement topicRow)
        {
                var titleElement = topicRow.FindElement(By.TagName("h3"));
                return titleElement.Text;
        }

        public Discussion OpenDiscussion()
        {
            using (Logger.StartSection($"Opening discussion '{Title}'"))
            {
                var link = _topicRow.FindElement(By.CssSelector("h3 a"));
                link.Click();

                var driver = _webDriver;
                return new Discussion(driver, Title);
            }
        }
    }
}