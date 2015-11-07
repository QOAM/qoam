namespace QOAM.Website.Tests.TestHelpers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    using MvcContrib.TestHelper;

    public abstract class ControllerTests<TController>
        where TController : Controller
    {
        protected readonly GlobalFilterCollection GlobalApplicationFilters = new GlobalFilterCollection();

        protected ControllerTests()
        {
            FilterConfig.RegisterGlobalFilters(GlobalApplicationFilters);
        }

        protected TAttribute GetAttribute<TAttribute>(Expression<Func<TController, ActionResult>> action)
            where TAttribute : Attribute
        {
            var methodCall = (MethodCallExpression)action.Body;

            if (methodCall.Method.IsDecoratedWith<TAttribute>())
            {
                return methodCall.Method.GetAttribute<TAttribute>();
            }

            if (methodCall.Method.DeclaringType.IsDecoratedWith<TAttribute>())
            {
                return methodCall.Method.DeclaringType.GetAttribute<TAttribute>();
            }

            return GlobalApplicationFilters.Where(f => f.Instance.GetType() == typeof(TAttribute)).Select(f => f.Instance).Cast<TAttribute>().FirstOrDefault();
        }
    }
}