namespace RU.Uci.OAMarket.Domain.Repositories.Filters
{
    using System.ComponentModel.DataAnnotations;

    public enum InstitutionSortMode
    {
        [Display(Name = "InstitutionSortMode_Name", ResourceType = typeof(Strings))]
        Name,

        [Display(Name = "InstitutionSortMode_NumberOfJournalScoreCards", ResourceType = typeof(Strings))]
        NumberOfJournalScoreCards
    }
}