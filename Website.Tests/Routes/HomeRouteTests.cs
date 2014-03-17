namespace QOAM.Website.Tests.Routes
{
    using MvcRouteTester;

    using QOAM.Website.Controllers;

    using Xunit;

    public class HomeRouteTests : RouteTest
    {
        [Fact]
        public void RootUrlRoutesToIndexAction()
        {
            // Arrange

            // Act

            // Assert    
            this.Routes.ShouldMap("/").To<HomeController>(c => c.Index());
        }

        [Fact]
        public void AboutUrlRoutesToIndexAction()
        {
            // Arrange

            // Act

            // Assert    
            this.Routes.ShouldMap("/about").To<HomeController>(c => c.About());
        }

        [Fact]
        public void OrganisationUrlRoutesToIndexAction()
        {
            // Arrange

            // Act

            // Assert    
            this.Routes.ShouldMap("/organisation").To<HomeController>(c => c.Organisation());
        }

        [Fact]
        public void PressUrlRoutesToIndexAction()
        {
            // Arrange

            // Act

            // Assert    
            this.Routes.ShouldMap("/press").To<HomeController>(c => c.Press());
        }

        [Fact]
        public void FaqUrlRoutesToIndexAction()
        {
            // Arrange

            // Act

            // Assert    
            this.Routes.ShouldMap("/faq").To<HomeController>(c => c.Faq());
        }

        [Fact]
        public void ContactUrlRoutesToIndexAction()
        {
            // Arrange

            // Act

            // Assert    
            this.Routes.ShouldMap("/contact").To<HomeController>(c => c.Contact());
        }

        [Fact]
        public void ContactSentUrlRoutesToIndexAction()
        {
            // Arrange

            // Act

            // Assert    
            this.Routes.ShouldMap("/contact/sent").To<HomeController>(c => c.ContactSent());
        }
    }
}