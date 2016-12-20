using System.Collections.Generic;
using QOAM.Core;

namespace QOAM.Website.ViewModels.Admin
{
    public class RemoveCornerViewModel
    {
        public IList<Corner> Corners { get; set; }
        public int CornerId { get; set; }
    }
}