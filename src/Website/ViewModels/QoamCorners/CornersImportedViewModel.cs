using System.Collections.Generic;

namespace QOAM.Website.ViewModels.QoamCorners
{
    public class CornersImportedViewModel
    {
        public CornersImportedViewModel()
        {
            ImportedCorners = new List<string>();
            UpdatedCorners = new List<string>();
            ExistingCorners = new List<string>();
        }

        public List<string> ImportedCorners { get; set; }
        public List<string> UpdatedCorners { get; set; }
        public List<string> ExistingCorners { get; set; }
    }
}