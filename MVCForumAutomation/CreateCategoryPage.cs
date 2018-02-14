using System;
using OpenQA.Selenium;

namespace MVCForumAutomation
{
    internal class CreateCategoryPage : FormPage
    {
        public CreateCategoryPage(IWebDriver webDriver)
            : base(webDriver)
        {
        }

        public string CategoryName
        {
            get { throw new NotImplementedException(); }
            set { FillInputElement("Name", value); }
        }

        public void Create()
        {
            var submit = WebDriver.FindElement(By.CssSelector("form input[type=submit]"));
            submit.Click();
        }
    }
}