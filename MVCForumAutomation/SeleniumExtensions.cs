using System.Linq;
using OpenQA.Selenium;

namespace MVCForumAutomation
{
    public static class SeleniumExtensions
    {
        /// <summary>
        /// Tries to find the element using the specified locator
        /// </summary>
        /// <param name="context">The context in which to find the element. This is typically an object implementing <see cref="IWebDriver"/> or <see cref="IWebElement"/></param>
        /// <param name="locator">The locator (<see cref="By"/>) to use for finding the element</param>
        /// <returns>An <see cref="IWebElement"/> representing the element if found, or null if the element could not be found</returns>
        public static IWebElement TryFindElement(this ISearchContext context, By locator)
        {
            var matchineElements = context.FindElements(locator);
            return matchineElements.FirstOrDefault();
        }
    }
}
