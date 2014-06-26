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
            
            // Retrieve the unique identifier
            var subjectIdentifier = assertion.Attributes.First(a => a.Name == SamlAttributes.EduPersonTargetedID).AttributeValue[0];
            
            // Create the identity
            var identity = new Saml20Identity(subjectIdentifier, assertion.Attributes, null);                        
            
            // Store the identity in the HTTP context to be able to access it later on in our forms authentication action
            context.Items[assertion.Id] = identity;
        }

        /// <summary>
        /// Action performed during logout.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="context">The context.</param>
        /// <param name="idpInitiated">During IdP initiated logout some actions such as redirecting should not be performed</param>
        public void LogoutAction(AbstractEndpointHandler handler, HttpContext context, bool idpInitiated)
        {
            // Note: we do not need to do anything here as we stored the identity in the HTTP context which is
            // automatically cleared after each request
        }

        private static bool AssertionDoesNotContainAllRequiredAttributes(Saml20Assertion assertion)
        {
            return !SamlAttributes.GetRequiredAttributes().IsSubsetOf(assertion.Attributes.Select(a => a.Name));
        }
    }
}