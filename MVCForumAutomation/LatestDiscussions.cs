using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;

namespace MVCForumAutomation
{
    public class LatestDiscussions
    {
        private readonly IWebDriver _webDriver;

        public LatestDiscussions(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public DiscussionHeader Top
        {
            get
            {
                var topicRows = GetAllTopicRows();
                return new DiscussionHeader(topicRows.First());
            }
        }

        public DiscussionHeader Bottom
        {
            get
            {
                var topicRows = GetAllTopicRows();
                return new DiscussionHeader(topicRows.Last());
            }
        }

        private ReadOnlyCollection<IWebElement> GetAllTopicRows()
        {
            Activate();
            var topicRows = _webDriver.FindElements(By.ClassName("topicrow"));
            return topicRows;
        }

        private void Activate()
        {
            var latestMenuItem = _webDriver.FindElement(By.CssSelector(".sub-nav-container .auto-latest"));
            latestMenuItem.Click();
        }
    }
}