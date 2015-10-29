namespace QOAM.Website.Tests.Authorization
{
    using Website.Controllers;
    using Website.Models;
    using Website.ViewModels.Profiles;
    using Xunit;

    public class ProfilesControllerAuthorizationTests : ControllerAuthorizationTests<ProfilesController>
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
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Details(null)));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.DataAdmin, false)]
        [InlineData(ApplicationRole.InstitutionAdmin, false)]
        [InlineData("unknown", false)]
        [InlineData("", false)]
        public void EditActionHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.Edit(5, (string)null), role));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.DataAdmin, false)]
        [InlineData(ApplicationRole.InstitutionAdmin, false)]
        [InlineData("unknown", false)]
        [InlineData("", false)]
        public void EditActionWithModelHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.Edit(5, (EditViewModel)null), role));
        }
        
        [Fact]
        public void ScoreCardsActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.BaseScoreCards(null)));
        }

        [Fact]
        public void NamesActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Names(null)));
        }
    }
}