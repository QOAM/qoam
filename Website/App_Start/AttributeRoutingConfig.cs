namespace QOAM.Website
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class AttributeRoutingConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("elmah.axd");

            routes.MapMvcAttributeRoutes();
        }
    }
}