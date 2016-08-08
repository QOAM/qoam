namespace QOAM.Website.Tests.Authorization
{
    using Website.Controllers;
    using Xunit;

    public class HomeControllerAuthorizationTests : ControllerAuthorizationTests<HomeController>
    {
        [Fact]
        public void IndexActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Index()));
        }

        [Fact]
        public void AboutActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.About()));
        }

        [Fact]
        public void OrganisationActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Organisation()));
        }

        [Fact]
        public void PressActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.References()));
        }

        [Fact]
        public void FaqActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Faq()));
        }

        [Fact]
        public void JournalScoreCardActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.JournalScoreCard()));
        }

        [Fact]
        public void ContactActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Contact()));
        }

        [Fact]
        public void ContactSentActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.ContactSent()));
        }
    }
}