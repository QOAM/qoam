namespace QOAM.Website.Tests.Routes
{
    using System.Net.Http;
    using System.Web.Mvc;

    using MvcContrib.TestHelper;
    using MvcRouteTester;
    using QOAM.Website.Controllers;

    using Xunit;

    public class HomeControllerRoutingTests : ControllerRoutingTests<HomeController>
    {
        [Fact]
        public void IndexActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/").To<HomeController>(HttpMethod.Get, x => x.Index());
        }

        [Fact]
        public void IndexActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Index()));
        }

        [Fact]
        public void AboutActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/about/").To<HomeController>(HttpMethod.Get, x => x.About());
        }

        [Fact]
        public void AboutActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.About()));
        }

        [Fact]
        public void OrganisationActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/organisation/").To<HomeController>(HttpMethod.Get, x => x.Organisation());
        }

        [Fact]
        public void OrganisationActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Organisation()));
        }

        [Fact]
        public void PressActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/press/").To<HomeController>(HttpMethod.Get, x => x.Press());
        }

        [Fact]
        public void PressActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Press()));
        }

        [Fact]
        public void FaqActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/faq/").To<HomeController>(HttpMethod.Get, x => x.Faq());
        }

        [Fact]
        public void FaqActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Faq()));
        }


        [Fact]
        public void ContactActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/contact/").To<HomeController>(HttpMethod.Get, x => x.Contact());
        }

        [Fact]
        public void ContactActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Contact()));
        }

        [Fact]
        public void ContactSentActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/contact/sent/").To<HomeController>(HttpMethod.Get, x => x.ContactSent());
        }

        [Fact]
        public void ContactSentActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.ContactSent()));
        }
    }
}