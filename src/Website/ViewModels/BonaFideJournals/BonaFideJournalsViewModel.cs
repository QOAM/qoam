using System.Linq;
using QOAM.Core;
using QOAM.Website.ViewModels.Journals;

namespace QOAM.Website.ViewModels.BonaFideJournals
{
    public class BonaFideJournalsViewModel : IndexViewModel
    {
        public override string JournalLinkCssClass(Journal journal)
        {
            if(journal.NoFee || journal.DoajSeal || journal.InstitutionJournalPrices.Any() || journal.TrustingInstitutions.Count >= 3)
                return "";

            if (journal.TrustingInstitutions.Count >= 1)
                return "lightblue";

            return "grey";
        }
    }
}