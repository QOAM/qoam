namespace RU.Uci.OAMarket.Website.Models
{
    using System.ComponentModel.DataAnnotations;

    using RU.Uci.OAMarket.Website.Resources;

    public enum LoginFailureReason
    {
        [Display(ResourceType = typeof(Errors), Name = "LoginFailureReason_ExternalAuthenticationFailed")]
        ExternalAuthenticationFailed,

        [Display(ResourceType = typeof(Errors), Name = "LoginFailureReason_UsernameAlreadyExists")]
        UsernameAlreadyExists,

        [Display(ResourceType = typeof(Errors), Name = "LoginFailureReason_SaveFailed")]
        SaveFailed,
    }
}