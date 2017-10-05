using System.Collections.Generic;

namespace QOAM.Core.Import
{
    public class JournalsImportResult
    {
        public int NumberOfImportedJournals { get; set; }
        public List<string> UpdatedIssns { get; set; } = new List<string>();
        public int NumberOfNewJournals { get; set; }
        public List<string> NewIssns { get; set; } = new List<string>();
    }
}