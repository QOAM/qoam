namespace QOAM.Website.Tests.Models
{
    using System;

    using QOAM.Core;
    using QOAM.Website.Models;

    using Xunit;

    public class QuestionExtensionsTests
    {
        [Fact]
        public void ToLocalizedStringOnNullQuestionThrowsArgumentNullException()
        {
            // Arrange
            Question nullQuestion = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => nullQuestion.ToLocalizedString());
        }

        [Fact]
        public void ToLocalizedStringOnWebsiteContainsAimsScopeAndReadershipKeyReturnsCorrectQuestionString()
        {
            // Arrange
            var question = new Question { Key = QuestionKey.WebsiteContainsAimsScopeAndReadership };

            // Act
            var localizedQuestion = question.ToLocalizedString();

            // Assert
            Assert.Equal("Aims, scope, and target audience of the journal are clearly specified on the journal’s website", localizedQuestion);
        }

        [Fact]
        public void ToLocalizedStringOnWebsiteContainsNameAndAffiliationsOfMembersEditorialBoardKeyReturnsCorrectQuestionString()
        {
            // Arrange
            var question = new Question { Key = QuestionKey.WebsiteContainsNameAndAffiliationsOfMembersEditorialBoard };

            // Act
            var localizedQuestion = question.ToLocalizedString();

            // Assert
            Assert.Equal("The names and affiliations of members of the editorial board are listed on the website", localizedQuestion);
        }

        [Fact]
        public void ToLocalizedStringOnWebsiteContainsSubmissionsReviewDetailsKeyReturnsCorrectQuestionString()
        {
            // Arrange
            var question = new Question { Key = QuestionKey.WebsiteContainsSubmissionsReviewDetails };

            // Act
            var localizedQuestion = question.ToLocalizedString();

            // Assert
            Assert.Equal("The website indicates whether all submissions are sent out for review", localizedQuestion);
        }

        [Fact]
        public void ToLocalizedStringOnWebsiteContainsRoleOfMembersEditorialBoardKeyReturnsCorrectQuestionString()
        {
            // Arrange
            var question = new Question { Key = QuestionKey.WebsiteContainsRoleOfMembersEditorialBoard };

            // Act
            var localizedQuestion = question.ToLocalizedString();

            // Assert
            Assert.Equal("The role of members of the editorial board is explicated on the website including who will make final decisions about article acceptance (e.g., editor, associate/action editor)", localizedQuestion);
        }

        [Fact]
        public void ToLocalizedStringOnWebsiteContainsCriteriaUsedByReviewersKeyReturnsCorrectQuestionString()
        {
            // Arrange
            var question = new Question { Key = QuestionKey.WebsiteContainsCriteriaUsedByReviewers };

            // Act
            var localizedQuestion = question.ToLocalizedString();

            // Assert
            Assert.Equal("Criteria used by reviewers to rate submissions are specified on the website", localizedQuestion);
        }

        [Fact]
        public void ToLocalizedStringOnReviewsCommentsArePublishedKeyReturnsCorrectQuestionString()
        {
            // Arrange
            var question = new Question { Key = QuestionKey.ReviewsCommentsArePublished };

            // Act
            var localizedQuestion = question.ToLocalizedString();

            // Assert
            Assert.Equal("Editorial correspondence and reviewer's comments are published alongside papers", localizedQuestion);
        }

        [Fact]
        public void ToLocalizedStringOnAuthorsAllowedToIndicateDesiredReviewersKeyReturnsCorrectQuestionString()
        {
            // Arrange
            var question = new Question { Key = QuestionKey.AuthorsAllowedToIndicateDesiredReviewers };

            // Act
            var localizedQuestion = question.ToLocalizedString();

            // Assert
            Assert.Equal("The website indicates if authors have a say in suggesting names of (non-)desired reviewers", localizedQuestion);
        }

        [Fact]
        public void ToLocalizedStringOnWebsiteAllowsRatingsAndCommentariesOfPapersKeyReturnsCorrectQuestionString()
        {
            // Arrange
            var question = new Question { Key = QuestionKey.WebsiteAllowsRatingsAndCommentariesOfPapers };

            // Act
            var localizedQuestion = question.ToLocalizedString();

            // Assert
            Assert.Equal("The journal website allows ratings of papers and post-publication commentaries by the community", localizedQuestion);
        }

        [Fact]
        public void ToLocalizedStringOnHasClearGuidelinesConcerningSharingKeyReturnsCorrectQuestionString()
        {
            // Arrange
            var question = new Question { Key = QuestionKey.HasClearGuidelinesConcerningSharing };

            // Act
            var localizedQuestion = question.ToLocalizedString();

            // Assert
            Assert.Equal("The journal (publisher) has clear guidelines concerning sharing and availability of data for verification purposes", localizedQuestion);
        }

        [Fact]
        public void ToLocalizedStringOnCopiesAreMadeInThirdPartyRepositoriesKeyReturnsCorrectQuestionString()
        {
            // Arrange
            var question = new Question { Key = QuestionKey.CopiesAreMadeInThirdPartyRepositories };

            // Act
            var localizedQuestion = question.ToLocalizedString();

            // Assert
            Assert.Equal("The journal (publisher) makes copies of published articles available in trusted third-party repositories (e.g. PubMed Central) immediately upon publication", localizedQuestion);
        }

        [Fact]
        public void ToLocalizedStringOnHighlightsIssuesOfPublicationEthicsKeyReturnsCorrectQuestionString()
        {
            // Arrange
            var question = new Question { Key = QuestionKey.HighlightsIssuesOfPublicationEthics };

            // Act
            var localizedQuestion = question.ToLocalizedString();

            // Assert
            Assert.Equal("Journal’s website highlights issues of publication ethics (e.g. plagiarism, retraction policy), conflicts of interest, and (if applicable) codes of conduct for research in life sciences and social sciences", localizedQuestion);
        }

        [Fact]
        public void ToLocalizedStringOnProvidesTrackAndTraceServiceKeyReturnsCorrectQuestionString()
        {
            // Arrange
            var question = new Question { Key = QuestionKey.ProvidesTrackAndTraceService };

            // Act
            var localizedQuestion = question.ToLocalizedString();

            // Assert
            Assert.Equal("The website provides a track & trace service enabling authors to follow the status of their submission (e.g. under review)", localizedQuestion);
        }

        [Fact]
        public void ToLocalizedStringOnPublishedPapersIncludeInformationOnDatesOfSubmissionAndAcceptanceKeyReturnsCorrectQuestionString()
        {
            // Arrange
            var question = new Question { Key = QuestionKey.PublishedPapersIncludeInformationOnDatesOfSubmissionAndAcceptance };

            // Act
            var localizedQuestion = question.ToLocalizedString();

            // Assert
            Assert.Equal("Published papers include information on dates of original submission and acceptance", localizedQuestion);
        }

        [Fact]
        public void ToLocalizedStringOnDisclosesNumberOfSubmissionsPublicationsAndRejectionRatesKeyReturnsCorrectQuestionString()
        {
            // Arrange
            var question = new Question { Key = QuestionKey.DisclosesNumberOfSubmissionsPublicationsAndRejectionRates };

            // Act
            var localizedQuestion = question.ToLocalizedString();

            // Assert
            Assert.Equal("The journal discloses the past (yearly) number of submissions, publications, and rejection rates", localizedQuestion);
        }

        [Fact]
        public void ToLocalizedStringOnPeerReviewProcessTransparentKeyReturnsCorrectQuestionString()
        {
            // Arrange
            var question = new Question { Key = QuestionKey.PeerReviewProcessTransparent };

            // Act
            var localizedQuestion = question.ToLocalizedString();

            // Assert
            Assert.Equal("I would consider the peer-review process in this journal to be transparent", localizedQuestion);
        }

        [Fact]
        public void ToLocalizedStringOnRecommendScholarsToSubmitKeyReturnsCorrectQuestionString()
        {
            // Arrange
            var question = new Question { Key = QuestionKey.RecommendScholarsToSubmit };

            // Act
            var localizedQuestion = question.ToLocalizedString();

            // Assert
            Assert.Equal("I would recommend scholars to submit their work to this journal", localizedQuestion);
        }

        [Fact]
        public void ToLocalizedStringOnGoodValueForMoneyKeyReturnsCorrectQuestionString()
        {
            // Arrange
            var question = new Question { Key = QuestionKey.GoodValueForMoney };

            // Act
            var localizedQuestion = question.ToLocalizedString();

            // Assert
            Assert.Equal("I would deem this journal good value for money", localizedQuestion);
        }

        [Fact]
        public void ToLocalizedStringOnTakesCareOfInclusionInRelevantIndexingServicesKeyReturnsCorrectQuestionString()
        {
            // Arrange
            var question = new Question { Key = QuestionKey.TakesCareOfInclusionInRelevantIndexingServices };

            // Act
            var localizedQuestion = question.ToLocalizedString();

            // Assert
            Assert.Equal("The journal (publisher) takes care of inclusion of its articles in relevant indexing services", localizedQuestion);
        }

        [Fact]
        public void ToLocalizedStringOnProvidesArticlesWithDigitalObjectIdentifierKeyReturnsCorrectQuestionString()
        {
            // Arrange
            var question = new Question { Key = QuestionKey.ProvidesArticlesWithDigitalObjectIdentifier };

            // Act
            var localizedQuestion = question.ToLocalizedString();

            // Assert
            Assert.Equal("The journal (publisher) provides the articles with a Digital Object Identifier", localizedQuestion);
        }
    }
}