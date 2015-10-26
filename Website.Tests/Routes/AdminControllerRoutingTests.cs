namespace QOAM.Website.Tests.Routes
{
    using System.Web.Mvc;

    using MvcContrib.TestHelper;

    using QOAM.Website.Controllers;
    using Website.ViewModels.Institutions;
    using Xunit;

    public class AdminControllerRoutingTests : ControllerRoutingTests<AdminController>
    {
        [Fact]
        public void IndexActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/admin/".WithMethod(HttpVerbs.Get).ShouldMapTo<AdminController>(x => x.Index());
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
            "~/admin/import/".WithMethod(HttpVerbs.Get).ShouldMapTo<AdminController>(x => x.Import());
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
            "~/admin/import/".WithMethod(HttpVerbs.Post).ShouldMapTo<AdminController>(x => x.Import(null));
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
            "~/admin/imported/".WithMethod(HttpVerbs.Get).ShouldMapTo<AdminController>(x => x.Imported());
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
            "~/admin/update/".WithMethod(HttpVerbs.Get).ShouldMapTo<AdminController>(x => x.Update());
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
            "~/admin/update/".WithMethod(HttpVerbs.Post).ShouldMapTo<AdminController>(x => x.Update(null));
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
            "~/admin/updated/".WithMethod(HttpVerbs.Get).ShouldMapTo<AdminController>(x => x.Updated());
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
            "~/admin/download/".WithMethod(HttpVerbs.Get).ShouldMapTo<AdminController>(x => x.Download());
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
            "~/admin/check/".WithMethod(HttpVerbs.Get).ShouldMapTo<AdminController>(x => x.Check());
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
            "~/admin/check/".WithMethod(HttpVerbs.Post).ShouldMapTo<AdminController>(x => x.Check(null));
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
            "~/admin/movescorecards/".WithMethod(HttpVerbs.Get).ShouldMapTo<AdminController>(x => x.MoveScoreCards((bool?)null));
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
            "~/admin/movescorecards/".WithMethod(HttpVerbs.Post).ShouldMapTo<AdminController>(x => x.MoveScoreCards((MoveScoreCardsViewModel) null));
        }

        [Fact]
        public void MoveScoreCardsActionWithModelDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.MoveScoreCards((MoveScoreCardsViewModel) null)));
        }
    }
}