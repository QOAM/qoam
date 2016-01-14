namespace QOAM.Core.Repositories.Filters
{
    public class UserJournalFilter : JournalFilter
    {
        public int? JournalId { get; set; }
        public int UserProfileId { get; set; }
    }
}