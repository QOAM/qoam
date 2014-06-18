namespace QOAM.Website.Controllers
{
    using System;
    using System.Web;
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
        private readonly HttpSessionStateBase session;

        public AccountController(IInstitutionRepository institutionRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication, HttpSessionStateBase session)
            : base(userProfileRepository, authentication)
        {
            Requires.NotNull(institutionRepository, "institutionRepository");

            this.institutionRepository = institutionRepository;
            this.session = session;
        }

        [GET("login/callback")]
        public ActionResult LoginCallback(string returnUrl)
        {
            if (this.session == null)
            {
                return this.RedirectToAction("LoginFailure", new { reason = LoginFailureReason.ExternalAuthenticationFailed });
            }

            var saml20Identity = this.session[typeof(Saml20Identity).FullName] as ISaml20Identity;

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
                this.institutionRepository.InsertOrUpdate(institution);
            }

            var user = this.UserProfileRepository.Find(saml20Identity.Name);

            if (user != null)
            {
                return this.RedirectToAction("LoginFailure", new { reason = LoginFailureReason.UsernameAlreadyExists });
            }

            this.UserProfileRepository.InsertOrUpdate(new UserProfile
                                                  {
                                                      UserName = saml20Identity.Name,
                                                      DisplayName = saml20Identity.GetDisplayName(),
                                                      Email = saml20Identity.GetEmail(),
                                                      DateRegistered = DateTime.Now,
                                                      Institution = institution
                                                  });

            try
            {
                this.UserProfileRepository.Save();

                this.Authentication.CreateOrUpdateAccount(saml20Identity.GetProvider(), saml20Identity.GetProviderUserId(), saml20Identity.Name);
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