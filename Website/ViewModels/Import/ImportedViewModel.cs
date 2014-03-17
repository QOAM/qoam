namespace QOAM.Website.ViewModels.Import
{
    using System.Collections.Generic;

    public class ImportedViewModel
    {
        public IEnumerable<string> NotFoundISSNs { get; set; }
        public IEnumerable<string> FoundISSNs { get; set; }
    }
}