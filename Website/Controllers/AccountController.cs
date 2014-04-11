namespace QOAM.Website.Controllers
{
    using System;
    using System.Web.Mvc;

    using AttributeRouting;
    using AttributeRouting.Web.Mvc;

    using QOAM.Core;
    using QOAM.Core.Repositories;
    using QOAM.Website.Helpers;
    using QOAM.Website.Models;
    using QOAM.Website.Models.SAML;

    using SAML2.Identity;

    using Validation;
    
    [RequireHttps]
    [RoutePrefix("account")]
    public class AccountController : ApplicationController
    {
        private readonly IInstitutionRepository institutionRepository;

        public AccountController(IInstitutionRepository institutionRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication)
            : base(userProfileRepository, authentication)
        {
            Requires.NotNull(institutionRepository, "institutionRepository");

            this.institutionRepository = institutionRepository;
        }

        [GET("login")]
        public ViewResult Login(string returnUrl)
        {
            this.ViewBag.ReturnUrl = returnUrl;

            return this.View();
        }

        [GET("login/callback")]
        public ActionResult LoginCallback(string returnUrl)
        {
            var saml20Identity = HttpContext.Session[typeof(Saml20Identity).FullName] as Saml20Identity;

            if (saml20Identity == null || !saml20Identity.IsAuthenticated)
            {
                return this.RedirectToAction("LoginFailure", new { reason = LoginFailureReason.ExternalAuthenticationFailed });
            }
            
            if (this.Authentication.Login(saml20Identity.GetProvider(), saml20Identity.GetProviderUserId(), createPersistentCookie: false))
            {
                return this.RedirectToLocal(returnUrl);
            }

            var institution = this.institutionRepository.Find(saml20Identity.GetInstitutionShortName());

            if (institution == null)
            {
                institution = new Institution { Name = saml20Identity.GetInstitutionShortName(), ShortName = saml20Identity.GetInstitutionShortName() };
                this.institutionRepository.Insert(institution);
            }

            var user = this.UserProfileRepository.Find(saml20Identity.GetUserName());

            if (user != null)
            {
                return this.RedirectToAction("LoginFailure", new { reason = LoginFailureReason.UsernameAlreadyExists });
            }

            this.UserProfileRepository.Insert(new UserProfile
                                                  {
                                                      UserName = saml20Identity.GetUserName(),
                                                      DisplayName = saml20Identity.GetDisplayName(),
                                                      Email = saml20Identity.GetEmail(),
                                                      DateRegistered = DateTime.Now,
                                                      Institution = institution
                                                  });

            try
            {
                this.UserProfileRepository.Save();

                this.Authentication.CreateOrUpdateAccount(saml20Identity.GetProvider(), saml20Identity.GetProviderUserId(), saml20Identity.GetUserName());
                this.Authentication.Login(saml20Identity.GetProvider(), saml20Identity.GetProviderUserId(), createPersistentCookie: false);

                return this.RedirectToLocal(returnUrl);
            }
            catch
            {
                return this.RedirectToAction("LoginFailure", new { reason = LoginFailureReason.SaveFailed });
            }
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