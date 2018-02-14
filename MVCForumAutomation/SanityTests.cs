using System.Linq;
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
                var adminPassword = MVCForum.GetAdminPassword();
                var adminUser = MVCForum.LoginAsAdmin(adminPassword);
                var adminConsole = adminUser.GoToAdminConsole();
                var permissions = adminConsole.GetPermissionsFor(TestDefaults.StandardMembers);
                permissions.AddToCategory(TestDefaults.ExampleCategory, PermissionTypes.CreateTopics);
                adminUser.Logout();
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

        [TestMethod]
        public void DiscussionsCanBeFilteredByCategory()
        {
            Category categoryA, categoryB;
            using (var adminConsole = MVCForum.OpenAdminConsole())
            {
                categoryA = adminConsole.CreateCategory();
                categoryB = adminConsole.CreateCategory();
            }

            var user = MVCForum.RegisterNewUserAndLogin();
            var discussion = user.CreateDiscussion(DiscussionWith.Category(categoryA));

            var categoryView = MVCForum.Categories.Select(categoryA);
            Assert.AreEqual(1, categoryView.Discussions.Count, $"1 discussion is expected in categoryA ({categoryA.Name})");
            Assert.AreEqual(discussion, categoryView.Discussions.Single(), $"The single discussion in categoryA ({categoryA.Name}) is expected to be the same category that we've created ('{discussion.Title}");

            categoryView = MVCForum.Categories.Select(categoryB);
            Assert.AreEqual(0, categoryView.Discussions.Count, $"No discussions expected in categoryB ({categoryB.Name}");
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
