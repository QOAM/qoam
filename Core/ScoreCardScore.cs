namespace QOAM.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Validation;

    public class ScoreCardScore
    {
        public ScoreCardScore()
        {
            this.EditorialInformationScore = new ScoreCardCategoryScore();
            this.PeerReviewScore = new ScoreCardCategoryScore();
            this.GovernanceScore = new ScoreCardCategoryScore();
            this.ProcessScore = new ScoreCardCategoryScore();
            this.ValuationScore = new ScoreCardCategoryScore();
            this.OverallScore = new ScoreCardCategoryScore();
        }

        public ScoreCardScore(ICollection<QuestionScore> questionScores)
        {
            Requires.NotNull(questionScores, "questionScores");

            this.EditorialInformationScore = CalculateScoreForCategory(questionScores, QuestionCategory.EditorialInformation);
            this.PeerReviewScore = CalculateScoreForCategory(questionScores, QuestionCategory.PeerReview);
            this.GovernanceScore = CalculateScoreForCategory(questionScores, QuestionCategory.Governance);
            this.ProcessScore = CalculateScoreForCategory(questionScores, QuestionCategory.Process);
            this.ValuationScore = CalculateScoreForCategory(questionScores, QuestionCategory.Valuation);
            this.OverallScore = this.CalculateOverallScore();
        }

        public ScoreCardCategoryScore OverallScore { get; set; }
        public ScoreCardCategoryScore EditorialInformationScore { get; set; }
        public ScoreCardCategoryScore PeerReviewScore { get; set; }
        public ScoreCardCategoryScore GovernanceScore { get; set; }
        public ScoreCardCategoryScore ProcessScore { get; set; }
        public ScoreCardCategoryScore ValuationScore { get; set; }

        public ScoreCardCategoryScore this[QuestionCategory questionCategory]
        {
            get
            {
                switch (questionCategory)
                {
                    case QuestionCategory.EditorialInformation:
                        return this.EditorialInformationScore;
                    case QuestionCategory.PeerReview:
                        return this.PeerReviewScore;
                    case QuestionCategory.Governance:
                        return this.GovernanceScore;
                    case QuestionCategory.Process:
                        return this.ProcessScore;
                    case QuestionCategory.Valuation:
                        return this.ValuationScore;
                    default:
                        throw new ArgumentOutOfRangeException("questionCategory");
                }
            }
        }
        
        private static ScoreCardCategoryScore CalculateScoreForCategory(IEnumerable<QuestionScore> questionScores, QuestionCategory questionCategory)
        {
            var categoryScores = questionScores.Where(q => q.Question.Category == questionCategory).ToList();

            return new ScoreCardCategoryScore
            {
                TotalScore = categoryScores.Sum(c => (int)c.Score),
                AverageScore = categoryScores.Sum(c => (float)c.Score) / categoryScores.Count()
            };
        }

        private ScoreCardCategoryScore CalculateOverallScore()
        {
            var scoreCardCategoryScore = new[] { this.EditorialInformationScore, this.PeerReviewScore, this.GovernanceScore, this.ProcessScore }.OrderBy(s => s.AverageScore).First();
            
            return new ScoreCardCategoryScore
                       {
                           TotalScore = scoreCardCategoryScore.TotalScore,
                           AverageScore = scoreCardCategoryScore.AverageScore
                       };
        }
    }
}