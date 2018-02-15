using System.Collections.Generic;
using OpenQA.Selenium;

namespace MVCForumAutomation
{
    public class CategoryView : DiscussionsList
    {
        public CategoryView(IWebDriver webDriver) 
            : base(webDriver)
        {
        }

        public IReadOnlyCollection<DiscussionHeader> Discussions
        {
            get { return GetAllDiscussionHeaders(); }
        }
    }
}