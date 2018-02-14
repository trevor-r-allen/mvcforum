using System;
using OpenQA.Selenium;
using TestAutomationEssentials.Common;

namespace MVCForumAutomation
{
    public class AdminConsole : IDisposable
    {
        private readonly IWebDriver _webDriver;
        private readonly LoggedInAdmin _loggedInAdmin;
        private readonly TestDefaults _testDefaults;

        public AdminConsole(IWebDriver webDriver, TestDefaults testDefaults, LoggedInAdmin loggedInAdmin)
        {
            _webDriver = webDriver;
            _loggedInAdmin = loggedInAdmin;
            _testDefaults = testDefaults;
        }

        public RolePermissionsPage GetPermissionsFor(Role role)
        {
            using (Logger.StartSection($"Openning permissions for '{role.Name}'"))
            {
                var sideNavBar = _webDriver.FindElement(By.ClassName("side-nav"));
                var permissionsMenu = sideNavBar.FindElement(By.XPath("//a[@data-target='#permissions']"));
                permissionsMenu.Click();

                var managePermissionsMenuItem = _webDriver.FindElement(By.ClassName("auto-managePermissions"));
                managePermissionsMenuItem.Click();

                var roleButton =
                    _webDriver.FindElement(By.XPath($"//ul[@class='rolepermissionlist']//a[text()='{role.Name}']"));
                roleButton.Click();

                return new RolePermissionsPage(_webDriver);
            }
        }

        public Category CreateCategory()
        {
            var categoryName = Guid.NewGuid().ToString();

            var categoriesPage = OpenCategoriesPage();
            var category = categoriesPage.Create(categoryName);

            GetPermissionsFor(_testDefaults.StandardMembers).AddToCategory(category, PermissionTypes.CreateTopics);
            return category;
        }

        private AdminCategoriesPage OpenCategoriesPage()
        {
            var categoriesLink = _webDriver.FindElement(By.ClassName("auto-adminCategory"));
            categoriesLink.Click();

            return new AdminCategoriesPage(_webDriver);
        }

        public void Dispose()
        {
            _loggedInAdmin.Logout();
        }
    }
}