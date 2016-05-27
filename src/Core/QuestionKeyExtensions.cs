namespace QOAM.Core
{
    public static class QuestionKeyExtensions
    {
        public static bool IsBaseScoreCardQuestion(this QuestionKey questionKey)
        {
            switch (questionKey)
            {
                case QuestionKey.PeerReviewProcessTransparent:
                case QuestionKey.PeerReviewHasAddedValue:
                case QuestionKey.EditorIsResponsive:
                case QuestionKey.RecommendScholarsToSubmit:
                case QuestionKey.GoodValueForMoney:
                    return false;
                default:
                    return true;
            }
        }

        public static bool IsValuationScoreCardQuestion(this QuestionKey questionKey)
        {
            return !questionKey.IsBaseScoreCardQuestion();
        }

        public static bool IsOutdated(this QuestionKey questionKey)
        {
            return questionKey == QuestionKey.PeerReviewProcessTransparent;
        }

    }
}