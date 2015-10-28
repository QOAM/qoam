namespace QOAM.Website.Tests.Authorization
{
    using Website.Controllers;
    using Xunit;

    public class ScoreControllerAuthorizationTests : ControllerAuthorizationTests<ScoreController>
    {
        [Fact]
        public void IndexActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Index(null)));
        }

        [Fact]
        public void DetailsActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.BaseScoreCardDetails(5)));
        }

        [Fact]
        public void JournalActionAuthorizedForAdminUser()
        {
            // Assert
            Assert.True(ActionRequiresAuthorizedUser(x => x.BaseScoreCard(5)));
        }

        [Fact]
        public void JournalActionWithModelAuthorizedForAdminUser()
        {
            // Assert
            Assert.True(ActionRequiresAuthorizedUser(x => x.BaseScoreCard(5, null)));
        }
    }
}