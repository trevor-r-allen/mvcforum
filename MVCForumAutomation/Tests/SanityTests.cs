using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCForumAutomation.Entities;
using MVCForumAutomation.Infrastructure;

namespace MVCForumAutomation.Tests
{
    [TestClass]
    public class SanityTests : MVCForumTestBase
    {
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
    }
}
