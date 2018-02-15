using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace MVCForumAutomation.PageObjects
{
    public class DiscussionsList
    {
        protected readonly IWebDriver WebDriver;

        protected DiscussionsList(IWebDriver webDriver)
        {
            WebDriver = webDriver;
        }

        protected IReadOnlyCollection<DiscussionHeader> GetAllDiscussionHeaders()
        {
            var topicRows = WebDriver.FindElements(By.ClassName("topicrow"));
            return topicRows.Select(row => new DiscussionHeader(row, WebDriver)).ToList();
        }
    }
}