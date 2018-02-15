using MVCForumAutomation.Infrastructure;
using OpenQA.Selenium;

namespace MVCForumAutomation.PageObjects
{
    internal class LoginPage : FormPage
    {
        public LoginPage(IWebDriver webDriver) 
            : base(webDriver)
        {
        }

        public string Username
        {
            get { throw new System.NotImplementedException(); }
            set
            {
                FillInputElement("UserName", value);
            }
        }

        public string Password
        {
            get { throw new System.NotImplementedException(); }
            set
            {
                FillInputElement("Password", value);
            }
        }

        /// <returns>
        /// The error message displayed on the Login Page, or null if no error is displayed
        /// </returns>
        public string GetErrorMessageIfExists()
        {
            var errorMessageElement = WebDriver.TryFindElement(By.ClassName("validation-summary-errors"));
            return errorMessageElement?.Text;
        }

        public void LogOn()
        {
            var submitButton = WebDriver.FindElement(By.CssSelector(".form-login button"));
            submitButton.Click();
        }
    }
}