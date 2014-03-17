namespace QOAM.Website.Controllers
{
    using System.Web.Mvc;

    using QOAM.Core.Repositories;
    using QOAM.Website.Helpers;

    using Validation;

    public abstract class ApplicationController : Controller
    {
        protected const string CacheOneQuarter = "CacheOneQuarter";
        protected const string CacheOneHour = "CacheOneHour";
        protected const int AutoCompleteItemsCount = 5;

        protected ApplicationController(IUserProfileRepository userProfileRepository, IAuthentication authentication)
        {
            Requires.NotNull(userProfileRepository, "userProfileRepository");
            Requires.NotNull(authentication, "authentication");

            this.UserProfileRepository = userProfileRepository;
            this.Authentication = authentication;
        }

        public IUserProfileRepository UserProfileRepository { get; private set; }
        public IAuthentication Authentication { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            this.ViewBag.PageId = GetPageId(filterContext); 
            this.ViewBag.SelectedMenuItem = GetSelectedMenuItem(filterContext);
            
            if (!this.User.Identity.IsAuthenticated)
            {
                return;
            }

            this.ViewBag.User = this.UserProfileRepository.Find(this.Authentication.CurrentUserId);

            if (this.ViewBag.User == null)
            {
                this.Logout(filterContext);
            }
        }

        private static string GetPageId(ActionExecutingContext filterContext)
        {
            return string.Format("{0}-{1}-page", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLowerInvariant(), filterContext.ActionDescriptor.ActionName.ToLowerInvariant());
        }

        private static string GetSelectedMenuItem(ActionExecutingContext filterContext)
        {
            return filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
        }

        private void Logout(ActionExecutingContext filterContext)
        {
            this.Authentication.Logout();

            filterContext.Result = this.RedirectToAction("Index", "Home");
        }
    }
}