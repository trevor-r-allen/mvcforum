using System;
using OpenQA.Selenium;

namespace MVCForumAutomation
{
    internal class CreateCategoryPage
    {
        private readonly IWebDriver _webDriver;

        public CreateCategoryPage(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public string CategoryName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void Create()
        {
            throw new NotImplementedException();
        }
    }
}