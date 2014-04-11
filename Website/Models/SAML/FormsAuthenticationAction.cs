namespace QOAM.Website.Models.SAML
{
    using System.Web;
    using System.Web.Security;

    using SAML2;
    using SAML2.Actions;
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
            // Note: we don't actually set the forms authentication action here as we will do it in the AccountController 
        }

        /// <summary>
        /// Action performed during logout.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="context">The context.</param>
        /// <param name="idpInitiated">During IdP initiated logout some actions such as redirecting should not be performed</param>
        public void LogoutAction(AbstractEndpointHandler handler, HttpContext context, bool idpInitiated)
        {
            FormsAuthentication.SignOut();
        }
    }
}