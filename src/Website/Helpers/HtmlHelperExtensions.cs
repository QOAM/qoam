using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace QOAM.Website.Helpers
{
    using PagedList.Mvc;

    using HtmlHelper = System.Web.Mvc.HtmlHelper;

    public static class HtmlHelperExtensions
    {
        private static readonly PagedListRenderOptions DefaultPagedListRenderOptions = new PagedListRenderOptions
                                                                                           {
                                                                                               Display = PagedListDisplayMode.IfNeeded,
                                                                                               UlElementClasses = new[] { "pagination" }
                                                                                           };
        
        public static PagedListRenderOptions PagedListRenderOptions(this HtmlHelper helper)
        {
            return DefaultPagedListRenderOptions;
        }

        public static MvcHtmlString AdminLink(this HtmlHelper helper, string linkText, IPrincipal user, string actionName, string controllerName, string cssClass, params string[] necessaryRoles)
        {
            return user.IsInRoles(necessaryRoles) ? helper.ActionLink(linkText, actionName, controllerName, null, new { @class = cssClass }) : helper.ActionLink(linkText, "", "", new { href = "javascript:void(0)", @class = $"{cssClass} disabled" });
        }

        public static MvcHtmlString JavascriptLink(this HtmlHelper helper, string linkText, string id, IPrincipal user, string cssClass, params string[] necessaryRoles)
        {
            return user.IsInRoles(necessaryRoles) ? helper.ActionLink(linkText, "", "", new { id, href="#", @class = cssClass }) : helper.ActionLink(linkText, "", "", new { href = "javascript:void(0)", @class = $"{cssClass} disabled" });
        }

        public static bool IsInRoles(this IPrincipal user, params string[] necessaryRoles)
        {
            return necessaryRoles.Any(user.IsInRole);
        }
    }
}