namespace QOAM.Website.Tests.Controllers.Helpers
{
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Moq;
    using MvcRouteTester;
    using Website.Controllers;

    public static class HttpContextHelper
    {
        public static UrlHelper CreateUrlHelper()
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();

            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);

            request.SetupGet(x => x.ApplicationPath).Returns("/");
            request.SetupGet(x => x.Url).Returns(new Uri("http://localhost/a", UriKind.Absolute));
            request.SetupGet(x => x.ServerVariables).Returns(new NameValueCollection());

            response.Setup(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(x => x);

            context.SetupGet(x => x.Request).Returns(request.Object);
            context.SetupGet(x => x.Response).Returns(response.Object);

            var routes = new RouteCollection();
            routes.MapAttributeRoutesInAssembly(typeof(HomeController).Assembly);

            return new UrlHelper(new RequestContext(context.Object, new RouteData()), routes);
        }
    }
}