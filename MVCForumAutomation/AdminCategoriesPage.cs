using OpenQA.Selenium;

namespace MVCForumAutomation
{
    internal class AdminCategoriesPage
    {
        private readonly IWebDriver _webDriver;

        public AdminCategoriesPage(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public Category Create(string categoryName)
        {
            throw new System.NotImplementedException();
        }
    }
}