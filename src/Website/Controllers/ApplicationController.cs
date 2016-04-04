namespace QOAM.Website.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using QOAM.Core.Repositories;
    using QOAM.Website.Helpers;

    using Validation;

    public abstract class ApplicationController : Controller
    {
        protected const string CacheOneQuarter = "CacheOneQuarter";
        protected const string CacheOneHour = "CacheOneHour";
        protected const int AutoCompleteItemsCount = 5;

        protected ApplicationController(
            IBaseScoreCardRepository baseScoreCardRepository,
            IValuationScoreCardRepository valuationScoreCardRepository, 
            IUserProfileRepository userProfileRepository, 
            IAuthentication authentication)
        {
            Requires.NotNull(baseScoreCardRepository, nameof(baseScoreCardRepository));
            Requires.NotNull(valuationScoreCardRepository, nameof(valuationScoreCardRepository));
            Requires.NotNull(userProfileRepository, nameof(userProfileRepository));
            Requires.NotNull(authentication, nameof(authentication));

            this.UserProfileRepository = userProfileRepository;
            this.Authentication = authentication;
            this.valuationScoreCardRepository = valuationScoreCardRepository;
            this.baseScoreCardRepository = baseScoreCardRepository;
        }

        public IUserProfileRepository UserProfileRepository { get; private set; }
        public IAuthentication Authentication { get; private set; }
        public IBaseScoreCardRepository baseScoreCardRepository { get; private set; }
        public IValuationScoreCardRepository valuationScoreCardRepository { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            this.ViewBag.PageId = GetPageId(filterContext); 
            this.ViewBag.SelectedMenuItem = GetSelectedMenuItem(filterContext);
            this.ViewBag.LastUpdate = GetLastUpdate();
            
            if (!this.User.Identity.IsAuthenticated)
            {
                return;
            }

            this.ViewBag.User = this.UserProfileRepository.Find(this.Authentication.CurrentUserId);
        }

        private static string GetPageId(ActionExecutingContext filterContext)
        {
            return string.Format("{0}-{1}-page", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLowerInvariant(), filterContext.ActionDescriptor.ActionName.ToLowerInvariant());
        }

        private static string GetSelectedMenuItem(ActionExecutingContext filterContext)
        {
           return $"{filterContext.ActionDescriptor.ControllerDescriptor.ControllerName}/{filterContext.ActionDescriptor.ActionName}";
        }

        private DateTime? GetLastUpdate()
        {
            var lastBaseScoreCardUpdate = this.baseScoreCardRepository.LastUpdate();
            var lastValuationScoreCardUpdate = this.valuationScoreCardRepository.LastUpdate();

            if (lastBaseScoreCardUpdate == null || lastValuationScoreCardUpdate == null)
            {
                return lastBaseScoreCardUpdate ?? lastValuationScoreCardUpdate;
            }

            return new DateTime(Math.Max(lastBaseScoreCardUpdate.Value.Ticks, lastValuationScoreCardUpdate.Value.Ticks));
        }

        protected static List<string> NormalizeSearchStrings(IList<string> input)
        {
            if (input == null)
            {
                return new List<string>();
            }

            return input.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
        }
    }
}