using OpenQA.Selenium;
using TestAutomationEssentials.Common;

namespace MVCForumAutomation.PageObjects
{
    public class LoggedInAdmin : LoggedInUser
    {
        public LoggedInAdmin(IWebDriver webDriver, TestDefaults testDefaults)
            :base(webDriver, testDefaults)
        {
        }

        public AdminConsole GoToAdminConsole()
        {
            using (Logger.StartSection("Opening Admin page"))
            {
                var myToolsMenu = WebDriver.FindElement(By.ClassName("mytoolslink"));
                myToolsMenu.Click();

                var adminLink = WebDriver.FindElement(By.CssSelector(".dropdown .auto-admin"));
                adminLink.Click();

                return new AdminConsole(WebDriver, TestDefaults, this);
            }
        }
    }
}