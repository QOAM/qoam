namespace QOAM.Core.Repositories.Filters
{
    using System.ComponentModel.DataAnnotations;

    public enum JournalSortMode
    {
        [Display(Name = "JournalSortMode_ValuationScore", ResourceType = typeof(Strings))]
        ValuationScore,

        [Display(Name = "JournalSortMode_Name", ResourceType = typeof(Strings))]
        Name,

        [Display(Name = "JournalSortMode_RobustScores", ResourceType = typeof(Strings))]
        RobustScores
    }
}