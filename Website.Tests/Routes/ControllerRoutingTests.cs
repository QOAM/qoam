namespace QOAM.Website.Tests.Routes
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Routing;

    using QOAM.Website.Tests.TestHelpers;

    public abstract class ControllerRoutingTests<TController> : ControllerTests<TController>
        where TController : Controller
    {
        protected ControllerRoutingTests()
        {
            RouteTable.Routes.Clear();
            AttributeRoutingConfig.RegisterRoutes(RouteTable.Routes);

            GlobalFilters.Filters.Clear();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        protected static bool ActionRequiresHttps(Expression<Func<TController, ActionResult>> action)
        {
            return GetAttribute<RequireHttpsAttribute>(action) != null;
        }
    }
}