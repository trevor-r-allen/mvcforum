using OpenQA.Selenium;
using TestAutomationEssentials.Common;

namespace MVCForumAutomation
{
    public class LoggedInUser
    {
        protected readonly IWebDriver WebDriver;
        protected readonly TestDefaults TestDefaults;

        public LoggedInUser(IWebDriver webDriver, TestDefaults testDefaults)
        {
            WebDriver = webDriver;
            TestDefaults = testDefaults;
        }

        public Discussion CreateDiscussion(Discussion.DiscussionBuilder builder)
        {
            using (Logger.StartSection($"Creating a discussion with: {builder.Describe()}"))
            {
                var newDiscussionButton = WebDriver.FindElement(By.ClassName("createtopicbutton"));
                newDiscussionButton.Click();

                var createDisucssionPage = new CreateDiscussionPage(WebDriver);
                builder.Fill(createDisucssionPage);
                createDisucssionPage.CreateDiscussion();

                return new Discussion(WebDriver, builder.UsedTitle);
            }
        }

        public void Logout()
        {
            var dropdownMenu = WebDriver.FindElement(By.ClassName("dropdown"));
            dropdownMenu.Click();

            var logoffMenuItem = dropdownMenu.FindElement(By.ClassName("auto-logoff"));
            logoffMenuItem.Click();
        }
    }
}