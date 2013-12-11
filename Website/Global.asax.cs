namespace RU.Uci.OAMarket.Website
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using RU.Uci.OAMarket.Website.App_Start;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            DependencyInjectionConfig.RegisterComponents();
            ViewEngineConfig.RegisterViewEngines(ViewEngines.Engines);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            AttributeRoutingConfig.RegisterRoutes(RouteTable.Routes);
            ModelBinderConfig.RegisterModelBinders(ModelBinders.Binders);
            AuthConfig.RegisterAuth();
            DatabaseConfig.Configure();
            WebSecurityConfig.Configure();
        }
    }
}