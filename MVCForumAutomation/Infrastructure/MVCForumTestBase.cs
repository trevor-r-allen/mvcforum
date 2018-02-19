using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCForumAutomation.Entities;
using MVCForumAutomation.PageObjects;
using TestAutomationEssentials.Common;
using TestAutomationEssentials.Common.Configuration;

namespace MVCForumAutomation.Infrastructure
{
    [TestClass]
    public class MVCForumTestBase
    {
        private static TestEnvironment s_environment;

        public MVCForumTestBase()
        {
            TestDefaults = new TestDefaults();
            MVCForum = new MVCForumClient(TestDefaults, s_environment);
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

            LoadTestEnvironment();
        }

        private static void LoadTestEnvironment()
        {
            const string filename = "TestEnvironment.xml";
            if (!File.Exists(filename))
                Assert.Fail("Configuration file TestEnvironment.xml not found. " 
                    + "In order to create one, copy the file TestEnvironment.Template.xml " 
                    + "from the project folder to TestEnvironment.xml (in the same folder), " 
                    + "edit it according to the contained instructions and rebuild the project");

            s_environment = TestConfig.Load<TestEnvironment>(filename);
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
            return new MVCForumClient(TestDefaults, s_environment);
        }
    }
}