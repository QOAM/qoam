namespace QOAM.Core.Import.Licences
{
    using System.Collections.Generic;

    public class UniversityLicense
    {
        public string Domain { get; set; }
        public IList<LicenseInfo> Licenses { get; set; }
    }
}