using System.Linq;

namespace QOAM.Website.ViewModels.Score
{
    using System.Collections.Generic;
    using Core.Helpers;
    using Core.Repositories.Filters;

    public class IndexViewModel : Journals.IndexViewModel
    {
        public IEnumerable<int> JournalIdsInMyQOAM { get; set; }

        public new JournalFilter ToFilter()
        {
            return new JournalFilter
                   {
                       Title = this.Title.TrimSafe(), 
                       Issn = this.Issn.TrimSafe(), 
                       Publisher = this.Publisher.TrimSafe(),
                       Disciplines = this.SelectedDisciplines ?? Enumerable.Empty<int>(),
                       Languages = this.Languages ?? Enumerable.Empty<string>(),
                       SortMode = this.SortBy, 
                       SortDirection = this.Sort, 
                       PageNumber = this.Page, 
                       PageSize = this.PageSize,
                   };
        }

        public new UserJournalFilter ToFilter(int userProfileId)
        {
            return new UserJournalFilter
            {
                Title = Title.TrimSafe(),
                Issn = Issn.TrimSafe(),
                Publisher = Publisher.TrimSafe(),
                Disciplines = SelectedDisciplines ?? Enumerable.Empty<int>(),
                Languages = Languages ?? Enumerable.Empty<string>(),
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
    }
}