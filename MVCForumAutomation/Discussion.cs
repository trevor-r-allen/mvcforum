using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace MVCForumAutomation
{
    public class Discussion
    {
        private readonly IWebElement _container;

        public Discussion(IWebDriver webDriver)
        {
            _container = webDriver.TryFindElement(By.ClassName("topicshow"));
            Assert.IsNotNull(_container, "Failed to open discussion");
        }

        public string Title
        {
            get
            {
                var titleElement = _container.FindElement(By.CssSelector(".topicheading h1"));
                return titleElement.Text;
            }
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
            private string _body = string.Empty;
            private Category _category;
            private readonly Dictionary<string, string> _parametersDescriptions = new Dictionary<string, string>();

            public DiscussionBuilder(TestDefaults testDefaults)
            {
                _category = testDefaults.ExampleCategory;
            }

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
                createDiscussionPage.Title = Guid.NewGuid().ToString();
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