using System.Collections.Generic;
using System.Linq;
using QOAM.Core;

namespace QOAM.Website.ViewModels.Journals
{
    public class JournalsForInstitutionViewModel : IndexViewModel
    {
        public Institution Institution { get; set; }
        public List<int> InstitutionJournalIds { get; set; }
        public List<int> OpenAccessJournalIds { get; set;}
        public override bool? PlanS => true;

        public override string JournalLinkCssClass(int journalId)
        {
            return InstitutionJournalIds.Contains(journalId) || OpenAccessJournalIds.Contains(journalId) ? "" : "orange";
        }
    }
}