using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MVCForumAutomation
{
    [TestClass]
    public partial class SanityTests
    {
        /*
        Login as a registered user
        Start a discussion titled "Hi!" and body "dummy body"
        Enter the site as an anonymous user (from another browser)
        Verify that a discussion titled "Hi!" appears
        Open that discussion
        Verify that the body of the discussion is "dummy body"
        */
        [TestMethod]
        public void WhenARegisteredUserStartsADiscussionOtherAnonymousUsersCanSeeIt()
        {
            const string body = "dummy body";
            LoggedInUser userA = MVCForum.RegisterNewUserAndLogin();
            Discussion createdDiscussion = userA.CreateDiscussion(Discussion.With.Body(body));

            MVCForumClient anonymousUser = new MVCForumClient();
            DiscussionHeader latestHeader = anonymousUser.LatestDiscussions.Top;
            Assert.AreEqual(createdDiscussion.Title, latestHeader.Title, "The title of the latest discussion should match the one we created");
            Discussion viewedDiscussion = latestHeader.OpenDiscussion();
            Assert.AreEqual(body, viewedDiscussion.Body, "The body of the latest discussion should match the one we created");
        }

        public MVCForumClient MVCForum { get; } = new MVCForumClient();
    }

}
