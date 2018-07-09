namespace QOAM.Website.Controllers
{
    using System;
    using System.Linq;
    using System.Net.Mail;
    using System.Web.Mvc;
    using System.Web.Security;
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

        public AccountController(IBaseScoreCardRepository baseScoreCardRepository, IValuationScoreCardRepository valuationScoreCardRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication, IInstitutionRepository institutionRepository, IJournalRepository journalRepository)
            : base(baseScoreCardRepository, valuationScoreCardRepository, userProfileRepository, authentication)
        {
            Requires.NotNull(authentication, nameof(authentication));
            Requires.NotNull(userProfileRepository, nameof(userProfileRepository));
            Requires.NotNull(institutionRepository, nameof(institutionRepository));
            Requires.NotNull(journalRepository, nameof(journalRepository));

            this.authentication = authentication;
            this.userProfileRepository = userProfileRepository;
            this.institutionRepository = institutionRepository;
            this.journalRepository = journalRepository;
        }

        [HttpGet, Route("login")]
        public ViewResult Login(string returnUrl, string loginAddress)
        {
            this.ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index", "Journals");

            if (string.IsNullOrEmpty(loginAddress))
            {
                return this.View();
            }

            var model = new LoginViewModel { Email = loginAddress, RememberMe = false };

            return this.View(model);
        }

        [HttpPost, Route("login")]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (this.ModelState.IsValid)
            {
                var userProfile = this.userProfileRepository.FindByEmail(model.Email);

                if (userProfile != null && this.authentication.Login(userProfile.UserName, model.Password, model.RememberMe))
                {
                    userProfile.DateLastLogin = DateTime.Now;

                    this.userProfileRepository.InsertOrUpdate(userProfile);
                    this.userProfileRepository.Save();

                    return this.RedirectToLocal(returnUrl);
                }
            }

            this.ModelState.AddModelError("", "The email address or password provided is incorrect.");

            return this.View(model);
        }

        [HttpGet, Route("logout")]
        public RedirectToRouteResult Logout()
        {
            this.authentication.Logout();

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet, Route("register")]
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

        [HttpPost, Route("register")]
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

            // We do first a full host comparison tu rule out similar insitution names.
            // In case it doesn't succeed, we try a partial comparison to be sure we 
            // include subomains of existing institutions
            var institution = institutionRepository.FindByExactHost(mailAddress) ?? institutionRepository.Find(mailAddress);
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

        [HttpGet, Route("registrationpending")]
        public ViewResult RegistrationPending()
        {
            return this.View();
        }

        [HttpGet, Route("registerconfirmation/{token}")]
        public ActionResult RegisterConfirmation(string token)
        {
            return this.RedirectToAction(!this.Authentication.ConfirmAccount(token) ? "RegisterFailure" : "RegisterSuccess");
        }

        [HttpGet, Route("registersuccess")]
        public ViewResult RegisterSuccess()
        {
            return this.View();
        }

        [HttpGet, Route("registersuccesswithlink")]
        public ActionResult RegisterSuccessWithLink(string addLink)
        {
            var model = new RequestValuationViewModel { JournalId = Convert.ToInt32(addLink.Split('/').Last()) };
            model.JournalTitle = this.journalRepository.Find(model.JournalId).Title;
            
            return this.View(model);
        }

        [HttpGet, Route("registerfailure")]
        public ViewResult RegisterFailure()
        {
            return this.View();
        }

        [HttpGet, Route("account/settings")]
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

        [HttpPost, Route("account/settings")]
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

        [HttpGet, Route("changepassword")]
        [Authorize]
        public ViewResult ChangePassword(bool saveSuccessful = false)
        {
            this.ViewBag.SaveSuccessful = saveSuccessful;

            return this.View();
        }

        [HttpPost, Route("changepassword")]
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

        [HttpGet, Route("resetpassword")]
        public ViewResult ResetPassword()
        {
            return this.View();
        }

        [HttpPost, Route("resetpassword")]
        [ValidateAntiForgeryToken]
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

        [HttpGet, Route("resetpasswordpending")]
        public ViewResult ResetPasswordPending()
        {
            return this.View();
        }

        [HttpGet, Route("resetpasswordconfirmed")]
        public ViewResult ResetPasswordConfirmed(string token)
        {
            var model = new ResetPasswordConfirmedViewModel { Token = token };

            return this.View(model);
        }

        [HttpPost, Route("resetpasswordconfirmed")]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPasswordConfirmed(ResetPasswordConfirmedViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            return this.RedirectToAction(this.authentication.ResetPassword(model.Token, model.NewPassword) ? "ResetPasswordSuccess" : "ResetPasswordFailure");
        }

        [HttpGet, Route("resetpasswordsuccess")]
        public ViewResult ResetPasswordSuccess()
        {
            return this.View();
        }

        [HttpGet, Route("resetpasswordfailure")]
        public ViewResult ResetPasswordFailure()
        {
            return this.View();
        }
        
        [HttpGet, Route("changeemail")]
        [Authorize]
        public ViewResult ChangeEmail(bool? saveSuccessful = null)
        {
            this.ViewBag.SaveSuccessful = saveSuccessful;

            return this.View();
        }

        [HttpPost, Route("changeemail")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeEmail(ChangeEmailViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            if (!this.authentication.ValidateUser(this.authentication.CurrentUserName, model.Password))
            {
                this.ModelState.AddModelError(nameof(model.Password), "The password is incorrect.");
                return this.View(model);
            }
            
            if (this.institutionRepository.Find(new MailAddress(model.NewEmail)) == null)
            {
                this.ModelState.AddModelError(nameof(model.NewEmail), "Sorry, the domain name in your email address does not match the name of an academic institution known to us. If you want your institution to be included in our list, please enter it’s name and web address in our contact box and we will respond promptly.");
                return this.View(model);
            }

            var userProfile = this.userProfileRepository.Find(this.authentication.CurrentUserName);

            if (string.Equals(userProfile.Email, model.NewEmail, StringComparison.InvariantCultureIgnoreCase))
            {
                return this.RedirectToAction("ChangeEmail", new { saveSuccessful = true });
            }

            if (this.userProfileRepository.FindByEmail(model.NewEmail) != null)
            {
                this.ModelState.AddModelError(nameof(model.NewEmail), "Sorry, unable to use this email address.");
                return this.View(model);
            }

            userProfile.Email = model.NewEmail;

            this.userProfileRepository.InsertOrUpdate(userProfile);
            this.userProfileRepository.Save();

            return this.RedirectToAction("ChangeEmail", new { saveSuccessful = true });
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