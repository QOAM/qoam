namespace QOAM.Website.Tests.Routes
{
    using System.Web.Mvc;

    using MvcContrib.TestHelper;

    using QOAM.Website.Controllers;

    using Xunit;

    public class HomeControllerRoutingTests : ControllerRoutingTests<HomeController>
    {
        [Fact]
        public void IndexActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/".WithMethod(HttpVerbs.Get).ShouldMapTo<HomeController>(x => x.Index());
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
            "~/about/".WithMethod(HttpVerbs.Get).ShouldMapTo<HomeController>(x => x.About());
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
            "~/organisation/".WithMethod(HttpVerbs.Get).ShouldMapTo<HomeController>(x => x.Organisation());
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
            "~/press/".WithMethod(HttpVerbs.Get).ShouldMapTo<HomeController>(x => x.Press());
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
            "~/faq/".WithMethod(HttpVerbs.Get).ShouldMapTo<HomeController>(x => x.Faq());
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
            "~/contact/".WithMethod(HttpVerbs.Get).ShouldMapTo<HomeController>(x => x.Contact());
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
            "~/contact/sent/".WithMethod(HttpVerbs.Get).ShouldMapTo<HomeController>(x => x.ContactSent());
        }

        [Fact]
        public void ContactSentActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.ContactSent()));
        }
    }
}