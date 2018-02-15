using MVCForumAutomation.Entities;
using OpenQA.Selenium;

namespace MVCForumAutomation.PageObjects
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
            var createNewButton = _webDriver.FindElement(By.ClassName("auto-createCategory"));
            createNewButton.Click();

            var createCategoryPage = new CreateCategoryPage(_webDriver);
            createCategoryPage.CategoryName = categoryName;
            createCategoryPage.Create();

            return new Category(categoryName);
        }
    }
}