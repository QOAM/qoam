namespace QOAM.Website
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

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
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DatabaseConfig.Configure();
            WebSecurityConfig.Configure();
        }
    }
}