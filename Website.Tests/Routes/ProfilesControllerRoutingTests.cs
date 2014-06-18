namespace QOAM.Website.Tests.Routes
{
    using System.Web.Mvc;

    using MvcContrib.TestHelper;

    using QOAM.Website.Controllers;
    using QOAM.Website.ViewModels.Profiles;

    using Xunit;

    public class ProfilesControllerRoutingTests : ControllerRoutingTests<ProfilesController>
    {
        [Fact]
        public void IndexActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/profiles/".WithMethod(HttpVerbs.Get).ShouldMapTo<ProfilesController>(x => x.Index(null));
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
            "~/profiles/5/".WithMethod(HttpVerbs.Get).ShouldMapTo<ProfilesController>(x => x.Details(null));
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
            "~/profiles/5/edit/".WithMethod(HttpVerbs.Get).ShouldMapTo<ProfilesController>(x => x.Edit(5, (string)null));
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
            "~/profiles/5/edit/".WithMethod(HttpVerbs.Post).ShouldMapTo<ProfilesController>(x => x.Edit(5, (EditViewModel)null));
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
            "~/profiles/5/basescorecards/".WithMethod(HttpVerbs.Get).ShouldMapTo<ProfilesController>(x => x.BaseScoreCards(null));
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
            "~/profiles/5/valuationscorecards/".WithMethod(HttpVerbs.Get).ShouldMapTo<ProfilesController>(x => x.ValuationScoreCards(null));
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
            "~/profiles/names/".WithMethod(HttpVerbs.Get).ShouldMapTo<ProfilesController>(x => x.Names(null));
        }

        [Fact]
        public void NamesActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Names(null)));
        }
    }
}