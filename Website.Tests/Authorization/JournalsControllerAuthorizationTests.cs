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
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.InstitutionAdmin, true)]
        [InlineData(ApplicationRole.DataAdmin, false)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public void InstitutionJournalLicenseActionHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.InstitutionJournalLicense(5, 1, null), role));
        }

        [Theory]
        [InlineData(ApplicationRole.Admin, true)]
        [InlineData(ApplicationRole.InstitutionAdmin, true)]
        [InlineData(ApplicationRole.DataAdmin, false)]
        [InlineData("invalid", false)]
        [InlineData("", false)]
        public void InstitutionJournalLicenseActionWithModelHasCorrectAuthorization(string role, bool expectedAuthorized)
        {
            // Assert
            Assert.Equal(expectedAuthorized, ActionAuthorizedForUserWithRole(x => x.InstitutionJournalLicense(5, null), role));
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