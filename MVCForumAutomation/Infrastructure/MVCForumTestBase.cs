using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCForumAutomation.Entities;
using MVCForumAutomation.PageObjects;
using TestAutomationEssentials.Common;

namespace MVCForumAutomation.Infrastructure
{
    [TestClass]
    public class MVCForumTestBase
    {
        public MVCForumTestBase()
        {
            TestDefaults = new TestDefaults();
            MVCForum = new MVCForumClient(TestDefaults);
        }

        public TestContext TestContext { get; set; }

        public Discussion.DiscussionBuilder DiscussionWith
        {
            get { return new Discussion.DiscussionBuilder(TestDefaults); }
        }

        public TestDefaults TestDefaults { get; }
        public MVCForumClient MVCForum { get; }

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
                using (var adminConsole = MVCForum.OpenAdminConsole())
                {
                    var permissions = adminConsole.GetPermissionsFor(TestDefaults.StandardMembers);
                    permissions.AddToCategory(TestDefaults.ExampleCategory, PermissionTypes.CreateTopics);
                }
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            VisualLogger.ReportOutcome(TestContext, MVCForum);
        }

        protected MVCForumClient OpenNewMVCForumClient()
        {
            return new MVCForumClient(TestDefaults);
        }
    }
}