namespace RU.Uci.OAMarket.Domain
{
    using System.ComponentModel.DataAnnotations;

    public enum Currency
    {
        [Display(Name = "Currency_Dollar", ResourceType = typeof(Strings))]
        Dollar,

        [Display(Name = "Currency_Euro", ResourceType = typeof(Strings))]
        Euro,

        [Display(Name = "Currency_Pound", ResourceType = typeof(Strings))]
        Pound
    }
}