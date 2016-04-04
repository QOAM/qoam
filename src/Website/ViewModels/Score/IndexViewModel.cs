using System.Linq;

namespace QOAM.Website.ViewModels.Score
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Routing;
    using PagedList;

    using QOAM.Core;
    using QOAM.Core.Helpers;
    using QOAM.Core.Repositories.Filters;

    public class IndexViewModel : PagedViewModel
    {
        public IndexViewModel()
        {
            this.Journals = new PagedList<Journal>(new Journal[0], this.Page, this.PageSize);
            this.Disciplines = new List<string>();
        }

        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("ISSN")]
        public string Issn { get; set; }

        [DisplayName("Publisher")]
        public string Publisher { get; set; }

        public JournalSortMode SortBy { get; set; }

        public SortDirection Sort { get; set; }

        public IPagedList<Journal> Journals { get; set; }

        [DisplayName("Discipline")]
        public IList<string> Disciplines { get; set; }

        [DisplayName("Language")]
        public IList<string> Languages { get; set; }

        public IEnumerable<int> JournalIdsInMyQOAM { get; set; }

        public JournalFilter ToFilter()
        {
            return new JournalFilter
                   {
                       Title = this.Title.TrimSafe(), 
                       Issn = this.Issn.TrimSafe(), 
                       Publisher = this.Publisher.TrimSafe(),
                       Disciplines = this.Disciplines ?? Enumerable.Empty<string>(),
                       Languages = this.Languages ?? Enumerable.Empty<string>(),
                       SortMode = this.SortBy, 
                       SortDirection = this.Sort, 
                       PageNumber = this.Page, 
                       PageSize = this.PageSize,
                   };
        }

        public UserJournalFilter ToFilter(int userProfileId)
        {
            return new UserJournalFilter
            {
                Title = Title.TrimSafe(),
                Issn = Issn.TrimSafe(),
                Publisher = Publisher.TrimSafe(),
                Disciplines = this.Disciplines ?? Enumerable.Empty<string>(),
                Languages = this.Languages ?? Enumerable.Empty<string>(),
                SortMode = SortBy,
                SortDirection = Sort,
                PageNumber = Page,
                PageSize = PageSize,
                UserProfileId = userProfileId
            };
        }

        public bool IsInMyQOAM(int journalId)
        {
            return JournalIdsInMyQOAM.Any(id => id == journalId);
        }

        public RouteValueDictionary ToRouteValueDictionary(int page)
        {
            var routeValueDictionary = new RouteValueDictionary
            {
                [nameof(page)] = page,
                [nameof(Title)] = Title,
                [nameof(Issn)] = Issn,
                [nameof(Publisher)] = Publisher,
                [nameof(Sort)] = Sort,
                [nameof(SortBy)] = SortBy,
            };

            for (var i = 0; i < Disciplines.Count; i++)
            {
                routeValueDictionary[$"{nameof(Disciplines)}[{i}]"] = Disciplines[i];
            }

            for (var i = 0; i < Languages.Count; i++)
            {
                routeValueDictionary[$"{nameof(Languages)}[{i}]"] = Languages[i];
            }

            return routeValueDictionary;
        }
    }
}