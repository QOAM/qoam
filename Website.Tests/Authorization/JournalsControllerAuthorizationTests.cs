namespace QOAM.Website.Tests.Authorization
{
    using QOAM.Website.Controllers;
    using QOAM.Website.Models;
    using QOAM.Website.ViewModels.Journals;

    using Xunit;
    using Xunit.Extensions;

    public class JournalsControllerAuthorizationTests : ControllerAuthorizationTests<JournalsController>
    {
        [Fact]
        public void IndexActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Index(null)));
        }

        [Fact]
        public void PricesActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Prices(null)));
        }

        [Fact]
        public void JournalPricesActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.BaseJournalPrices(null)));
        }

        [Fact]
        public void InstitutionJournalPricesActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.InstitutionJournalPrices(null)));
        }

        [Fact]
        public void ScoresActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.BaseScoreCards(null)));
        }

        [Fact]
        public void CommentsActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Comments(null)));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin)]
        [InlineData(ApplicationRole.InstitutionAdmin)]
        public void InstitutionJournalLicenseActionAuthorizedForCorrectRoles(string authorizedRole)
        {
            // Assert
            Assert.True(ActionAuthorizedForUserWithRole(x => x.InstitutionJournalLicense(5, 1, (string)null), authorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.DataAdmin)]
        [InlineData("invalid")]
        public void InstitutionJournalLicenseActionNotAuthorizedForInvalidRoles(string unauthorizedRole)
        {
            // Assert
            Assert.False(ActionAuthorizedForUserWithRole(x => x.InstitutionJournalLicense(5, 1, (string)null), unauthorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin)]
        [InlineData(ApplicationRole.InstitutionAdmin)]
        public void InstitutionJournalLicenseActionWithModelAuthorizedForCorrectRoles(string authorizedRole)
        {
            // Assert
            Assert.True(ActionAuthorizedForUserWithRole(x => x.InstitutionJournalLicense(5, (InstitutionJournalLicenseViewModel)null), authorizedRole));
        }

        [Theory]
        [InlineData(ApplicationRole.DataAdmin)]
        [InlineData("invalid")]
        public void InstitutionJournalLicenseActionWithModelNotAuthorizedForInvalidRoles(string unauthorizedRole)
        {
            // Assert
            Assert.False(ActionAuthorizedForUserWithRole(x => x.InstitutionJournalLicense(5, (InstitutionJournalLicenseViewModel)null), unauthorizedRole));
        }

        [Fact]
        public void TitlesActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Titles(null)));
        }

        [Fact]
        public void IssnsActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Issns(null)));
        }

        [Fact]
        public void PublishersActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Publishers(null)));
        }
    }
}