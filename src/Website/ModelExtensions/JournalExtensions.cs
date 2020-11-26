using System.Linq;
using QOAM.Core;

namespace QOAM.Website.ModelExtensions
{
    public static class JournalExtensions
    {
        public static string BonaFideText(this Journal journal)
        {
            if (journal.TrustingInstitutions.Any())
                return $"This journal is trusted by {journal.TrustingInstitutions.Count} {(journal.TrustingInstitutions.Count == 1 ? "library" : "libraries")}";

            if (journal.InstitutionJournalPrices.Any())
                return $"This journal is contracted by {journal.InstitutionJournalPrices.Count} {(journal.InstitutionJournalPrices.Count == 1 ? "institution" : "institutions")}";

            if (journal.DoajSeal)
                return "This journal is included in DOAJ";

            if (journal.NoFee)
                return "This is a no-fee journal";

            return "";
        }
    }
}