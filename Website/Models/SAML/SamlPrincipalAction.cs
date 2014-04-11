namespace QOAM.Website.Models.SAML
{
    using System.Linq;
    using System.Web;

    using SAML2;
    using SAML2.Actions;
    using SAML2.Identity;
    using SAML2.Protocol;

    /// <summary>
    /// Sets the <see cref="Saml20Identity"/> on the current HTTP context. We use a custom implementation here due to conversion issues 
    /// between our previous OAuth and our current SAML authentication implementation.
    /// </summary>
    public class SamlPrincipalAction : IAction
    {
        public SamlPrincipalAction()
        {
            this.Name = "SetSamlPrincipal";
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
            if (AssertionDoesNotContainAllRequiredAttributes(assertion))
            {
                return;
            }

            // Extract the user's name from the SAML attributes
            var name = assertion.Attributes.First(s => s.Name == SamlAttributes.UID).AttributeValue.First();

            // Create the identity
            var identity = new Saml20Identity(name, assertion.Attributes, name);

            // Store the identity in the session to be able to access it later on
            HttpContext.Current.Session[typeof(Saml20Identity).FullName] = identity;
        }

        /// <summary>
        /// Action performed during logout.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="context">The context.</param>
        /// <param name="idpInitiated">During IdP initiated logout some actions such as redirecting should not be performed</param>
        public void LogoutAction(AbstractEndpointHandler handler, HttpContext context, bool idpInitiated)
        {
            HttpContext.Current.Session.Remove(typeof(Saml20Identity).FullName);
        }

        private static bool AssertionDoesNotContainAllRequiredAttributes(Saml20Assertion assertion)
        {
            return !SamlAttributes.GetRequiredAttributes().IsSubsetOf(assertion.Attributes.Select(a => a.Name));
        }
    }
}