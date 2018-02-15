using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCForumAutomation.Entities;
using OpenQA.Selenium;

namespace MVCForumAutomation.PageObjects
{
    public class CategoriesList
    {
        private readonly IWebDriver _webDriver;
        private readonly IWebElement _categoriesList;

        public CategoriesList(IWebDriver webDriver)
        {
            _webDriver = webDriver;
            _categoriesList = webDriver.FindElement(By.ClassName("categories-box"));
        }

        public CategoryView Select(Category category)
        {
            var allLinks = _categoriesList.FindElements(By.TagName("a"));
            var linkToSelect = allLinks.SingleOrDefault(link => link.Text == category.Name);
            Assert.IsNotNull(linkToSelect, $"Category {category} was not found in categories list");

            linkToSelect.Click();

            return new CategoryView(_webDriver);
        }
    }
}