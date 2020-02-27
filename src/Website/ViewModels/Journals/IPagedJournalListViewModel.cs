using System.Web.Routing;
using PagedList;
using QOAM.Core;
using QOAM.Core.Repositories.Filters;

namespace QOAM.Website.ViewModels.Journals
{
    public interface IPagedJournalListViewModel
    {
        JournalFilter ToFilter();
        UserJournalFilter ToFilter(int userProfileId);
        RouteValueDictionary ToRouteValueDictionary(int page);
        IPagedList<Journal> Journals { get; set; }
        string JournalLinkCssClass(Journal journal);
    }
}