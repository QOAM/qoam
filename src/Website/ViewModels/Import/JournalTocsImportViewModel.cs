using System.ComponentModel;
using QOAM.Core.Import;

namespace QOAM.Website.ViewModels.Import
{
    public class JournalTocsImportViewModel
    {
        [DisplayName("Fetch Mode")]
        public JournalTocsFetchMode FetchMode { get; set; }
    }
}