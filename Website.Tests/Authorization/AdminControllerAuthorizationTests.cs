namespace QOAM.Website.Tests.Authorization
{
    using QOAM.Website.Controllers;
    using QOAM.Website.Models;

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
        [InlineData(ApplicationRole.Admin)]
        [InlineData(ApplicationRole.DataAdmin)]
        public void ImportActionAuthorizedForCorrectRoles(string authorizedRole)
        {
            // Assert
            Assert.True(ActionAuthorizedForUserWithRole(x => x.Import(), authorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.InstitutionAdmin)]
        [InlineData("invalid")]
        public void ImportActionNotAuthorizedForInvalidRoles(string unauthorizedRole)
        {
            // Assert
            Assert.False(ActionAuthorizedForUserWithRole(x => x.Import(), unauthorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin)]
        [InlineData(ApplicationRole.DataAdmin)]
        public void ImportActionWithModelAuthorizedForCorrectRoles(string authorizedRole)
        {
            // Assert
            Assert.True(ActionAuthorizedForUserWithRole(x => x.Import(null), authorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.InstitutionAdmin)]
        [InlineData("invalid")]
        public void ImportActionWithModelNotAuthorizedForInvalidRoles(string unauthorizedRole)
        {
            // Assert
            Assert.False(ActionAuthorizedForUserWithRole(x => x.Import(null), unauthorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin)]
        [InlineData(ApplicationRole.DataAdmin)]
        public void ImportedActionAuthorizedForCorrectRoles(string authorizedRole)
        {
            // Assert
            Assert.True(ActionAuthorizedForUserWithRole(x => x.Imported(), authorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.InstitutionAdmin)]
        [InlineData("invalid")]
        public void ImportedActionNotAuthorizedForInvalidRoles(string unauthorizedRole)
        {
            // Assert
            Assert.False(ActionAuthorizedForUserWithRole(x => x.Imported(), unauthorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin)]
        [InlineData(ApplicationRole.DataAdmin)]
        public void UpdateActionAuthorizedForCorrectRoles(string authorizedRole)
        {
            // Assert
            Assert.True(ActionAuthorizedForUserWithRole(x => x.Update(), authorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.InstitutionAdmin)]
        [InlineData("invalid")]
        public void UpdateActionNotAuthorizedForInvalidRoles(string unauthorizedRole)
        {
            // Assert
            Assert.False(ActionAuthorizedForUserWithRole(x => x.Update(), unauthorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin)]
        [InlineData(ApplicationRole.DataAdmin)]
        public void UpdateActionWithModelAuthorizedForCorrectRoles(string authorizedRole)
        {
            // Assert
            Assert.True(ActionAuthorizedForUserWithRole(x => x.Update(null), authorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.InstitutionAdmin)]
        [InlineData("invalid")]
        public void UpdateActionWithModelNotAuthorizedForInvalidRoles(string unauthorizedRole)
        {
            // Assert
            Assert.False(ActionAuthorizedForUserWithRole(x => x.Update(null), unauthorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin)]
        [InlineData(ApplicationRole.DataAdmin)]
        public void UpdatedActionAuthorizedForCorrectRoles(string authorizedRole)
        {
            // Assert
            Assert.True(ActionAuthorizedForUserWithRole(x => x.Updated(), authorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.InstitutionAdmin)]
        [InlineData("invalid")]
        public void UpdatedActionNotAuthorizedForInvalidRoles(string unauthorizedRole)
        {
            // Assert
            Assert.False(ActionAuthorizedForUserWithRole(x => x.Updated(), unauthorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin)]
        [InlineData(ApplicationRole.DataAdmin)]
        public void DownloadActionAuthorizedForCorrectRoles(string authorizedRole)
        {
            // Assert
            Assert.True(ActionAuthorizedForUserWithRole(x => x.Download(), authorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.InstitutionAdmin)]
        [InlineData("invalid")]
        public void DownloadActionNotAuthorizedForInvalidRoles(string unauthorizedRole)
        {
            // Assert
            Assert.False(ActionAuthorizedForUserWithRole(x => x.Download(), unauthorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin)]
        [InlineData(ApplicationRole.InstitutionAdmin)]
        public void CheckActionAuthorizedForCorrectRoles(string authorizedRole)
        {
            // Assert
            Assert.True(ActionAuthorizedForUserWithRole(x => x.Check(), authorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.DataAdmin)]
        [InlineData("invalid")]
        public void CheckActionNotAuthorizedForInvalidRoles(string unauthorizedRole)
        {
            // Assert
            Assert.False(ActionAuthorizedForUserWithRole(x => x.Check(), unauthorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin)]
        [InlineData(ApplicationRole.InstitutionAdmin)]
        public void CheckActionWithModelAuthorizedForCorrectRoles(string authorizedRole)
        {
            // Assert
            Assert.True(ActionAuthorizedForUserWithRole(x => x.Check(null), authorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.DataAdmin)]
        [InlineData("invalid")]
        public void CheckActionWithModelNotAuthorizedForInvalidRoles(string unauthorizedRole)
        {
            // Assert
            Assert.False(ActionAuthorizedForUserWithRole(x => x.Check(null), unauthorizedRole));
        }
    }
}