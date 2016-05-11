namespace QOAM.Core
{
    public enum QuestionKey
    {
        // Editorial info questions
        WebsiteContainsAimsScopeAndReadership,
        WebsiteContainsNameAndAffiliationsOfMembersEditorialBoard,
        WebsiteContainsSubmissionsReviewDetails,
        WebsiteContainsRoleOfMembersEditorialBoard,

        // Peer review questions
        WebsiteContainsCriteriaUsedByReviewers,
        ReviewsCommentsArePublished,
        AuthorsAllowedToIndicateDesiredReviewers,
        WebsiteAllowsRatingsAndCommentariesOfPapers,

        // Governance questions
        HasClearGuidelinesConcerningSharing,
        CopiesAreMadeInThirdPartyRepositories,
        TakesCareOfInclusionInRelevantIndexingServices,
        HighlightsIssuesOfPublicationEthics,

        // Process questions
        ProvidesTrackAndTraceService,
        PublishedPapersIncludeInformationOnDatesOfSubmissionAndAcceptance,
        ProvidesArticlesWithDigitalObjectIdentifier,
        DisclosesNumberOfSubmissionsPublicationsAndRejectionRates,

        // Valuation questions
        PeerReviewProcessTransparent,
        RecommendScholarsToSubmit,
        GoodValueForMoney,
        EditorIsResponsive,
        PeerReviewHasAddedValue,
    }
}
