namespace QOAM.Website.ViewModels.Score
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using QOAM.Core;

    public class BaseScoreCardViewModel : IValidatableObject
    {
        public int Id { get; set; }
        public string Remarks { get; set; }
        public bool Submitted { get; set; }
        public bool Editor { get; set; }
        public bool Publish { get; set; }
        public IList<QuestionScoreViewModel> QuestionScores { get; set; }
        public JournalViewModel Journal { get; set; }
        public JournalPriceViewModel Price { get; set; }
        public IEnumerable<KeyValuePair<Currency, string>> Currencies { get; set; }
        public ScoreCardState State { get; set; }

        public void UpdateScoreCard(BaseScoreCard scoreCard, TimeSpan scoreCardLifeTime)
        {
            foreach (var questionScore in scoreCard.QuestionScores)
            {
                questionScore.Score = this.QuestionScores.First(q => q.Id == questionScore.Id).Score;
            }

            scoreCard.Score = new BaseScoreCardScore(scoreCard.QuestionScores);
            scoreCard.Remarks = this.Remarks;
            scoreCard.Submitted = this.Submitted;
            scoreCard.Editor = this.Editor;
            
            if (this.Publish)
            {
                scoreCard.State = ScoreCardState.Published;
                scoreCard.DatePublished = DateTime.Now;
                scoreCard.DateExpiration = DateTime.Now + scoreCardLifeTime;
            }
        }

        public void UpdateJournalPrice(BaseJournalPrice journalPrice)
        {
            var hasPrice = this.Price.FeeType == FeeType.Article || this.Price.FeeType == FeeType.Page;

            journalPrice.Price.Amount = hasPrice ? this.Price.Amount : 0;
            journalPrice.Price.Currency = hasPrice ? this.Price.Currency : null;
            journalPrice.Price.FeeType = this.Price.FeeType;
            journalPrice.DateAdded = DateTime.Now;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (QuestionScores == null || QuestionScores.Count <= 0)
            {
                return new[] { new ValidationResult("Not all questions have been scored.", new[] { nameof(QuestionScores) }) };
            }

            if (QuestionScores.Any(q => q.Score == Score.Undecided))
            {
                return new[] { new ValidationResult("Not all questions have been scored.", new[] { nameof(QuestionScores) }) };
            }

            return Enumerable.Empty<ValidationResult>();
        }
    }
}