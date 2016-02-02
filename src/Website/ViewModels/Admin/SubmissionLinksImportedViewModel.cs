using System.Collections.Generic;
using QOAM.Core.Import.SubmissionLinks;

namespace QOAM.Website.ViewModels.Admin
{
    public class SubmissionLinksImportedViewModel
    {
        public int AmountImported { get; set; }
        public int AmountRejected { get; set; }
        public List<SubmissionPageLink> RejectedUrls { get; set; }
    }
}