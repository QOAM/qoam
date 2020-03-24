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
            return "";
            //if (!journal.PlanS)
            //    return "black";

            //return OpenAccessJournalIds.Contains(journal.Id) || InstitutionJournalIds.Contains(journal.Id) ? "gold" : "green";
        }

        public override string PlanSImage(Journal journal)
        {
            return !journal.PlanS ? "red" : "green";
        }

        public override string PlanSTooltipText(Journal journal)
        {
            if (!journal.PlanS)
                return "Sorry, this journal does not meet the open access requirements of your funder.";

            return OpenAccessJournalIds.Contains(journal.Id) || InstitutionJournalIds.Contains(journal.Id)
                ? "Publishing your article in this journal meets the open access requirements of your funder."
                : "Self-archiving the accepted version of your article in a <a href=\"http://v2.sherpa.ac.uk/opendoar/search.html\" target=\"_blank\">registered repository</a> meets the open access requirements of your funder.";
        }
    }
}