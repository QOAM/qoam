namespace QOAM.Website.Tests.Routes
{
    using System.Net.Http;
    using MvcRouteTester;
    using Website.Controllers;
    using Website.ViewModels.Profiles;
    using Xunit;

    public class ProfilesControllerRoutingTests : ControllerRoutingTests<ProfilesController>
    {
        [Fact]
        public void IndexActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/profiles/").To<ProfilesController>(HttpMethod.Get, x => x.Index(null));
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
            ApplicationRoutes.ShouldMap("~/profiles/5/").To<ProfilesController>(HttpMethod.Get, x => x.Details(null));
        }

        [Fact]
        public void DetailsActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Details(null)));
        }

        [Fact]
        public void EditActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/profiles/5/edit/").To<ProfilesController>(HttpMethod.Get, x => x.Edit(5, (string)null));
        }

        [Fact]
        public void EditActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Edit(5, (string)null)));
        }

        [Fact]
        public void EditActionWithModelRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/profiles/5/edit/").To<ProfilesController>(HttpMethod.Post, x => x.Edit(5, (EditViewModel)null));
        }

        [Fact]
        public void EditActionWithModelDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Edit(5, (EditViewModel)null)));
        }

        [Fact]
        public void BaseScoreCardsActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/profiles/5/basescorecards/").To<ProfilesController>(HttpMethod.Get, x => x.BaseScoreCards(null));
        }

        [Fact]
        public void BaseScoreCardsActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.BaseScoreCards(null)));
        }

        [Fact]
        public void ValuationScoreCardsActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/profiles/5/valuationscorecards/").To<ProfilesController>(HttpMethod.Get, x => x.ValuationScoreCards(null));
        }

        [Fact]
        public void ValuationScoreCardsActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.ValuationScoreCards(null)));
        }

        [Fact]
        public void NamesActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/profiles/names/").To<ProfilesController>(HttpMethod.Get, x => x.Names(null));
        }

        [Fact]
        public void NamesActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Names(null)));
        }
    }
}