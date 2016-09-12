using System.Collections.Generic;
using QOAM.Core;

namespace QOAM.Website.ViewModels.Admin
{
    public class InstitutionsAddedViewModel
    {
        public InstitutionsAddedViewModel()
        {
            Invalid = new List<Institution>();
            ExistingNames = new List<Institution>();
            ExistingDomains = new List<Institution>();
        }

        public List<Institution> Invalid { get; set; }
        public List<Institution> ExistingNames { get; set; }
        public List<Institution> ExistingDomains { get; set; }
        public int AmountImported { get; set; }
    }
}