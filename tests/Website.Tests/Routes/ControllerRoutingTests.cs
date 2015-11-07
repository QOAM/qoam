namespace QOAM.Website.Tests.Routes
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Routing;
    using MvcRouteTester;

    using QOAM.Website.Tests.TestHelpers;
    using Website.Controllers;

    public abstract class ControllerRoutingTests<TController> : ControllerTests<TController>
        where TController : Controller
    {
        protected RouteCollection ApplicationRoutes { get; }

        protected ControllerRoutingTests()
        {
            ApplicationRoutes = new RouteCollection();

            ApplicationRoutes.MapAttributeRoutesInAssembly(typeof(HomeController).Assembly);
        }

        protected bool ActionRequiresHttps(Expression<Func<TController, ActionResult>> action)
        {
            return GetAttribute<RequireHttpsAttribute>(action) != null;
        }
    }
}