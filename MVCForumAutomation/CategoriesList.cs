using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace MVCForumAutomation
{
    public class CategoriesList
    {
        private readonly IWebElement _categoriesList;

        public CategoriesList(IWebDriver webDriver)
        {
            _categoriesList = webDriver.FindElement(By.ClassName("categories-box"));
        }

        public CategoryView Select(Category category)
        {
            var allLinks = _categoriesList.FindElements(By.TagName("a"));
            var linkToSelect = allLinks.SingleOrDefault(link => link.Text == category.Name);
            Assert.IsNotNull(linkToSelect, $"Category {category} was not found in categories list");

            linkToSelect.Click();

            return new CategoryView();
        }
    }
}