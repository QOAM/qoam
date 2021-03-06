﻿namespace QOAM.Core.Tests
{
    using Xunit;

    public class QuestionKeyExtensionsTests
    {
        [Theory]
        [InlineData(QuestionKey.WebsiteContainsAimsScopeAndReadership)]
        [InlineData(QuestionKey.WebsiteContainsNameAndAffiliationsOfMembersEditorialBoard)]
        [InlineData(QuestionKey.WebsiteContainsSubmissionsReviewDetails)]
        [InlineData(QuestionKey.WebsiteContainsRoleOfMembersEditorialBoard)]
        [InlineData(QuestionKey.WebsiteContainsCriteriaUsedByReviewers)]
        [InlineData(QuestionKey.ReviewsCommentsArePublished)]
        [InlineData(QuestionKey.AuthorsAllowedToIndicateDesiredReviewers)]
        [InlineData(QuestionKey.WebsiteAllowsRatingsAndCommentariesOfPapers)]
        [InlineData(QuestionKey.HasClearGuidelinesConcerningSharing)]
        [InlineData(QuestionKey.CopiesAreMadeInThirdPartyRepositories)]
        [InlineData(QuestionKey.TakesCareOfInclusionInRelevantIndexingServices)]
        [InlineData(QuestionKey.HighlightsIssuesOfPublicationEthics)]
        [InlineData(QuestionKey.ProvidesTrackAndTraceService)]
        [InlineData(QuestionKey.PublishedPapersIncludeInformationOnDatesOfSubmissionAndAcceptance)]
        [InlineData(QuestionKey.ProvidesArticlesWithDigitalObjectIdentifier)]
        [InlineData(QuestionKey.DisclosesNumberOfSubmissionsPublicationsAndRejectionRates)]
        public void IsBaseScoreCardQuestionOnBaseScoreCardQuestionReturnsTrue(QuestionKey baseScoreCardQuestionKey)
        {
            // Arrange

            // Act
            var isBaseScoreCardQuestion = baseScoreCardQuestionKey.IsBaseScoreCardQuestion();

            // Assert
            Assert.True(isBaseScoreCardQuestion);
        }

        [Theory]
        [InlineData(QuestionKey.PeerReviewHasAddedValue)]
        [InlineData(QuestionKey.EditorIsResponsive)]
        [InlineData(QuestionKey.RecommendScholarsToSubmit)]
        [InlineData(QuestionKey.GoodValueForMoney)]
        public void IsBaseScoreCardQuestionOnValuationScoreCardQuestionReturnsFalse(QuestionKey valuationScoreCardQuestionKey)
        {
            // Arrange

            // Act
            var isBaseScoreCardQuestion = valuationScoreCardQuestionKey.IsBaseScoreCardQuestion();

            // Assert
            Assert.False(isBaseScoreCardQuestion);
        }


        [Theory]
        [InlineData(QuestionKey.WebsiteContainsAimsScopeAndReadership)]
        [InlineData(QuestionKey.WebsiteContainsNameAndAffiliationsOfMembersEditorialBoard)]
        [InlineData(QuestionKey.WebsiteContainsSubmissionsReviewDetails)]
        [InlineData(QuestionKey.WebsiteContainsRoleOfMembersEditorialBoard)]
        [InlineData(QuestionKey.WebsiteContainsCriteriaUsedByReviewers)]
        [InlineData(QuestionKey.ReviewsCommentsArePublished)]
        [InlineData(QuestionKey.AuthorsAllowedToIndicateDesiredReviewers)]
        [InlineData(QuestionKey.WebsiteAllowsRatingsAndCommentariesOfPapers)]
        [InlineData(QuestionKey.HasClearGuidelinesConcerningSharing)]
        [InlineData(QuestionKey.CopiesAreMadeInThirdPartyRepositories)]
        [InlineData(QuestionKey.TakesCareOfInclusionInRelevantIndexingServices)]
        [InlineData(QuestionKey.HighlightsIssuesOfPublicationEthics)]
        [InlineData(QuestionKey.ProvidesTrackAndTraceService)]
        [InlineData(QuestionKey.PublishedPapersIncludeInformationOnDatesOfSubmissionAndAcceptance)]
        [InlineData(QuestionKey.ProvidesArticlesWithDigitalObjectIdentifier)]
        [InlineData(QuestionKey.DisclosesNumberOfSubmissionsPublicationsAndRejectionRates)]
        public void IsValuationScoreCardQuestionOnBaseScoreCardQuestionReturnsFalse(QuestionKey baseScoreCardQuestionKey)
        {
            // Arrange

            // Act
            var isBaseScoreCardQuestion = baseScoreCardQuestionKey.IsValuationScoreCardQuestion();

            // Assert
            Assert.False(isBaseScoreCardQuestion);
        }

        [Theory]
        [InlineData(QuestionKey.EditorIsResponsive)]
        [InlineData(QuestionKey.PeerReviewHasAddedValue)]
        [InlineData(QuestionKey.RecommendScholarsToSubmit)]
        [InlineData(QuestionKey.GoodValueForMoney)]
        public void IsValuationScoreCardQuestionOnValuationScoreCardQuestionReturnsTrue(QuestionKey valuationScoreCardQuestionKey)
        {
            // Arrange

            // Act
            var isBaseScoreCardQuestion = valuationScoreCardQuestionKey.IsValuationScoreCardQuestion();

            // Assert
            Assert.True(isBaseScoreCardQuestion);
        }

        [Theory]
        [InlineData(QuestionKey.PeerReviewProcessTransparent)]
        public void IsQuestionOutdated(QuestionKey questionKey)
        {
            var isOutdated = questionKey.IsOutdated();

            Assert.True(isOutdated);
        }

        [Theory]
        [InlineData(QuestionKey.WebsiteContainsAimsScopeAndReadership)]
        [InlineData(QuestionKey.WebsiteContainsNameAndAffiliationsOfMembersEditorialBoard)]
        [InlineData(QuestionKey.WebsiteContainsSubmissionsReviewDetails)]
        [InlineData(QuestionKey.WebsiteContainsRoleOfMembersEditorialBoard)]
        [InlineData(QuestionKey.WebsiteContainsCriteriaUsedByReviewers)]
        [InlineData(QuestionKey.ReviewsCommentsArePublished)]
        [InlineData(QuestionKey.AuthorsAllowedToIndicateDesiredReviewers)]
        [InlineData(QuestionKey.WebsiteAllowsRatingsAndCommentariesOfPapers)]
        [InlineData(QuestionKey.HasClearGuidelinesConcerningSharing)]
        [InlineData(QuestionKey.CopiesAreMadeInThirdPartyRepositories)]
        [InlineData(QuestionKey.TakesCareOfInclusionInRelevantIndexingServices)]
        [InlineData(QuestionKey.HighlightsIssuesOfPublicationEthics)]
        [InlineData(QuestionKey.ProvidesTrackAndTraceService)]
        [InlineData(QuestionKey.PublishedPapersIncludeInformationOnDatesOfSubmissionAndAcceptance)]
        [InlineData(QuestionKey.ProvidesArticlesWithDigitalObjectIdentifier)]
        [InlineData(QuestionKey.DisclosesNumberOfSubmissionsPublicationsAndRejectionRates)]
        [InlineData(QuestionKey.EditorIsResponsive)]
        [InlineData(QuestionKey.PeerReviewHasAddedValue)]
        [InlineData(QuestionKey.RecommendScholarsToSubmit)]
        [InlineData(QuestionKey.GoodValueForMoney)]
        public void IsQuestionCurrent(QuestionKey questionKey)
        {
            var isOutdated = questionKey.IsOutdated();

            Assert.False(isOutdated);
        }
    }
}