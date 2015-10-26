namespace QOAM.Website.Tests.Routes
{
    using System.Net.Http;
    using MvcRouteTester;
    using QOAM.Website.Controllers;
    using Website.ViewModels.Import;
    using Xunit;

    public class AdminControllerRoutingTests : ControllerRoutingTests<AdminController>
    {
        [Fact]
        public void IndexActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert
            ApplicationRoutes.ShouldMap("~/admin/").To<AdminController>(HttpMethod.Get, x => x.Index());
        }

        [Fact]
        public void IndexActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Index()));
        }

        [Fact]
        public void ImportActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/admin/import/").To<AdminController>(HttpMethod.Get, x => x.Import());
        }

        [Fact]
        public void ImportActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Import()));
        }
        
        [Fact]
        public void ImportActionWithModelRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/admin/import/").To<AdminController>(HttpMethod.Post, x => x.Import(null));
        }

        [Fact]
        public void ImportActionWithModelDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Import(null)));
        }

        [Fact]
        public void ImportedActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/admin/imported/").To<AdminController>(HttpMethod.Get, x => x.Imported());
        }

        [Fact]
        public void ImportedActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Imported()));
        }

        [Fact]
        public void UpdateActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/admin/update/").To<AdminController>(HttpMethod.Get, x => x.Update());
        }

        [Fact]
        public void UpdateActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Update()));
        }

        [Fact]
        public void UpdateActionWithModelRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/admin/update/").To<AdminController>(HttpMethod.Post, x => x.Update(null));
        }

        [Fact]
        public void UpdateActionWithModelDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Update(null)));
        }

        [Fact]
        public void UpdatedActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/admin/updated/").To<AdminController>(HttpMethod.Get, x => x.Updated());
        }

        [Fact]
        public void UpdatedActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Updated()));
        }

        [Fact]
        public void DownloadActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/admin/download/").To<AdminController>(HttpMethod.Get, x => x.Download());
        }

        [Fact]
        public void DownloadActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Download()));
        }

        [Fact]
        public void CheckActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert
            ApplicationRoutes.ShouldMap("~/admin/check/").To<AdminController>(HttpMethod.Get, x => x.Check());
        }

        [Fact]
        public void CheckActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Check()));
        }

        [Fact]
        public void CheckActionWithModelRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/admin/check/").To<AdminController>(HttpMethod.Post, x => x.Check(null));
        }

        [Fact]
        public void CheckActionWithModelDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Check(null)));
        }

        [Fact]
        public void MoveScoreCardsActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/admin/movescorecards/").To<AdminController>(HttpMethod.Get, x => x.MoveScoreCards((bool?)null));
        }

        [Fact]
        public void MoveScoreCardsActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.MoveScoreCards((bool?)null)));
        }

        [Fact]
        public void MoveScoreCardsActionWithModelRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/admin/movescorecards/").To<AdminController>(HttpMethod.Post, x => x.MoveScoreCards((MoveScoreCardsViewModel) null));
        }

        [Fact]
        public void MoveScoreCardsActionWithModelDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.MoveScoreCards((MoveScoreCardsViewModel) null)));
        }

        [Fact]
        public void RemoveBaseScoreCardActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/admin/removebasescorecard/5").To<AdminController>(HttpMethod.Get, x => x.RemoveBaseScoreCard(null));
        }

        [Fact]
        public void RemoveBaseScoreCardActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.RemoveBaseScoreCard(null)));
        }

        [Fact]
        public void RemoveBaseScoreCardPostActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/admin/removebasescorecard/5").To<AdminController>(HttpMethod.Post, x => x.RemoveBaseScoreCardPost(null));
        }

        [Fact]
        public void RemoveBaseScoreCardPostActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.RemoveBaseScoreCardPost(null)));
        }

        [Fact]
        public void RemovedBaseScoreCardActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/admin/removedbasescorecard/").To<AdminController>(HttpMethod.Get, x => x.RemovedBaseScoreCard());
        }

        [Fact]
        public void RemovedBaseScoreCardActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.RemovedBaseScoreCard()));
        }

        [Fact]
        public void RemoveValuationScoreCardActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/admin/removevaluationscorecard/7").To<AdminController>(HttpMethod.Get, x => x.RemoveValuationScoreCard(null));
        }

        [Fact]
        public void RemoveValuationScoreCardActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.RemoveValuationScoreCard(null)));
        }

        [Fact]
        public void RemoveValuationScoreCardPostActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/admin/removevaluationscorecard/7").To<AdminController>(HttpMethod.Post, x => x.RemoveValuationScoreCardPost(null));
        }

        [Fact]
        public void RemoveValuationScoreCardPostActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.RemoveValuationScoreCardPost(null)));
        }

        [Fact]
        public void RemovedValuationScoreCardActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/admin/removedvaluationscorecard/").To<AdminController>(HttpMethod.Get, x => x.RemovedValuationScoreCard());
        }

        [Fact]
        public void RemovedValuationScoreCardActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.RemovedValuationScoreCard()));
        }
    }
}