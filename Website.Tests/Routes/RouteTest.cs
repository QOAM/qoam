namespace QOAM.Website.Tests.Routes
{
    using System.Web.Routing;

    public abstract class RouteTest
    {
        protected RouteTest()
        {
            this.Routes = new RouteCollection();
            AttributeRoutingConfig.RegisterRoutes(this.Routes);
        }

        public RouteCollection Routes { get; private set; }
    }
}