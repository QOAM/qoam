using System.Collections.Generic;
using QOAM.Core;

namespace QOAM.Website.ViewModels.Journals
{
    public class JournalsForInstitutionViewModel : IndexViewModel
    {
        public Institution Institution { get; set; }
        public List<int> InstitutionJournalIds { get; set; }
        public List<int> OpenAccessJournalIds { get; set;}

        public override string JournalLinkCssClass(Journal journal)
        {
            if (!journal.PlanS)
                return "black";

            return OpenAccessJournalIds.Contains(journal.Id) || InstitutionJournalIds.Contains(journal.Id) ? "" : "green";
        }
    }
}