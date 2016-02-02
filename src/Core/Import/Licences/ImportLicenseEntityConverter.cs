namespace QOAM.Core.Import.Licences
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    public class ImportLicenseEntityConverter : IImportEntityConverter<UniversityLicense>
    {
        public List<UniversityLicense> Execute(DataSet data)
        {
            var licenses = (from row in data.Tables["Universities"].Rows.Cast<DataRow>()
                select new UniversityLicense
                {
                    Domain = row["Domains"].ToString(),
                    Licenses = ExtractJournalLicenses(row["Tabs"].ToString().Split(new[] { ", ", ",", "; ", ";" }, StringSplitOptions.RemoveEmptyEntries), data)
                }).ToList();

            return licenses;
        }

        #region Private Methods

        private static IList<LicenseInfo> ExtractJournalLicenses(IEnumerable<string> licenseNames, DataSet data)
        {
            return (from l in licenseNames
                from row in data.Tables[l].Rows.Cast<DataRow>()
                select new LicenseInfo
                {
                    LicenseName = l,
                    ISSN = row["eISSN"].ToString(),
                    Text = row["Text"].ToString()
                }).ToList();
        }

        #endregion
    }
}