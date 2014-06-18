namespace QOAM.Website.Tests.Routes
{
    using System.Web.Mvc;

    using MvcContrib.TestHelper;

    using QOAM.Website.Controllers;

    using Xunit;

    public class ScoreControllerRoutingTests : ControllerRoutingTests<ScoreController>
    {
        [Fact]
        public void IndexActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/score/".WithMethod(HttpVerbs.Get).ShouldMapTo<ScoreController>(x => x.Index(null));
        }

        [Fact]
        public void IndexActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Index(null)));
        }

        [Fact]
        public void BaseScoreCardActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/score/basescorecard/5/".WithMethod(HttpVerbs.Get).ShouldMapTo<ScoreController>(x => x.BaseScoreCard(5));
        }

        [Fact]
        public void BaseScoreCardActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.BaseScoreCard(5)));
        }

        [Fact]
        public void BaseScoreCardActionWithModelRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/score/basescorecard/5/".WithMethod(HttpVerbs.Get).ShouldMapTo<ScoreController>(x => x.BaseScoreCard(5, null));
        }

        [Fact]
        public void BaseScoreCardActionWithModelDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.BaseScoreCard(5, null)));
        }

        [Fact]
        public void ValuationScoreCardActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/score/valuationscorecard/5/".WithMethod(HttpVerbs.Get).ShouldMapTo<ScoreController>(x => x.ValuationScoreCard(5));
        }

        [Fact]
        public void ValuationScoreCardActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.ValuationScoreCard(5)));
        }

        [Fact]
        public void ValuationScoreCardActionWithModelRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/score/valuationscorecard/5/".WithMethod(HttpVerbs.Get).ShouldMapTo<ScoreController>(x => x.ValuationScoreCard(5, null));
        }

        [Fact]
        public void ValuationScoreCardActionWithModelDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.ValuationScoreCard(5, null)));
        }
        
        [Fact]
        public void BaseScoreCardDetailsActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/score/basescorecard/details/5/".WithMethod(HttpVerbs.Get).ShouldMapTo<ScoreController>(x => x.BaseScoreCardDetails(5));
        }

        [Fact]
        public void BaseScoreCardDetailsActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.BaseScoreCardDetails(5)));
        }

        [Fact]
        public void ValuationScoreCardDetailsActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/score/valuationscorecard/details/5/".WithMethod(HttpVerbs.Get).ShouldMapTo<ScoreController>(x => x.ValuationScoreCardDetails(5));
        }

        [Fact]
        public void ValuationScoreCardDetailsActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.ValuationScoreCardDetails(5)));
        }
    }
}