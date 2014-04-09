namespace QOAM.Website.ViewModels.Score
{
    using System.Linq;

    using QOAM.Core;

    using Validation;

    public static class ConversionExtensions
    {
        public static BaseScoreCardViewModel ToBaseScoreCardViewModel(this BaseScoreCard scoreCard)
        {
            Requires.NotNull(scoreCard, "scoreCard");

            return new BaseScoreCardViewModel
                       {
                           Id = scoreCard.JournalId,
                           Remarks = scoreCard.Remarks,
                           Submitted = scoreCard.Submitted,
                           Editor = scoreCard.Editor,
                           Journal = scoreCard.Journal.ToViewModel(),
                           State = scoreCard.State,
                           QuestionScores = scoreCard.QuestionScores.Select(ToViewModel).ToList()
                       };
        }

        public static ValuationScoreCardViewModel ToValuationScoreCardViewModel(this ValuationScoreCard scoreCard)
        {
            Requires.NotNull(scoreCard, "scoreCard");

            return new ValuationScoreCardViewModel
            {
                Id = scoreCard.JournalId,
                Remarks = scoreCard.Remarks,
                Submitted = scoreCard.Submitted,
                Editor = scoreCard.Editor,
                Journal = scoreCard.Journal.ToViewModel(),
                State = scoreCard.State,
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

        public static JournalPriceViewModel ToViewModel(this BaseJournalPrice journalPrice)
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

        public static JournalPriceViewModel ToViewModel(this ValuationJournalPrice journalPrice)
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

        public static QuestionScoreViewModel ToViewModel(this BaseQuestionScore questionScore)
        {
            Requires.NotNull(questionScore, "questionScore");

            return new QuestionScoreViewModel
                       {
                           Id = questionScore.Id,
                           QuestionId = questionScore.QuestionId,
                           QuestionKey = questionScore.Question.Key,
                           QuestionCategory = questionScore.Question.Category,
                           ScoreCardId = questionScore.BaseScoreCardId,
                           Score = questionScore.Score
                       };
        }

        public static QuestionScoreViewModel ToViewModel(this ValuationQuestionScore questionScore)
        {
            Requires.NotNull(questionScore, "questionScore");

            return new QuestionScoreViewModel
            {
                Id = questionScore.Id,
                QuestionId = questionScore.QuestionId,
                QuestionKey = questionScore.Question.Key,
                QuestionCategory = questionScore.Question.Category,
                ScoreCardId = questionScore.ValuationScoreCardId,
                Score = questionScore.Score
            };
        }
    }
}