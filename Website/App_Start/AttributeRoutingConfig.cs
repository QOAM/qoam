namespace QOAM.Website
{
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Routing;

    using AttributeRouting.Web.Mvc;

    public static class AttributeRoutingConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromAssembly(Assembly.GetExecutingAssembly());
                config.UseLowercaseRoutes = true;
                config.PreserveCaseForUrlParameters = true;
            });
        }
    }
}