using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;

namespace MVCForumAutomation
{
    public partial class SanityTests
    {
        public class MVCForumClient
        {
            private readonly ChromeDriver _webDriver;

            public MVCForumClient()
            {
                _webDriver = new ChromeDriver("C:\\Program Files\\Google\\Chrome\\Application");
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
                const string email = "abc@def.com";

                var registrationPage = GoToRegistrationPage();
                registrationPage.Username = username;
                registrationPage.Password = password;
                registrationPage.ConfirmPassword = password;
                registrationPage.Email = email;

                registrationPage.Register();

                return new LoggedInUser();

            }

            private RegistrationPage GoToRegistrationPage()
            {
                var registerLink = _webDriver.FindElement(By.ClassName("auto-register"));
                registerLink.Click();
                return new RegistrationPage(_webDriver);
            }

            public LatestDiscussions LatestDiscussions
            {
                get { throw new NotImplementedException(); }
            }
        }


    }

}
