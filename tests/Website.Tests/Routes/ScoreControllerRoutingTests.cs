namespace QOAM.Website.Tests.Routes
{
    using System.Net.Http;
    using MvcRouteTester;
    using Website.Controllers;
    using Xunit;

    public class ScoreControllerRoutingTests : ControllerRoutingTests<ScoreController>
    {
        [Fact]
        public void IndexActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/score/").To<ScoreController>(HttpMethod.Get, x => x.Index(null));
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
            ApplicationRoutes.ShouldMap("~/score/basescorecard/5/").To<ScoreController>(HttpMethod.Get, x => x.BaseScoreCard(5));
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
            ApplicationRoutes.ShouldMap("~/score/basescorecard/5/").To<ScoreController>(HttpMethod.Get, x => x.BaseScoreCard(5, null));
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
            ApplicationRoutes.ShouldMap("~/score/valuationscorecard/5/").To<ScoreController>(HttpMethod.Get, x => x.ValuationScoreCard(5));
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
            ApplicationRoutes.ShouldMap("~/score/valuationscorecard/5/").To<ScoreController>(HttpMethod.Get, x => x.ValuationScoreCard(5, null));
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
            ApplicationRoutes.ShouldMap("~/score/basescorecard/details/5/").To<ScoreController>(HttpMethod.Get, x => x.BaseScoreCardDetails(5));
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
            ApplicationRoutes.ShouldMap("~/score/valuationscorecard/details/5/").To<ScoreController>(HttpMethod.Get, x => x.ValuationScoreCardDetails(5));
        }

        [Fact]
        public void ValuationScoreCardDetailsActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.ValuationScoreCardDetails(5)));
        }
    }
}