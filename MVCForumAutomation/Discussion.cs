using System;
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
            private readonly TestDefaults _testDefaults;
            private string _body;

            public DiscussionBuilder(TestDefaults testDefaults)
            {
                _testDefaults = testDefaults;
            }

            public DiscussionBuilder Body(string body)
            {
                _body = body;
                return this;
            }

            public void Fill(CreateDiscussionPage createDiscussionPage)
            {
                createDiscussionPage.Title = Guid.NewGuid().ToString();
                createDiscussionPage.SelectCategory(_testDefaults.ExampleCategory);
                createDiscussionPage.Body = _body;

                createDiscussionPage.CreateDiscussion();
            }
        }
    }
}