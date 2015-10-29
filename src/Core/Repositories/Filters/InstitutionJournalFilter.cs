namespace QOAM.Core.Repositories.Filters
{
    public class InstitutionJournalFilter
    {
        public int? JournalId { get; set; }
        public int? InstitutionId { get; set; }
        public int? UserProfileId { get; set; }
        public int? PublisherId { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}