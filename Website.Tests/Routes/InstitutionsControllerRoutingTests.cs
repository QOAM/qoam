namespace QOAM.Website.Tests.Routes
{
    using System.Web.Mvc;

    using MvcContrib.TestHelper;

    using QOAM.Website.Controllers;

    using Xunit;

    public class InstitutionsControllerRoutingTests : ControllerRoutingTests<InstitutionsController>
    {
        [Fact]
        public void IndexActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/institutions/".WithMethod(HttpVerbs.Get).ShouldMapTo<InstitutionsController>(x => x.Index(null));
        }

        [Fact]
        public void IndexActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Index(null)));
        }

        [Fact]
        public void DetailsActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/institutions/5/".WithMethod(HttpVerbs.Get).ShouldMapTo<InstitutionsController>(x => x.Details(null));
        }

        [Fact]
        public void DetailsActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Details(null)));
        }

        [Fact]
        public void NamesActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/institutions/names/".WithMethod(HttpVerbs.Get).ShouldMapTo<InstitutionsController>(x => x.Names(null));
        }

        [Fact]
        public void NamesActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Names(null)));
        }
    }
}