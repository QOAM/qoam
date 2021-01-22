using System;
using System.Web;

namespace QOAM.Website
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Http;
    using System.Web.Routing;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // ControllerBuilder.Current.DefaultNamespaces.Add("Gigantisch.Controllers");

            DependencyInjectionConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);


            ViewEngineConfig.RegisterViewEngines(ViewEngines.Engines);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            
            AttributeRoutingConfig.RegisterRoutes(RouteTable.Routes);

            ModelBinderConfig.RegisterModelBinders(ModelBinders.Binders);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DatabaseConfig.Configure();
            WebSecurityConfig.Configure();
            
        }

        public override void Init()
        {
            base.Init();
            this.AcquireRequestState += ShowRouteValues;
        }

        protected void Application_BeginRequest()
        {
            //if (!Request.Url.LocalPath.StartsWith("/bfj", StringComparison.OrdinalIgnoreCase))
            //{
            //    Response.RedirectToRoute(new RouteValueDictionary
            //    {
            //        {"controller", "BonaFideJournals"},
            //        {"action", "Index"}
            //    });
            //}
        }

        protected void ShowRouteValues(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            if (context == null)
                return;
            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(context));
        }
    }
}