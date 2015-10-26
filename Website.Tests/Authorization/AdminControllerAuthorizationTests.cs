namespace QOAM.Website.Tests.Authorization
{
    using QOAM.Website.Controllers;
    using QOAM.Website.Models;
    using Website.ViewModels.Import;
    using Xunit;
    using Xunit.Extensions;

    public class AdminControllerAuthorizationTests : ControllerAuthorizationTests<AdminController>
    {
        [Fact]
        public void IndexActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionRequiresAuthorizedUser(x => x.Index()));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.DataAdmin, true)]
        [InlineData(ApplicationRole.InstitutionAdmin, false)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public void ImportActionHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.Import(), role));
        }
        
        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.DataAdmin, true)]
        [InlineData(ApplicationRole.InstitutionAdmin, false)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public void ImportActionWithModelHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.Import(null), role));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.DataAdmin, true)]
        [InlineData(ApplicationRole.InstitutionAdmin, false)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public void ImportedActionHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.Imported(), role));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.DataAdmin, true)]
        [InlineData(ApplicationRole.InstitutionAdmin, false)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public void UpdateActionHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.Update(), role));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.DataAdmin, true)]
        [InlineData(ApplicationRole.InstitutionAdmin, false)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public void UpdateActionWithModelHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.Update(null), role));
        }
        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.DataAdmin, true)]
        [InlineData(ApplicationRole.InstitutionAdmin, false)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public void UpdatedActionHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.Updated(), role));
        }
        
        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.DataAdmin, true)]
        [InlineData(ApplicationRole.InstitutionAdmin, false)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public void DownloadActionHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.Download(), role));
        }
        
        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.DataAdmin, false)]
        [InlineData(ApplicationRole.InstitutionAdmin, true)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public void CheckActionHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.Check(), role));
        }
        
        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.DataAdmin, false)]
        [InlineData(ApplicationRole.InstitutionAdmin, true)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public void CheckActionWithModelHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.Check(null), role));
        }
        
        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.DataAdmin, true)]
        [InlineData(ApplicationRole.InstitutionAdmin, false)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public void MoveScoreCardsActionHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.MoveScoreCards((bool?)null), role));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.DataAdmin, true)]
        [InlineData(ApplicationRole.InstitutionAdmin, false)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public void MoveScoreCardsWithModelActionHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.MoveScoreCards((MoveScoreCardsViewModel)null), role));
        }
        
        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.DataAdmin, true)]
        [InlineData(ApplicationRole.InstitutionAdmin, false)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public void RemoveBaseScoreCardActionHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.RemoveBaseScoreCard(null), role));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.DataAdmin, true)]
        [InlineData(ApplicationRole.InstitutionAdmin, false)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public void RemoveBaseScoreCardPostActionHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.RemoveBaseScoreCardPost(null), role));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.DataAdmin, true)]
        [InlineData(ApplicationRole.InstitutionAdmin, false)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public void RemovedBaseScoreCardActionHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.RemovedBaseScoreCard(), role));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.DataAdmin, true)]
        [InlineData(ApplicationRole.InstitutionAdmin, false)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public void RemoveValuationScoreCardActionHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.RemoveValuationScoreCard(null), role));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.DataAdmin, true)]
        [InlineData(ApplicationRole.InstitutionAdmin, false)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public void RemoveValuationScoreCardPostActionHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.RemoveValuationScoreCardPost(null), role));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.DataAdmin, true)]
        [InlineData(ApplicationRole.InstitutionAdmin, false)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public void RemovedValuationScoreCardActionHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.RemovedValuationScoreCard(), role));
        }
    }
}