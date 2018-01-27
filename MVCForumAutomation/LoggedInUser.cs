using System;
using OpenQA.Selenium;

namespace MVCForumAutomation
{
    public class LoggedInUser : 
        ICreator<LoggedInUser.DiscussionBuilder, Discussion, LoggedInUser>
    {
        protected readonly IWebDriver WebDriver;
        private readonly TestDefaults _testDefaults;

        public LoggedInUser(IWebDriver webDriver, TestDefaults testDefaults)
        {
            WebDriver = webDriver;
            _testDefaults = testDefaults;
        }

        public DiscussionBuilder CreateDiscussion
        {
            get
            {
                return new DiscussionBuilder(this, _testDefaults);
            }
        }

        Discussion ICreator<DiscussionBuilder, Discussion, LoggedInUser>.Create(DiscussionBuilder builder)
        {
            var newDiscussionButton = WebDriver.FindElement(By.ClassName("createtopicbutton"));
            newDiscussionButton.Click();

            var createDisucssionPage = new CreateDiscussionPage(WebDriver);
            builder.Fill(createDisucssionPage);
            createDisucssionPage.CreateDiscussion();

            return new Discussion(WebDriver);
        }

        public void Logout()
        {
            var dropdownMenu = WebDriver.FindElement(By.ClassName("dropdown"));
            dropdownMenu.Click();

            var logoffMenuItem = dropdownMenu.FindElement(By.ClassName("auto-logoff"));
            logoffMenuItem.Click();
        }

        public class DiscussionBuilder : AbstractBuilder<DiscussionBuilder, Discussion, LoggedInUser>
        {
            private readonly TestDefaults _testDefaults;
            private string _body;

            public DiscussionBuilder(LoggedInUser owner, TestDefaults testDefaults)
                : base(owner)
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