using System.Linq;
using OpenQA.Selenium;

namespace MVCForumAutomation.PageObjects
{
    public class LatestDiscussions : DiscussionsList
    {
        public LatestDiscussions(IWebDriver webDriver)
            : base(webDriver)
        {
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

        private void Activate()
        {
            var latestMenuItem = WebDriver.FindElement(By.CssSelector(".sub-nav-container .auto-latest"));
            latestMenuItem.Click();
        }
    }
}