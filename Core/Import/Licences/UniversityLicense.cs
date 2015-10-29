using System.Collections.Generic;

namespace QOAM.Core.Import.Licences
{
    public class UniversityLicense
    {
        public string Domain { get; set; }
        public IList<LicenseInfo> Licenses { get; set; }
    }
}