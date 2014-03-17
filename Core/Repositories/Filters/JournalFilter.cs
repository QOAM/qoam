namespace QOAM.Core.Repositories.Filters
{
    using System.Web.Helpers;

    public class JournalFilter
    {
        public string Title { get; set; }
        public string Issn { get; set; }
        public string Publisher { get; set; }
        public int? PublisherId { get; set; }
        public int? Discipline { get; set; }
        public int? Language { get; set; }
        public float? MinimumBaseScore { get; set; }
        public float? MinimumValuationScore { get; set; }
        public bool MustHaveBeenScored { get; set; }
        public JournalSortMode SortMode { get; set; }
        public SortDirection SortDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool SubmittedOnly { get; set; }
    }
}