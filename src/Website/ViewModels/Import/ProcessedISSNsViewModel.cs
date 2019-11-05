namespace QOAM.Website.ViewModels.Import
{
    using System.Collections.Generic;

    public class ProcessedISSNsViewModel
    {
        public string LabelType { get; set; }
        public IEnumerable<string> NotFoundISSNs { get; set; }
        public IEnumerable<string> FoundISSNs { get; set; }
        public IEnumerable<string> HaveScoreCards { get; set; }
        public string CustomMessage { get; set; }
    }
}