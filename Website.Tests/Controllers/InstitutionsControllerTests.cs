namespace QOAM.Website.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Core;
    using Core.Repositories;
    using Core.Repositories.Filters;
    using Moq;
    using MvcContrib.TestHelper;
    using PagedList;
    using Ploeh.AutoFixture.Xunit2;
    using Website.Controllers;
    using Website.Helpers;
    using Website.ViewModels.Institutions;
    using Xunit;

    public class InstitutionsControllerTests
    {
        [Fact]
        public void ConstructorWithNullInstitutionRepositoryThrowsArgumentNullException()
        {
            // Arrange
            IInstitutionRepository nullInstitutionRepository = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new InstitutionsController(
                nullInstitutionRepository,
                Mock.Of<IBaseScoreCardRepository>(),
                Mock.Of<IValuationScoreCardRepository>(),
                Mock.Of<IUserProfileRepository>(),
                Mock.Of<IAuthentication>()));
        }

        [Fact]
        public void ConstructorWithNullBaseScoreCardRepositoryThrowsArgumentNullException()
        {
            // Arrange
            IBaseScoreCardRepository nullBaseScoreCardRepository = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new InstitutionsController(
                Mock.Of<IInstitutionRepository>(),
                nullBaseScoreCardRepository,
                Mock.Of<IValuationScoreCardRepository>(),
                Mock.Of<IUserProfileRepository>(),
                Mock.Of<IAuthentication>()));
        }

        [Fact]
        public void ConstructorWithNullValuationScoreCardRepositoryThrowsArgumentNullException()
        {
            // Arrange
            IValuationScoreCardRepository nullValuationScoreCardRepository = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new InstitutionsController(
                Mock.Of<IInstitutionRepository>(),
                Mock.Of<IBaseScoreCardRepository>(),
                nullValuationScoreCardRepository,
                Mock.Of<IUserProfileRepository>(),
                Mock.Of<IAuthentication>()));
        }

        [Fact]
        public void ConstructorWithNullUserProfileRepositoryThrowsArgumentNullException()
        {
            // Arrange
            IUserProfileRepository nullUserProfileRepository = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new InstitutionsController(
                Mock.Of<IInstitutionRepository>(),
                Mock.Of<IBaseScoreCardRepository>(),
                Mock.Of<IValuationScoreCardRepository>(),
                nullUserProfileRepository,
                Mock.Of<IAuthentication>()));
        }

        [Fact]
        public void ConstructorWithNullAuthenticationThrowsArgumentNullException()
        {
            // Arrange
            IAuthentication nullAuthentication = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new InstitutionsController(
                Mock.Of<IInstitutionRepository>(),
                Mock.Of<IBaseScoreCardRepository>(),
                Mock.Of<IValuationScoreCardRepository>(),
                Mock.Of<IUserProfileRepository>(),
                nullAuthentication));
        }

        [Fact]
        public void IndexRendersView()
        {
            // Arrange
            var institutionsController = CreateInstitutionsController();

            // Act
            var viewResult = institutionsController.Index(new IndexViewModel());

            // Assert
            Assert.IsType<ViewResult>(viewResult);
        }

        [Theory, AutoData]
        public void IndexSetsNumberOfBaseScoreCardsInModel(int numberOfActiveBaseScoreCards)
        {
            // Arrange
            var baseScoreCardRepository = new Mock<IBaseScoreCardRepository>();
            baseScoreCardRepository.Setup(b => b.Count(It.Is<ScoreCardFilter>(f => f.State == ScoreCardState.Published))).Returns(numberOfActiveBaseScoreCards);

            var institutionsController = CreateInstitutionsController(baseScoreCardRepository: baseScoreCardRepository.Object);

            // Act
            var viewResult = institutionsController.Index(new IndexViewModel());

            // Assert
            Assert.Equal(numberOfActiveBaseScoreCards, viewResult.WithViewData<IndexViewModel>().NumberOfBaseScoreCards);
        }

        [Theory, AutoData]
        public void IndexSetsNumberOfValuationScoreCardsInModel(int numberOfActiveValuationScoreCards)
        {
            // Arrange
            var valuationScoreCardRepository = new Mock<IValuationScoreCardRepository>();
            valuationScoreCardRepository.Setup(b => b.Count(It.Is<ScoreCardFilter>(f => f.State == ScoreCardState.Published))).Returns(numberOfActiveValuationScoreCards);

            var institutionsController = CreateInstitutionsController(valuationScoreCardRepository: valuationScoreCardRepository.Object);

            // Act
            var viewResult = institutionsController.Index(new IndexViewModel());

            // Assert
            Assert.Equal(numberOfActiveValuationScoreCards, viewResult.WithViewData<IndexViewModel>().NumberOfValuationScoreCards);
        }

        [Fact]
        public void IndexSetsInstitutionsInModel()
        {
            // Arrange
            var institutions = new List<Institution>();
            var institutionRepository = CreateInstitutionRepository(institutions);

            var institutionsController = CreateInstitutionsController(institutionRepository: institutionRepository);

            // Act
            var viewResult = institutionsController.Index(new IndexViewModel());

            // Assert
            Assert.Equal(institutions, viewResult.WithViewData<IndexViewModel>().Institutions);
        }

        private static InstitutionsController CreateInstitutionsController(
            IInstitutionRepository institutionRepository = null,
            IBaseScoreCardRepository baseScoreCardRepository = null,
            IValuationScoreCardRepository valuationScoreCardRepository = null,
            IUserProfileRepository userProfileRepository = null,
            IAuthentication authentication = null)
        {
            return new InstitutionsController(
                institutionRepository ?? CreateInstitutionRepository(),
                baseScoreCardRepository ?? Mock.Of<IBaseScoreCardRepository>(),
                valuationScoreCardRepository ?? Mock.Of<IValuationScoreCardRepository>(),
                userProfileRepository ?? Mock.Of<IUserProfileRepository>(),
                authentication ?? Mock.Of<IAuthentication>());
        }

        private static IInstitutionRepository CreateInstitutionRepository()
        {
            return CreateInstitutionRepository(new List<Institution>());
        }

        private static IInstitutionRepository CreateInstitutionRepository(IList<Institution> institutions)
        {
            var institutionRepository = new Mock<IInstitutionRepository>();
            institutionRepository.Setup(i => i.Search(It.IsAny<InstitutionFilter>())).Returns(institutions.ToPagedList(1, int.MaxValue));

            return institutionRepository.Object;
        }
    }
}