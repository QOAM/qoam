using System.Linq;
using QOAM.Core;
using QOAM.Core.Import;

namespace QOAM.Website.ModelExtensions
{
    public static class JournalExtensions
    {
        public static string BonaFideText(this Journal journal)
        {
            if (journal.NoFee)
                return "This is a no-fee journal";
            
            if (journal.IsIncludedInDoaj())
                return "This journal is included in DOAJ";
            
            if (journal.InstitutionJournalPrices.Any())
                return $"This journal is contracted by {journal.InstitutionJournalPrices.Count} {(journal.InstitutionJournalPrices.Count == 1 ? "institution" : "institutions")}";
            
            if (journal.TrustingInstitutions.Any())
                return $"This journal is trusted by {journal.TrustingInstitutions.Count} {(journal.TrustingInstitutions.Count == 1 ? "library" : "libraries")}";
            
            return "";
        }

        public static bool DisplayExpressTrustText(this Journal journal)
        {
            return !journal.NoFee && !journal.IsIncludedInDoaj() && !journal.InstitutionJournalPrices.Any();
        }

        public static bool IsIncludedInDoaj(this Journal journal)
        {
            return journal.InDoaj;
        }
    }
}