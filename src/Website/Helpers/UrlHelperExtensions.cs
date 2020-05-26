namespace QOAM.Website.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Routing;

    using QOAM.Core.Repositories.Filters;

    public static class UrlHelperExtensions
    {
        private static readonly IDictionary<JournalSortMode, SortDirection> DefaultSortDirectionJournal = new Dictionary<JournalSortMode, SortDirection>
                                                                                                              {
                                                                                                                  { JournalSortMode.Name, SortDirection.Ascending },
                                                                                                                  { JournalSortMode.ValuationScore, SortDirection.Descending },
                                                                                                                  { JournalSortMode.RobustScores, SortDirection.Descending },
                                                                                                              };

        private static readonly IDictionary<UserProfileSortMode, SortDirection> DefaultSortDirectionUserProfile = new Dictionary<UserProfileSortMode, SortDirection>
                                                                                                                      {
                                                                                                                          { UserProfileSortMode.Name, SortDirection.Ascending },
                                                                                                                          { UserProfileSortMode.Institution, SortDirection.Ascending },
                                                                                                                          { UserProfileSortMode.DateRegistered, SortDirection.Descending },
                                                                                                                          { UserProfileSortMode.NumberOfValuationJournalScoreCards, SortDirection.Descending },
                                                                                                                      };

        private static readonly IDictionary<InstitutionSortMode, SortDirection> DefaultSortDirectionInstitution = new Dictionary<InstitutionSortMode, SortDirection>
                                                                                                                      {
                                                                                                                          { InstitutionSortMode.Name, SortDirection.Ascending },
                                                                                                                          { InstitutionSortMode.NumberOfValuationJournalScoreCards, SortDirection.Descending },
                                                                                                                      };

        public static string SortUrl(this UrlHelper helper, JournalSortMode newSortMode, JournalSortMode currentSortMode, SortDirection sortDirection)
        {
            var query = helper.RequestContext.HttpContext.Request.QueryString;
            var values = query.AllKeys.ToDictionary(key => key, key => (object)query[key]);

            values["SortBy"] = newSortMode;
            values["Sort"] = GetOrderDirection(newSortMode, currentSortMode, sortDirection);

            var routeValues = new RouteValueDictionary(values);
            return helper.Action(null, routeValues);
        }

        public static string SortUrl(this UrlHelper helper, UserProfileSortMode newSortMode, UserProfileSortMode currentSortMode, SortDirection sortDirection)
        {
            var query = helper.RequestContext.HttpContext.Request.QueryString;
            var values = query.AllKeys.ToDictionary(key => key, key => (object)query[key]);

            values["SortBy"] = newSortMode;
            values["Sort"] = GetOrderDirection(newSortMode, currentSortMode, sortDirection);

            var routeValues = new RouteValueDictionary(values);
            return helper.Action(null, routeValues);
        }

        public static string SortUrl(this UrlHelper helper, InstitutionSortMode newSortMode, InstitutionSortMode currentSortMode, SortDirection sortDirection)
        {
            var query = helper.RequestContext.HttpContext.Request.QueryString;
            var values = query.AllKeys.ToDictionary(key => key, key => (object)query[key]);

            values["SortBy"] = newSortMode;
            values["Sort"] = GetOrderDirection(newSortMode, currentSortMode, sortDirection);

            var routeValues = new RouteValueDictionary(values);
            return helper.Action(null, routeValues);
        }

        public static SortDirection GetOrderDirection(JournalSortMode newSortMode, JournalSortMode currentSortMode, SortDirection sortDirection)
        {
            if (currentSortMode == newSortMode)
            {
                return sortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            }

            return DefaultSortDirectionJournal[newSortMode];
        }

        public static SortDirection GetOrderDirection(UserProfileSortMode newSortMode, UserProfileSortMode currentSortMode, SortDirection sortDirection)
        {
            if (currentSortMode == newSortMode)
            {
                return sortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            }

            return DefaultSortDirectionUserProfile[newSortMode];
        }

        public static SortDirection GetOrderDirection(InstitutionSortMode newSortMode, InstitutionSortMode currentSortMode, SortDirection sortDirection)
        {
            if (currentSortMode == newSortMode)
            {
                return sortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            }

            return DefaultSortDirectionInstitution[newSortMode];
        }
    }
}