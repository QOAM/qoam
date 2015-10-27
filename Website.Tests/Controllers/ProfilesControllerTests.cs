namespace QOAM.Website.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Core;
    using Core.Repositories;
    using Core.Repositories.Filters;
    using FsCheck.Xunit;
    using Moq;
    using MvcContrib.TestHelper;
    using PagedList;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;
    using Website.Controllers;
    using Website.Helpers;
    using Website.ViewModels.Profiles;
    using Xunit;

    public class ProfilesControllerTests
    {
        [Fact]
        public void ConstructorWithNullInstitutionRepositoryThrowsArgumentNullException()
        {
            // Arrange
            IInstitutionRepository nullInstitutionRepository = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new ProfilesController(
                nullInstitutionRepository,
                Mock.Of<IBaseScoreCardRepository>(),
                Mock.Of<IValuationScoreCardRepository>(),
                Mock.Of<IRoles>(),
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
            Assert.Throws<ArgumentNullException>(() => new ProfilesController(
                Mock.Of<IInstitutionRepository>(),
                nullBaseScoreCardRepository,
                Mock.Of<IValuationScoreCardRepository>(),
                Mock.Of<IRoles>(),
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
            Assert.Throws<ArgumentNullException>(() => new ProfilesController(
                Mock.Of<IInstitutionRepository>(),
                Mock.Of<IBaseScoreCardRepository>(),
                nullValuationScoreCardRepository,
                Mock.Of<IRoles>(),
                Mock.Of<IUserProfileRepository>(),
                Mock.Of<IAuthentication>()));
        }

        [Fact]
        public void ConstructorWithNullRolesThrowsArgumentNullException()
        {
            // Arrange
            IRoles nullRoles = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new ProfilesController(
                Mock.Of<IInstitutionRepository>(),
                Mock.Of<IBaseScoreCardRepository>(),
                Mock.Of<IValuationScoreCardRepository>(),
                nullRoles,
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
            Assert.Throws<ArgumentNullException>(() => new ProfilesController(
                Mock.Of<IInstitutionRepository>(),
                Mock.Of<IBaseScoreCardRepository>(),
                Mock.Of<IValuationScoreCardRepository>(),
                Mock.Of<IRoles>(),
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
            Assert.Throws<ArgumentNullException>(() => new ProfilesController(
                Mock.Of<IInstitutionRepository>(),
                Mock.Of<IBaseScoreCardRepository>(),
                Mock.Of<IValuationScoreCardRepository>(),
                Mock.Of<IRoles>(),
                Mock.Of<IUserProfileRepository>(),
                nullAuthentication));
        }

        [Fact]
        public void IndexRendersView()
        {
            // Arrange
            var profilesController = CreateProfilesController();

            // Act
            var viewResult = profilesController.Index(new IndexViewModel());

            // Assert
            Assert.IsType<ViewResult>(viewResult);
        }

        [Theory, AutoData]
        public void IndexSetsNumberOfBaseScoreCardsInModel(int numberOfActiveBaseScoreCards)
        {
            // Arrange
            var baseScoreCardRepository = new Mock<IBaseScoreCardRepository>();
            baseScoreCardRepository.Setup(b => b.Count(It.Is<ScoreCardFilter>(f => f.State == ScoreCardState.Published))).Returns(numberOfActiveBaseScoreCards);

            var profilesController = CreateProfilesController(baseScoreCardRepository: baseScoreCardRepository.Object);

            // Act
            var viewResult = profilesController.Index(new IndexViewModel());

            // Assert
            Assert.Equal(numberOfActiveBaseScoreCards, viewResult.WithViewData<IndexViewModel>().NumberOfBaseScoreCards);
        }

        [Theory, AutoData]
        public void IndexSetsNumberOfValuationScoreCardsInModel(int numberOfActiveValuationScoreCards)
        {
            // Arrange
            var valuationScoreCardRepository = new Mock<IValuationScoreCardRepository>();
            valuationScoreCardRepository.Setup(b => b.Count(It.Is<ScoreCardFilter>(f => f.State == ScoreCardState.Published))).Returns(numberOfActiveValuationScoreCards);

            var profilesController = CreateProfilesController(valuationScoreCardRepository: valuationScoreCardRepository.Object);

            // Act
            var viewResult = profilesController.Index(new IndexViewModel());

            // Assert
            Assert.Equal(numberOfActiveValuationScoreCards, viewResult.WithViewData<IndexViewModel>().NumberOfValuationScoreCards);
        }

        [Fact]
        public void IndexSetsInstitutionsInModel()
        {
            // Arrange
            var institutions = new List<Institution>();
            var institutionRepository = CreateInstitutionRepository(institutions);

            var profilesController = CreateProfilesController(institutionRepository: institutionRepository);

            // Act
            var viewResult = profilesController.Index(new IndexViewModel());

            // Assert
            Assert.Equal(institutions.Count + 1, viewResult.WithViewData<IndexViewModel>().Institutions.Count());
        }

        [Fact]
        public void IndexSetsProfilesInModel()
        {
            // Arrange
            var userProfiles = new List<UserProfile>();
            var userProfileRepository = CreateUserProfileRepository(userProfiles);

            var profilesController = CreateProfilesController(userProfileRepository: userProfileRepository);

            // Act
            var viewResult = profilesController.Index(new IndexViewModel());

            // Assert
            Assert.Equal(userProfiles, viewResult.WithViewData<IndexViewModel>().Profiles);
        }

        private static ProfilesController CreateProfilesController(
            IInstitutionRepository institutionRepository = null,
            IBaseScoreCardRepository baseScoreCardRepository = null,
            IValuationScoreCardRepository valuationScoreCardRepository = null,
            IRoles roles = null,
            IUserProfileRepository userProfileRepository = null,
            IAuthentication authentication = null)
        {
            return new ProfilesController(
                institutionRepository ?? CreateInstitutionRepository(),
                baseScoreCardRepository ?? Mock.Of<IBaseScoreCardRepository>(),
                valuationScoreCardRepository ?? Mock.Of<IValuationScoreCardRepository>(),
                roles ?? Mock.Of<IRoles>(),
                userProfileRepository ?? CreateUserProfileRepository(),
                authentication ?? Mock.Of<IAuthentication>());
        }

        private static IInstitutionRepository CreateInstitutionRepository()
        {
            return CreateInstitutionRepository(new List<Institution>());
        }

        private static IInstitutionRepository CreateInstitutionRepository(IList<Institution> institutions)
        {
            var institutionRepository = new Mock<IInstitutionRepository>();
            institutionRepository.Setup(i => i.All).Returns(institutions);

            return institutionRepository.Object;
        }

        private static IUserProfileRepository CreateUserProfileRepository()
        {
            return CreateUserProfileRepository(new List<UserProfile>());
        }

        private static IUserProfileRepository CreateUserProfileRepository(IList<UserProfile> userProfiles)
        {
            var userProfileRepository = new Mock<IUserProfileRepository>();
            userProfileRepository.Setup(i => i.Search(It.IsAny<UserProfileFilter>())).Returns(userProfiles.ToPagedList(1, int.MaxValue));

            return userProfileRepository.Object;
        }
    }
}