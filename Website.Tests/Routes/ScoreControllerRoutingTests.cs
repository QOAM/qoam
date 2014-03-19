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
        public void DetailsActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/score/5/".WithMethod(HttpVerbs.Get).ShouldMapTo<ScoreController>(x => x.Details(5));
        }

        [Fact]
        public void DetailsActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Details(5)));
        }

        [Fact]
        public void JournalActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/score/journal/5/".WithMethod(HttpVerbs.Get).ShouldMapTo<ScoreController>(x => x.Journal(5));
        }

        [Fact]
        public void JournalActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Journal(5)));
        }

        [Fact]
        public void JournalActionWithModelRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/score/journal/5/".WithMethod(HttpVerbs.Get).ShouldMapTo<ScoreController>(x => x.Journal(5, null));
        }

        [Fact]
        public void JournalActionWithModelDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Journal(5, null)));
        }
    }
}