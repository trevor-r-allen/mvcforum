using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.Extensions;
using TestAutomationEssentials.Common;

namespace MVCForumAutomation
{
    public class MVCForumClient : ITakesScreenshot
    {
        private readonly TestDefaults _testDefaults;
        private readonly IWebDriver _webDriver;

        public MVCForumClient(TestDefaults testDefaults)
        {
            _testDefaults = testDefaults;
            // TODO: select the type of browser and the URL from a configuration file
            var parentDriver = new ChromeDriver();
            var eventFiringDriver = new EventFiringWebDriver(parentDriver);
            VisualLogger.RegisterWebDriverEvents(eventFiringDriver);
            _webDriver = eventFiringDriver;
            _webDriver.Url = "http://localhost:8080";
        }

        ~MVCForumClient()
        {
            _webDriver.Quit();
        }

        public LoggedInUser RegisterNewUserAndLogin()
        {
            var username = Guid.NewGuid().ToString();
            const string password = "123456";
            var email = $"abc@{Guid.NewGuid()}.com";

            var registrationPage = GoToRegistrationPage();
            registrationPage.Username = username;
            registrationPage.Password = password;
            registrationPage.ConfirmPassword = password;
            registrationPage.Email = email;

            registrationPage.Register();

            return new LoggedInUser(_webDriver, _testDefaults);
        }

        private RegistrationPage GoToRegistrationPage()
        {
            var registerLink = _webDriver.FindElement(By.ClassName("auto-register"));
            registerLink.Click();

            return new RegistrationPage(_webDriver);
        }

        private LoggedInPage GoToLoginPage()
        {
            var logonLink = _webDriver.FindElement(By.ClassName("auto-logon"));
            logonLink.Click();

            return new LoggedInPage(_webDriver);
        }

        public LatestDiscussions LatestDiscussions
        {
            get { return new LatestDiscussions(_webDriver); }
        }

        public AdminConsole OpenAdminConsole()
        {
            var adminPassword = GetAdminPassword();
            var adminUser = LoginAsAdmin(adminPassword);
            var adminConsole = adminUser.GoToAdminConsole();

            return adminConsole;
        }

        public CategoriesList Categories
        {
            get { throw new NotImplementedException(); }
        }

        public LoggedInAdmin LoginAsAdmin(string adminPassword)
        {
            return LoginAs(_testDefaults.AdminUsername, adminPassword, () => new LoggedInAdmin(_webDriver, _testDefaults));
        }

        private TLoggedInUser LoginAs<TLoggedInUser>(string username, string password, Func<TLoggedInUser> createLoggedInUser)
            where TLoggedInUser : LoggedInUser
        {
            using (Logger.StartSection($"Logging in as '{username}'/'{password}'"))
            {
                var loginPage = GoToLoginPage();
                loginPage.Username = username;
                loginPage.Password = password;
                loginPage.LogOn();

                var loginErrorMessage = loginPage.GetErrorMessageIfExists();
                Assert.IsNull(loginErrorMessage,
                    $"Login failed for user:{username} and password:{password}. Error message: {loginErrorMessage}");

                return createLoggedInUser();
            }
        }

        public void TakeScreenshot()
        {
            var screenshot = GetScreenshotInternal();
            VisualLogger.AddScreenshot(screenshot);
        }

        Screenshot ITakesScreenshot.GetScreenshot()
        {
            return GetScreenshotInternal();
        }

        private Screenshot GetScreenshotInternal()
        {
            return _webDriver.TakeScreenshot();
        }

        public string GetAdminPassword()
        {
            using (Logger.StartSection("Getting Admin password from 'Read Me' topic"))
            {
                var readMeHeader = this.LatestDiscussions.Bottom;
                var readmeTopic = readMeHeader.OpenDiscussion();
                var body = readmeTopic.BodyElement;
                var password = body.FindElement(By.XPath(".//strong[2]"));
                var adminPassword = password.Text;
                Logger.WriteLine($"Admin password='{adminPassword}'");
                return adminPassword;
            }
        }
    }
}