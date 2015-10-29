namespace QOAM.Core.Repositories.Filters
{
    using System.ComponentModel.DataAnnotations;

    public enum InstitutionSortMode
    {
        [Display(Name = "InstitutionSortMode_Name", ResourceType = typeof(Strings))]
        Name,

        [Display(Name = "InstitutionSortMode_NumberOfBaseJournalScoreCards", ResourceType = typeof(Strings))]
        NumberOfBaseJournalScoreCards,

        [Display(Name = "InstitutionSortMode_NumberOfValuationJournalScoreCards", ResourceType = typeof(Strings))]
        NumberOfValuationJournalScoreCards
    }
}