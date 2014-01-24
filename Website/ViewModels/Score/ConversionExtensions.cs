namespace RU.Uci.OAMarket.Website.ViewModels.Score
{
    using System.Linq;

    using RU.Uci.OAMarket.Domain;

    using Validation;

    public static class ConversionExtensions
    {
        public static ScoreViewModel ToViewModel(this ScoreCard scoreCard)
        {
            Requires.NotNull(scoreCard, "scoreCard");

            return new ScoreViewModel
                       {
                           Id = scoreCard.Id,
                           Remarks = scoreCard.Remarks,
                           Submitted = scoreCard.Submitted,
                           Editor = scoreCard.Editor,
                           Journal = scoreCard.Journal.ToViewModel(),
                           QuestionScores = scoreCard.QuestionScores.Select(ToViewModel).ToList()
                       };
        }

        public static JournalViewModel ToViewModel(this Journal journal)
        {
            Requires.NotNull(journal, "journal");

            return new JournalViewModel
            {
                Id = journal.Id,
                Title = journal.Title,
                ISSN = journal.ISSN,
                Link = journal.Link,
                Publisher = journal.Publisher.Name,
                Languages = journal.Languages.Select(l => l.Name),
                Subjects = journal.Subjects.Select(s => s.Name),
            };
        }

        public static JournalPriceViewModel ToViewModel(this JournalPrice journalPrice)
        {
            if (journalPrice == null)
            {
                return new JournalPriceViewModel();
            }

            return new JournalPriceViewModel
            {
                JournalPriceId = journalPrice.Id,
                Amount = journalPrice.Price.Amount,
                Currency = journalPrice.Price.Currency,
                FeeType = journalPrice.Price.FeeType
            };
        }

        public static QuestionScoreViewModel ToViewModel(this QuestionScore questionScore)
        {
            Requires.NotNull(questionScore, "questionScore");

            return new QuestionScoreViewModel
                       {
                           Id = questionScore.Id,
                           QuestionId = questionScore.QuestionId,
                           QuestionKey = questionScore.Question.Key,
                           QuestionCategory = questionScore.Question.Category,
                           ScoreCardId = questionScore.ScoreCardId,
                           Score = questionScore.Score
                       };
        }
    }
}