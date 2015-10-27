namespace QOAM.Core.Repositories
{
    using System;
    using System.Collections.Generic;

    using PagedList;

    using QOAM.Core.Repositories.Filters;

    public interface IBaseScoreCardRepository
    {
        BaseScoreCard Find(int id);
        BaseScoreCard Find(int journalId, int userProfileId);
        IPagedList<BaseScoreCard> Find(ScoreCardFilter filter);
        void InsertOrUpdate(BaseScoreCard scoreCard);
        void Delete(BaseScoreCard entity);
        void Save();
        IPagedList<BaseScoreCard> FindForUser(ScoreCardFilter filter);
        ScoreCardStats CalculateStats(UserProfile userProfile);
        ScoreCardStats CalculateStats(Institution institution);
        IList<BaseScoreCard> FindScoreCardsToBeArchived();
        IList<BaseScoreCard> FindScoreCardsThatWillBeArchived(TimeSpan toBeArchivedWindow);
        void MoveScoreCards(Journal oldJournal, Journal newJournal);
        int Count(ScoreCardFilter filter);
    }
}