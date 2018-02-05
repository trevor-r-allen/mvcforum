using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using TestAutomationEssentials.Common;

namespace MVCForumAutomation
{
    [TestClass]
    public class SanityTests
    {
        public SanityTests()
        {
            TestDefaults = new TestDefaults();
            MVCForum = new MVCForumClient(TestDefaults);
        }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            const string reportPath = "Report.html";
            VisualLogger.Initialize(reportPath);
            context.AddResultFile(reportPath);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            VisualLogger.StartTest(TestContext.TestName);

            AddCreateTopicPermissionToStandardMembers();

            Logger.WriteLine("*** Test Initialize Completed ***");
        }

        private void AddCreateTopicPermissionToStandardMembers()
        {
            using (Logger.StartSection("Adding 'Create Topic' permission to Standard members"))
            {
                var adminPassword = GetAdminPassword();
                var adminUser = MVCForum.LoginAsAdmin(adminPassword);
                var adminPage = adminUser.GoToAdminPage();
                var permissions = adminPage.GetPermissionsFor(TestDefaults.StandardMembers);
                permissions.AddToCategory(TestDefaults.ExampleCategory, PermissionTypes.CreateTopics);
                adminUser.Logout();
            }
        }

        private string GetAdminPassword()
        {
            using (Logger.StartSection("Getting Admin password from 'Read Me' topic"))
            {
                var readMeHeader = MVCForum.LatestDiscussions.Bottom;
                var readmeTopic = readMeHeader.OpenDiscussion();
                var body = readmeTopic.BodyElement;
                var password = body.FindElement(By.XPath(".//strong[2]"));
                var adminPassword = password.Text;
                Logger.WriteLine($"Admin password='{adminPassword}'");
                return adminPassword;
            }
        }

        public TestContext TestContext { get; set; }

        [TestCleanup]
        public void TestCleanup()
        {
            VisualLogger.ReportOutcome(TestContext, MVCForum);
        }

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
