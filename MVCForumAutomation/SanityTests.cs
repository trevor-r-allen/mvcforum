using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace MVCForumAutomation
{
    [TestClass]
    public class SanityTests
    {
        private static ExtentReports s_reports;
        public static ExtentTest TestLog;

        public SanityTests()
        {
            TestDefaults = new TestDefaults();
            MVCForum = new MVCForumClient(TestDefaults);
        }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            s_reports = new ExtentReports();
            var reportPath = "Report.html";
            var htmlReporter = new ExtentHtmlReporter(reportPath);
            context.AddResultFile(reportPath);
            s_reports.AttachReporter(htmlReporter);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestLog = s_reports.CreateTest(TestContext.TestName);

            var adminPassword = GetAdminPassword();
            var adminUser = MVCForum.LoginAsAdmin(adminPassword);
            var adminPage = adminUser.GoToAdminPage();
            var permissions = adminPage.GetPermissionsFor(TestDefaults.StandardMembers);
            permissions.AddToCategory(TestDefaults.ExampleCategory, PermissionTypes.CreateTopics);
            adminUser.Logout();

            TestLog.Info("Test Initialize Completed");
        }

        private string GetAdminPassword()
        {
            var readMeHeader = MVCForum.LatestDiscussions.Bottom;
            var readmeTopic = readMeHeader.OpenDiscussion();
            var body = readmeTopic.BodyElement;
            var password = body.FindElement(By.XPath(".//strong[2]"));
            return password.Text;
        }

        public TestContext TestContext { get; set; }

        [TestCleanup]
        public void TestCleanup()
        {
            if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
            {
                TestLog.Fail("Test outcome=" + TestContext.CurrentTestOutcome);
                var screenshotFilename = $"Screenshot.{TestContext.TestName}.jpg";
                MVCForum.TakeScreenshot(screenshotFilename);
                TestLog.AddScreenCaptureFromPath(screenshotFilename);
            }
            else
                TestLog.Pass("Test Passed");

            s_reports.Flush();
        }

        [TestMethod]
        public void AnotherTest()
        { }

        [TestMethod]
        public void WhenARegisteredUserStartsADiscussionOtherAnonymousUsersCanSeeIt()
        {
            const string body = "dummy body";
            var userA = MVCForum.RegisterNewUserAndLogin();
            var createdDiscussion = userA.CreateDiscussion(DiscussionWith.Body(body));

            var anonymousUser = OpenNewMVCForumClient();
            var latestHeader = anonymousUser.LatestDiscussions.Top;
            Assert.AreEqual(createdDiscussion.Title, latestHeader.Title,
                "The title of the latest discussion should match the one we created");
            var viewedDiscussion = latestHeader.OpenDiscussion();
            Assert.AreEqual(body, viewedDiscussion.Body, 
                "The body of the latest discussion should match the one we created");
        }

        public Discussion.DiscussionBuilder DiscussionWith
        {
            get { return new Discussion.DiscussionBuilder(TestDefaults); }
        }

        private MVCForumClient OpenNewMVCForumClient()
        {
            return new MVCForumClient(TestDefaults);
        }

        public TestDefaults TestDefaults { get; }

        public MVCForumClient MVCForum { get; }
    }
}
