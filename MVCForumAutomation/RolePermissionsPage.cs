using System.Linq;
using OpenQA.Selenium;
using TestAutomationEssentials.Common;

namespace MVCForumAutomation
{
    public class RolePermissionsPage
    {
        private readonly IWebDriver _webDriver;

        public RolePermissionsPage(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public void AddToCategory(Category category, PermissionTypes permissionType)
        {
            using (Logger.StartSection($"Adding permission '{permissionType}' to category '{category.Name}'"))
            {
                var permissionsTable = _webDriver.FindElement(By.ClassName("permissiontable"));

                var categoryRows = permissionsTable.FindElements(By.CssSelector(".permissiontable tbody tr"));
                var categoryRow = categoryRows.Single(row => row.FindElement(By.XPath("./td")).Text == category.Name);

                var permissionCheckboxes = categoryRow.FindElements(By.CssSelector(".permissioncheckbox input"));
                var permissionCheckbox = permissionCheckboxes[(int) permissionType];
                if (!permissionCheckbox.Selected)
                    permissionCheckbox.Click();
            }
        }
    }
}