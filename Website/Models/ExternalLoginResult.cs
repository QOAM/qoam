namespace RU.Uci.OAMarket.Website.Models
{
    using System.Web.Mvc;

    using Microsoft.Web.WebPages.OAuth;

    public class ExternalLoginResult : ActionResult
    {
        public ExternalLoginResult(string provider, string returnUrl)
        {
            this.Provider = provider;
            this.ReturnUrl = returnUrl;
        }

        public string Provider { get; private set; }
        public string ReturnUrl { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            OAuthWebSecurity.RequestAuthentication(this.Provider, this.ReturnUrl);
        }
    }
}