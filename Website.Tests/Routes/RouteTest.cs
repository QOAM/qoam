namespace RU.Uci.OAMarket.Website.Tests.Routes
{
    using System.Web.Routing;

    using RU.Uci.OAMarket.Website.App_Start;

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