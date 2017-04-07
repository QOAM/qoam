using System.ComponentModel;
using QOAM.Core.Import.JournalTOCs;

namespace QOAM.Website.ViewModels.Import
{
    public class JournalTocsImportViewModel
    {
        [DisplayName("Fetch Mode")]
        public JournalTocsFetchMode FetchMode { get; set; }
    }
}