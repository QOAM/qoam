namespace QOAM.Core.Repositories
{
    using System;
    using System.Collections.Generic;

    using PagedList;

    using QOAM.Core.Repositories.Filters;

    public interface IValuationScoreCardRepository
    {
        ValuationScoreCard Find(int id);
        ValuationScoreCard Find(int journalId, int userProfileId);
        IPagedList<ValuationScoreCard> Find(ScoreCardFilter filter);
        void InsertOrUpdate(ValuationScoreCard scoreCard);
        void Save();
        IPagedList<ValuationScoreCard> FindForUser(ScoreCardFilter filter);
        ScoreCardStats CalculateStats(UserProfile userProfile);
        ScoreCardStats CalculateStats(Institution institution);
        IList<ValuationScoreCard> FindScoreCardsToBeArchived();
        IList<ValuationScoreCard> FindScoreCardsThatWillBeArchived(TimeSpan toBeArchivedWindow);
    }
}