namespace RU.Uci.OAMarket.Website.Helpers
{
    using System;
    using System.Web.Helpers;
    using System.Web.Mvc;

    using Validation;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateJsonAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            Requires.NotNull(filterContext, "filterContext");

            AntiForgery.Validate(GetAntiForgeryCookieToken(filterContext), GetAntiForgeryHeaderToken(filterContext));
        }

        private static string GetAntiForgeryCookieToken(AuthorizationContext filterContext)
        {
            var cookie = filterContext.HttpContext.Request.Cookies[AntiForgeryConfig.CookieName];
            return cookie != null ? cookie.Value : null;
        }

        private static string GetAntiForgeryHeaderToken(AuthorizationContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers["__RequestVerificationToken"];
        }
    }
}