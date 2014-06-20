namespace QOAM.Website.Controllers
{
    using System.Web.Mvc;

    using AttributeRouting;
    using AttributeRouting.Web.Mvc;

    using QOAM.Core.Repositories;
    using QOAM.Website.Helpers;
    using QOAM.Website.Models;

    [RequireHttps]
    [RoutePrefix("account")]
    public class AccountController : ApplicationController
    {
        public AccountController(IUserProfileRepository userProfileRepository, IAuthentication authentication)
            : base(userProfileRepository, authentication)
        {
        }

        [GET("login/callback")]
        public ActionResult LoginCallback(string returnUrl)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("LoginFailure", new { reason = LoginFailureReason.ExternalAuthenticationFailed });
            }
            
            return this.RedirectToLocal(returnUrl);
        }

        [GET("login/failure")]
        public ViewResult LoginFailure(LoginFailureReason reason = LoginFailureReason.ExternalAuthenticationFailed)
        {
            return this.View(reason);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("Index", "Home");
        }
    }
}