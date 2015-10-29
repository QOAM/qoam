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
        void Delete(ValuationScoreCard entity);
        void Save();
        IPagedList<ValuationScoreCard> FindForUser(ScoreCardFilter filter);
        ScoreCardStats CalculateStats(UserProfile userProfile);
        ScoreCardStats CalculateStats(Institution institution);
        IList<ValuationScoreCard> FindScoreCardsToBeArchived();
        IList<ValuationScoreCard> FindScoreCardsThatWillBeArchived(TimeSpan toBeArchivedWindow);
        void MoveScoreCards(Journal oldJournal, Journal newJournal);
        int Count(ScoreCardFilter filter);
        DateTime? LastUpdate();
        void RemoveUnpublishedScoreCards(TimeSpan toBeRemovedWindow);
        void ArchiveDuplicateScoreCards();
    }
}