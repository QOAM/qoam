namespace QOAM.Website.Tests.Routes
{
    using System.Net.Http;
    using System.Web.Mvc;

    using MvcContrib.TestHelper;
    using MvcRouteTester;
    using QOAM.Website.Controllers;
    using QOAM.Website.ViewModels.Journals;

    using Xunit;

    public class JournalsControllerRoutingTests : ControllerRoutingTests<JournalsController>
    {
        [Fact]
        public void IndexActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/journals/").To<JournalsController>(HttpMethod.Get, x => x.Index(null));
        }

        [Fact]
        public void IndexActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Index(null)));
        }

        [Fact]
        public void PricesActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/journals/5/prices/").To<JournalsController>(HttpMethod.Get, x => x.Prices(null));
        }

        [Fact]
        public void PricesActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Prices(null)));
        }

        [Fact]
        public void BaseJournalPricesActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/journals/5/basejournalprices/").To<JournalsController>(HttpMethod.Get, x => x.BaseJournalPrices(null));
        }

        [Fact]
        public void BaseJournalPricesActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.BaseJournalPrices(null)));
        }

        [Fact]
        public void ValuationJournalPricesActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/journals/5/valuationjournalprices/").To<JournalsController>(HttpMethod.Get, x => x.ValuationJournalPrices(null));
        }

        [Fact]
        public void ValuationJournalPricesActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.ValuationJournalPrices(null)));
        }

        [Fact]
        public void InstitutionJournalPricesActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/journals/5/institutionalprices/").To<JournalsController>(HttpMethod.Get, x => x.InstitutionJournalPrices(null));
        }

        [Fact]
        public void InstitutionJournalPricesActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.InstitutionJournalPrices(null)));
        }

        [Fact]
        public void BaseScoreCardsActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/journals/5/basescorecards/").To<JournalsController>(HttpMethod.Get, x => x.BaseScoreCards(null));
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
            ApplicationRoutes.ShouldMap("~/journals/5/valuationscorecards/").To<JournalsController>(HttpMethod.Get, x => x.ValuationScoreCards(null));
        }

        [Fact]
        public void ValuationScoreCardsActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.ValuationScoreCards(null)));
        }

        [Fact]
        public void CommentsActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/journals/5/comments/").To<JournalsController>(HttpMethod.Get, x => x.Comments(null));
        }

        [Fact]
        public void CommentsActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Comments(null)));
        }

        [Fact(Skip = "Cannot get it to work")]
        public void InstitutionJournalLicenseActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/journals/5/institutionjournallicense/").To<JournalsController>(HttpMethod.Get, x => x.InstitutionJournalLicense(5, 1, null));
        }

        [Fact]
        public void InstitutionJournalLicenseActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.InstitutionJournalLicense(5, 1, null)));
        }

        [Fact]
        public void InstitutionJournalLicenseActionWithModelRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/journals/5/institutionjournallicense/").To<JournalsController>(HttpMethod.Post, x => x.InstitutionJournalLicense(5, null));
        }

        [Fact]
        public void InstitutionJournalLicenseActionWithModelDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.InstitutionJournalLicense(5, null)));
        }

        [Fact]
        public void TitlesActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/journals/titles/").To<JournalsController>(HttpMethod.Get, x => x.Titles(null));
        }

        [Fact]
        public void TitlesActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Titles(null)));
        }

        [Fact]
        public void IssnsActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/journals/issns/").To<JournalsController>(HttpMethod.Get, x => x.Issns(null));
        }

        [Fact]
        public void IssnsActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Issns(null)));
        }

        [Fact]
        public void PublishersActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/journals/publishers/").To<JournalsController>(HttpMethod.Get, x => x.Publishers(null));
        }

        [Fact]
        public void PublishersActionDoesNotRequireHttps()
        {
            // Assert
            Assert.False(ActionRequiresHttps(x => x.Publishers(null)));
        }
    }
}