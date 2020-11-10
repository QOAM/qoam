using QOAM.Core;
using QOAM.Website.ViewModels.Journals;

namespace QOAM.Website.ViewModels.BonaFideJournals
{
    public class BonaFideJournalsViewModel : IndexViewModel
    {
        public override string JournalLinkCssClass(Journal journal)
        {
            if(journal.NoFee || journal.DoajSeal || journal.InstitutionJournalPrices.Count > 0)
                return "";

            return "grey";
        }
    }
}