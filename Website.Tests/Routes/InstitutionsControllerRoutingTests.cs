namespace QOAM.Website.Tests.Routes
{
    using System.Net.Http;
    using MvcRouteTester;
    using Website.Controllers;
    using Xunit;

    public class InstitutionsControllerRoutingTests : ControllerRoutingTests<InstitutionsController>
    {
        [Fact]
        public void IndexActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/institutions/").To<InstitutionsController>(HttpMethod.Get, x => x.Index(null));
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
            ApplicationRoutes.ShouldMap("~/institutions/5/").To<InstitutionsController>(HttpMethod.Get, x => x.Details(null));
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
            ApplicationRoutes.ShouldMap("~/institutions/names/").To<InstitutionsController>(HttpMethod.Get, x => x.Names(null));
        }

        [Fact]
        public void NamesActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Names(null)));
        }
    }
}