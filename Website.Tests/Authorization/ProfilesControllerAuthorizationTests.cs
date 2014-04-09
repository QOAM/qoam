namespace QOAM.Website.Tests.Authorization
{
    using QOAM.Website.Controllers;
    using QOAM.Website.Models;
    using QOAM.Website.ViewModels.Profiles;

    using Xunit;
    using Xunit.Extensions;

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

        [Fact]
        public void EditActionAuthorizedForAdminUser()
        {
            // Assert
            Assert.True(ActionAuthorizedForUserWithRole(x => x.Edit(5, (string)null), ApplicationRole.Admin));
        }

        [Theory]
        [InlineData(ApplicationRole.DataAdmin)]
        [InlineData(ApplicationRole.InstitutionAdmin)]
        [InlineData("unknown")]
        public void EditActionNotAuthorizedForNonAdminUser(string nonAdminUserRole)
        {
            // Assert
            Assert.False(ActionAuthorizedForUserWithRole(x => x.Edit(5, (string)null), nonAdminUserRole));
        }

        [Fact]
        public void EditActionWithModelAuthorizedForAdminUser()
        {
            // Assert
            Assert.True(ActionAuthorizedForUserWithRole(x => x.Edit(5, (EditViewModel)null), ApplicationRole.Admin));
        }

        [Theory]
        [InlineData(ApplicationRole.DataAdmin)]
        [InlineData(ApplicationRole.InstitutionAdmin)]
        [InlineData("unknown")]
        public void EditActionWithModelNotAuthorizedForNonAdminUser(string nonAdminUserRole)
        {
            // Assert
            Assert.False(ActionAuthorizedForUserWithRole(x => x.Edit(5, (EditViewModel)null), nonAdminUserRole));
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