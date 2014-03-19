namespace QOAM.Website.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using AttributeRouting;
    using AttributeRouting.Web.Mvc;

    using DotNetOpenAuth.AspNet;

    using QOAM.Core;
    using QOAM.Core.Repositories;
    using QOAM.Website.Helpers;
    using QOAM.Website.Models;

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
            this.ViewBag.SurfContextClientData = this.Authentication.RegisteredClientData.First(c => c.AuthenticationClient.GetType() == typeof(SurfConextClient));

            return this.View();
        }

        [POST("logoff")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult LogOff()
        {
            this.Authentication.Logout();

            return this.RedirectToAction("Index", "Home");
        }

        [POST("externallogin")]
        [ValidateAntiForgeryToken]
        public ExternalLoginResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, this.Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        [GET("externallogincallback")]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result;

            try
            {
                result = this.Authentication.VerifyAuthentication(this.Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            }
            catch (Exception)
            {
                return this.RedirectToAction("ExternalLoginFailure", new { reason = LoginFailureReason.ExternalAuthenticationFailed });
            }

            if (!result.IsSuccessful)
            {
                return this.RedirectToAction("ExternalLoginFailure", new { reason = LoginFailureReason.ExternalAuthenticationFailed });
            }
            
            if (this.Authentication.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return this.RedirectToLocal(returnUrl);
            }

            var institution = this.institutionRepository.Find(result.GetInstitutionShortName());

            if (institution == null)
            {
                institution = new Institution { Name = result.GetInstitutionShortName(), ShortName = result.GetInstitutionShortName() };
                this.institutionRepository.Insert(institution);
            }

            var user = this.UserProfileRepository.Find(result.GetUsername());

            if (user != null)
            {
                return this.RedirectToAction("ExternalLoginFailure", new { reason = LoginFailureReason.UsernameAlreadyExists });
            }

            this.UserProfileRepository.Insert(new UserProfile
                                                  {
                                                      UserName = result.GetUsername(),
                                                      DisplayName = result.GetDisplayName(),
                                                      Email = result.GetEmail(),
                                                      DateRegistered = DateTime.Now,
                                                      Institution = institution
                                                  });

            try
            {
                this.UserProfileRepository.Save();

                this.Authentication.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, result.GetUsername());
                this.Authentication.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false);

                return this.RedirectToLocal(returnUrl);
            }
            catch
            {
                return this.RedirectToAction("ExternalLoginFailure", new { reason = LoginFailureReason.SaveFailed });
            }
        }

        [GET("externalloginfailure")]
        public ViewResult ExternalLoginFailure(LoginFailureReason reason)
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