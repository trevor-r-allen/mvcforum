using OpenQA.Selenium;

namespace MVCForumAutomation
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

        public void LogOn()
        {
            var submitButton = WebDriver.FindElement(By.CssSelector(".form-login button"));
            submitButton.Click();
        }
    }
}