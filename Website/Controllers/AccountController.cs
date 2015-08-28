namespace QOAM.Website.Controllers
{
    using System;
    using System.Linq;
    using System.Net.Mail;
    using System.Web.Mvc;
    using System.Web.Security;

    using AttributeRouting;
    using AttributeRouting.Web.Mvc;

    using Postal;

    using QOAM.Core.Repositories;
    using QOAM.Website.Helpers;
    using QOAM.Website.ViewModels.Account;
    using QOAM.Website.ViewModels.Score;

    using Validation;

    [RoutePrefix("")]
    [RequireHttps]
    public class AccountController : ApplicationController
    {
        private readonly IAuthentication authentication;

        private readonly IInstitutionRepository institutionRepository;

        private readonly IUserProfileRepository userProfileRepository;

        private readonly IJournalRepository journalRepository;

        public AccountController(IUserProfileRepository userProfileRepository, IAuthentication authentication, IInstitutionRepository institutionRepository, IJournalRepository journalRepository)
            : base(userProfileRepository, authentication)
        {
            Requires.NotNull(authentication, "authentication");
            Requires.NotNull(userProfileRepository, "userProfileRepository");
            Requires.NotNull(institutionRepository, "institutionRepository");
            Requires.NotNull(journalRepository, "journalRepository");

            this.authentication = authentication;
            this.userProfileRepository = userProfileRepository;
            this.institutionRepository = institutionRepository;
            this.journalRepository = journalRepository;
        }

        [GET("login")]
        public ViewResult Login(string returnUrl, string loginAddress)
        {
            this.ViewBag.ReturnUrl = returnUrl;

            if (string.IsNullOrEmpty(loginAddress))
            {
                return this.View();
            }

            var model = new LoginViewModel { Email = loginAddress, RememberMe = false };

            return this.View(model);
        }

        [POST("login")]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (this.ModelState.IsValid)
            {
                var userProfile = this.userProfileRepository.FindByEmail(model.Email);
                if (userProfile != null && this.authentication.Login(userProfile.UserName, model.Password, model.RememberMe))
                {
                    return this.RedirectToLocal(returnUrl);
                }
            }

            this.ModelState.AddModelError("", "The email address or password provided is incorrect.");

            return this.View(model);
        }

        [GET("logout")]
        public RedirectToRouteResult Logout()
        {
            this.authentication.Logout();

            return this.RedirectToAction("Index", "Home");
        }

        [GET("register")]
        public ActionResult Register(string addLink, string loginAddress)
        {
            if (string.IsNullOrEmpty(addLink) && string.IsNullOrEmpty(loginAddress))
            {
                return this.View();
            }

            var model = new RegisterViewModel { AddLink = addLink, Email = loginAddress };
            this.ViewBag.ShortRegistration = true;

            return this.View(model);
        }

        [POST("register")]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var emailExists = this.userProfileRepository.FindByEmail(model.Email);
            if (emailExists != null)
            {
                this.ModelState.AddModelError("", "Sorry, unable to register with this email address.");
                return this.View(model);
            }

            var mailAddress = new MailAddress(model.Email);
            var institution = this.institutionRepository.All.FirstOrDefault(i => mailAddress.Host.Contains(i.ShortName));
            if (institution == null)
            {
                this.ModelState.AddModelError("", "Sorry, the domain name in your email address does not match the name of an academic institution known to us. If you want your institution to be included in our list, please enter it’s name and web address in our contact box and we will respond promptly.");
                return this.View(model);
            }

            model.InstitutionId = institution.Id;
            model.DisplayName = model.UserName;
            model.DateRegistered = DateTime.Now;

            try
            {
                var confirmationToken = this.authentication.CreateUserAndAccount(model.UserName, model.Password, new { model.Email, model.DisplayName, model.DateRegistered, model.InstitutionId, model.OrcId });

                if (string.IsNullOrEmpty(model.AddLink))
                {
                    dynamic email = new Email("RegistrationEmail");
                    email.To = model.Email;
                    email.DisplayName = model.DisplayName;
                    email.Url = this.Url.Action("RegisterConfirmation", "Account", new { token = confirmationToken }, this.Request.Url.Scheme);
                    email.Send();

                    return this.RedirectToAction("RegistrationPending");
                }

                if (!this.Authentication.ConfirmAccount(confirmationToken))
                {
                    return this.RedirectToAction("RegisterFailure");
                }

                var loginViewModel = new LoginViewModel { Email = model.Email, Password = model.Password, RememberMe = false };

                this.Login(loginViewModel, model.AddLink);

                return this.RedirectToAction("RegisterSuccessWithLink", "Account", new { addLink = model.AddLink });
            }
            catch (MembershipCreateUserException)
            {
                this.ModelState.AddModelError("", "Can't create the user.");
            }

            return View(model);
        }

        [GET("registrationpending")]
        public ActionResult RegistrationPending()
        {
            return this.View();
        }

        [GET("registerconfirmation/{token}")]
        public ActionResult RegisterConfirmation(string token)
        {
            return this.RedirectToAction(!this.Authentication.ConfirmAccount(token) ? "RegisterFailure" : "RegisterSuccess");
        }

        [GET("registersuccess")]
        public ActionResult RegisterSuccess()
        {
            return this.View();
        }

        [GET("registersuccesswithlink")]
        public ActionResult RegisterSuccessWithLink(string addLink)
        {
            var model = new RequestValuationViewModel { JournalId = Convert.ToInt32(addLink.Split('/').Last()) };
            model.JournalTitle = this.journalRepository.Find(model.JournalId).Title;
            
            return this.View(model);
        }

        [GET("registerfailure")]
        public ActionResult RegisterFailure()
        {
            return this.View();
        }

        [GET("account/settings")]
        [Authorize]
        public ActionResult Settings(bool saveSuccessful = false)
        {
            var userProfile = this.userProfileRepository.Find(this.authentication.CurrentUserId);
            if (userProfile == null)
            {
                return this.HttpNotFound();
            }

            var model = new SettingsViewModel { DisplayName = userProfile.DisplayName, OrcId = userProfile.OrcId };
            this.ViewBag.SaveSuccessful = saveSuccessful;

            return this.View(model);
        }

        [POST("account/settings")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(SettingsViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var userProfile = this.userProfileRepository.Find(this.authentication.CurrentUserId);
                if (userProfile == null)
                {
                    return this.HttpNotFound();
                }

                userProfile.DisplayName = model.DisplayName;
                userProfile.OrcId = model.OrcId;

                this.userProfileRepository.InsertOrUpdate(userProfile);
                this.userProfileRepository.Save();

                return this.RedirectToAction("Settings", new { saveSuccessful = true });
            }

            return this.View(model);
        }

        [GET("changepassword")]
        [Authorize]
        public ActionResult ChangePassword(bool saveSuccessful = false)
        {
            this.ViewBag.SaveSuccessful = saveSuccessful;

            return this.View();
        }

        [POST("changepassword")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (this.ModelState.IsValid && this.authentication.ChangePassword(this.authentication.CurrentUserName, model.OldPassword, model.NewPassword))
            {
                return this.RedirectToAction("ChangePassword", new { saveSuccessful = true });
            }

            this.ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");

            return this.View(model);
        }

        [GET("resetpassword")]
        public ActionResult ResetPassword()
        {
            return this.View();
        }

        [POST("resetpassword")]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var userProfile = this.userProfileRepository.FindByEmail(model.Email);
            if (userProfile == null || !this.authentication.UserIsConfirmed(userProfile.UserName))
            {
                return this.RedirectToAction("ResetPasswordFailure");
            }

            var passwordResetToken = this.authentication.GeneratePasswordResetToken(userProfile.UserName);

            dynamic email = new Email("ResetPasswordEmail");
            email.To = userProfile.Email;
            email.DisplayName = userProfile.DisplayName;
            email.Url = this.Url.Action("ResetPasswordConfirmed", "Account", new { token = passwordResetToken }, this.Request.Url.Scheme);
            email.Send();

            return this.RedirectToAction("ResetPasswordPending");
        }

        [GET("resetpasswordpending")]
        public ViewResult ResetPasswordPending()
        {
            return this.View();
        }

        [GET("resetpasswordconfirmed")]
        public ActionResult ResetPasswordConfirmed(string token)
        {
            var model = new ResetPasswordConfirmedViewModel { Token = token };

            return this.View(model);
        }

        [POST("resetpasswordconfirmed")]
        public ActionResult ResetPasswordConfirmed(ResetPasswordConfirmedViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            return this.RedirectToAction(this.authentication.ResetPassword(model.Token, model.NewPassword) ? "ResetPasswordSuccess" : "ResetPasswordFailure");
        }

        [GET("resetpasswordsuccess")]
        public ViewResult ResetPasswordSuccess()
        {
            return this.View();
        }

        [GET("resetpasswordfailure")]
        public ViewResult ResetPasswordFailure()
        {
            return this.View();
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