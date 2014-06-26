namespace QOAM.Website.Models.SAML
{
    using System;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    using QOAM.Core;
    using QOAM.Core.Repositories;
    using QOAM.Website.Helpers;

    using SAML2;
    using SAML2.Actions;
    using SAML2.Identity;
    using SAML2.Protocol;

    /// <summary>
    /// Handles setting Forms Authentication cookies. We use a custom implementation here due to conversion issues 
    /// between our previous OAuth and our current SAML authentication implementation.
    /// </summary>
    public class FormsAuthenticationAction : IAction
    {
        public FormsAuthenticationAction()
        {
            this.Name = "FormsAuthentication";
        }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Action performed during SignOn.
        /// </summary>
        /// <param name="handler">The handler initiating the call.</param>
        /// <param name="context">The current http context.</param>
        /// <param name="assertion">The SAML assertion of the currently logged in user.</param>
        public void SignOnAction(AbstractEndpointHandler handler, HttpContext context, Saml20Assertion assertion)
        {
            var saml20Identity = context.Items[assertion.Id] as ISaml20Identity;

            if (saml20Identity == null)
            {
                return;
            }
            
            var authentication = DependencyResolver.Current.GetService<IAuthentication>();
            if (authentication.Login(saml20Identity.GetProvider(), saml20Identity.GetProviderUserId(), createPersistentCookie: false))
            {
                return;
            }

            var institutionRepository = DependencyResolver.Current.GetService<IInstitutionRepository>();
            var userProfileRepository = DependencyResolver.Current.GetService<IUserProfileRepository>();

            var institution = institutionRepository.Find(saml20Identity.GetInstitutionShortName());

            if (institution == null)
            {
                institution = new Institution { Name = saml20Identity.GetInstitutionShortName(), ShortName = saml20Identity.GetInstitutionShortName() };
                institutionRepository.InsertOrUpdate(institution);
            }

            var user = userProfileRepository.Find(saml20Identity.GetProviderUserId());

            if (user != null)
            {
                return;
            }

            var userByEmail = userProfileRepository.FindByEmail(saml20Identity.GetEmail());

            if (userByEmail != null)
            {
                userProfileRepository.UpdateProviderUserId(userByEmail, saml20Identity.GetProviderUserId());
                userProfileRepository.Save();
            }
            else
            {
                userProfileRepository.InsertOrUpdate(new UserProfile
                {
                    UserName = saml20Identity.GetProviderUserId(),
                    DisplayName = saml20Identity.GetDisplayName(),
                    Email = saml20Identity.GetEmail(),
                    DateRegistered = DateTime.Now,
                    Institution = institution
                });

                userProfileRepository.Save();
            }

            authentication.CreateOrUpdateAccount(saml20Identity.GetProvider(), saml20Identity.GetProviderUserId(), saml20Identity.GetProviderUserId());
            authentication.Login(saml20Identity.GetProvider(), saml20Identity.GetProviderUserId(), createPersistentCookie: false);
        }

        /// <summary>
        /// Action performed during logout.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="context">The context.</param>
        /// <param name="idpInitiated">During IdP initiated logout some actions such as redirecting should not be performed</param>
        public void LogoutAction(AbstractEndpointHandler handler, HttpContext context, bool idpInitiated)
        {
            var authentication = DependencyResolver.Current.GetService<IAuthentication>();
            authentication.Logout();
        }
    }
}