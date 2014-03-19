namespace QOAM.Website.Tests.Authorization
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    using QOAM.Website.Tests.TestHelpers;

    public abstract class ControllerAuthorizationTests<TController> : ControllerTests<TController>
        where TController : Controller
    {
        protected ControllerAuthorizationTests()
        {
            GlobalFilters.Filters.Clear();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        protected static bool ActionRequiresAuthorizedUser(Expression<Func<TController, ActionResult>> action)
        {
            return GetAttribute<AuthorizeAttribute>(action) != null;
        }

        protected static bool ActionAuthorizedForUserWithRole(Expression<Func<TController, ActionResult>> action, string role)
        {
            var authorizeAccessAttribute = GetAttribute<AuthorizeAttribute>(action);

            if (authorizeAccessAttribute == null)
            {
                return false;
            }

            var roles = new HashSet<string>(authorizeAccessAttribute.Roles.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries), StringComparer.InvariantCultureIgnoreCase);

            return roles.Contains(role);
        }

        protected static bool ActionDoesNotRequireAuthorizedUser(Expression<Func<TController, ActionResult>> action)
        {
            return GetAttribute<AuthorizeAttribute>(action) == null;
        }
    }
}