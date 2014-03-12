namespace RU.Uci.OAMarket.Domain.Repositories.Filters
{
    using System.ComponentModel.DataAnnotations;

    public enum UserProfileSortMode
    {
        [Display(Name = "UserProfileSortMode_Name", ResourceType = typeof(Strings))]
        Name,

        [Display(Name = "UserProfileSortMode_DateRegistered", ResourceType = typeof(Strings))]
        DateRegistered,
        
        [Display(Name = "UserProfileSortMode_Institution", ResourceType = typeof(Strings))]
        Institution,

        [Display(Name = "UserProfileSortMode_NumberOfJournalScoreCards", ResourceType = typeof(Strings))]
        NumberOfJournalScoreCards
    }
}