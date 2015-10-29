namespace QOAM.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Validation;

    public class ValuationScoreCardScore
    {
        public ValuationScoreCardScore()
        {
            this.ValuationScore = new ScoreCardCategoryScore();
        }

        public ValuationScoreCardScore(ICollection<ValuationQuestionScore> questionScores)
        {
            Requires.NotNull(questionScores, nameof(questionScores));

            this.ValuationScore = CalculateScoreForCategory(questionScores, QuestionCategory.Valuation);
        }

        public ScoreCardCategoryScore ValuationScore { get; set; }

        public ScoreCardCategoryScore this[QuestionCategory questionCategory]
        {
            get
            {
                switch (questionCategory)
                {
                    case QuestionCategory.Valuation:
                        return this.ValuationScore;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(questionCategory));
                }
            }
        }

        private static ScoreCardCategoryScore CalculateScoreForCategory(IEnumerable<ValuationQuestionScore> questionScores, QuestionCategory questionCategory)
        {
            var categoryScores = questionScores.Where(q => q.Question.Category == questionCategory).ToList();

            return new ScoreCardCategoryScore
            {
                TotalScore = categoryScores.Sum(c => (int)c.Score),
                AverageScore = categoryScores.Sum(c => (float)c.Score) / categoryScores.Count()
            };
        }

    }
}