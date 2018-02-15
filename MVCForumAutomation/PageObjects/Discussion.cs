using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCForumAutomation.Entities;
using MVCForumAutomation.Infrastructure;
using OpenQA.Selenium;

namespace MVCForumAutomation.PageObjects
{
    public class Discussion : DiscussionIdentifier
    {
        private readonly IWebElement _container;

        public Discussion(IWebDriver webDriver, string title) 
            : base(title)
        {
            _container = webDriver.TryFindElement(By.ClassName("topicshow"));
            Assert.IsNotNull(_container, "Failed to open discussion");
        }

        public string Body
        {
            get { return BodyElement.Text; }
        }

        public IWebElement BodyElement
        {
            get
            {
                return _container.FindElement(By.ClassName("postcontent"));
            }
        }

        public class DiscussionBuilder
        {
            private string _body = "Lorem ipsum... (empty body is not supported)";
            private Category _category;
            private readonly Dictionary<string, string> _parametersDescriptions = new Dictionary<string, string>();

            public DiscussionBuilder(TestDefaults testDefaults)
            {
                _category = testDefaults.ExampleCategory;
            }

            public string UsedTitle { get; private set; }

            public DiscussionBuilder Body(string body)
            {
                _body = body;
                _parametersDescriptions["Body"] = body;
                return this;
            }

            public DiscussionBuilder Category(Category category)
            {
                _category = category;
                _parametersDescriptions["Category"] = category.Name;
                return this;
            }

            public void Fill(CreateDiscussionPage createDiscussionPage)
            {
                UsedTitle = UniqueIdentifier.For("Discussion");
                createDiscussionPage.Title = UsedTitle;
                createDiscussionPage.SelectCategory(_category);
                createDiscussionPage.Body = _body;
            }

            public string Describe()
            {
                return string.Join(", ", _parametersDescriptions.Select(p => $"{p.Key}:'{p.Value}'"));
            }
        }
    }
}