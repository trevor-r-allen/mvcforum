using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;

namespace MVCForumAutomation
{
    internal class RegistrationPage
    {
        private readonly ChromeDriver _webDriver;
        public RegistrationPage(ChromeDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public string Username
        {
            get { throw new NotImplementedException(); }
            set
            {
                FillInputElement("UserName", value);
            }
        }
        public string Password
        {
            get { throw new NotImplementedException(); }
            set
            {
                FillInputElement("Password", value);
            }
        }

        public string ConfirmPassword
        {
            get { throw new NotImplementedException(); }
            set
            {
                FillInputElement("ConfirmPassword", value);
            }
        }

        public string Email
        {
            get { throw new NotImplementedException(); }
            set
            {
                FillInputElement("Email", value);
            }
        }

        public void Register()
        {
            var form = _webDriver.FindElement(By.ClassName("form-register"));
            form.Submit();
        }

        private void FillInputElement(string id, string value)
        {
            var input = _webDriver.FindElement(By.Id(id));
            input.SendKeys(value);
        }

    }
}
