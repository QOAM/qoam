namespace QOAM.Website.ViewModels.Score
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using QOAM.Core;

    public class ValuationScoreCardViewModel
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

        public bool HasPrice
        {
            get
            {
                return this.Submitted && this.Price.Amount.HasValue;
            }
        }

        public void UpdateScoreCard(ValuationScoreCard scoreCard, TimeSpan scoreCardLifeTime)
        {
            foreach (var questionScore in scoreCard.QuestionScores)
            {
                questionScore.Score = this.QuestionScores.First(q => q.Id == questionScore.Id).Score;
            }

            scoreCard.Score = new ValuationScoreCardScore(scoreCard.QuestionScores);
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

        public void UpdateJournalPrice(ValuationJournalPrice journalPrice)
        {
            journalPrice.Price.Amount = this.HasPrice ? this.Price.Amount : 0;
            journalPrice.Price.Currency = this.HasPrice ? this.Price.Currency : null;
            journalPrice.Price.FeeType = this.HasPrice ? FeeType.Article : FeeType.Absent;
            journalPrice.DateAdded = DateTime.Now;
        }
    }
}