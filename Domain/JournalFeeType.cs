namespace RU.Uci.OAMarket.Domain
{
    using System.ComponentModel.DataAnnotations;

    public enum FeeType
    {
        [Display(Name = "FeeType_Article", ResourceType = typeof(Strings))]
        Article,

        [Display(Name = "FeeType_Page", ResourceType = typeof(Strings))]
        Page,

        [Display(Name = "FeeType_NoFee", ResourceType = typeof(Strings))]
        NoFee,
        
        [Display(Name = "FeeType_Absent", ResourceType = typeof(Strings))]
        Absent
    }
}