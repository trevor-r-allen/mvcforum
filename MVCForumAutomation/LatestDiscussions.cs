using System.Collections.Generic;
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
            Activate();
        }

        public DiscussionHeader Top
        {
            get { return GetAllDiscussionHeaders().First(); }
        }

        public DiscussionHeader Bottom
        {
            get { return GetAllDiscussionHeaders().Last(); }
        }

        private IReadOnlyCollection<DiscussionHeader> GetAllDiscussionHeaders()
        {
            var topicRows = _webDriver.FindElements(By.ClassName("topicrow"));
            return topicRows.Select(row => new DiscussionHeader(row, _webDriver)).ToList();
        }

        private void Activate()
        {
            var latestMenuItem = _webDriver.FindElement(By.CssSelector(".sub-nav-container .auto-latest"));
            latestMenuItem.Click();
        }
    }
}