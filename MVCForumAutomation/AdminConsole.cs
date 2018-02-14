using System;
using OpenQA.Selenium;
using TestAutomationEssentials.Common;

namespace MVCForumAutomation
{
    public class AdminConsole : IDisposable
    {
        private readonly IWebDriver _webDriver;
        private readonly LoggedInAdmin _loggedInAdmin;

        public AdminConsole(
            IWebDriver webDriver, LoggedInAdmin loggedInAdmin)
        {
            _webDriver = webDriver;
            _loggedInAdmin = loggedInAdmin;
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
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _loggedInAdmin.Logout();
        }
    }
}