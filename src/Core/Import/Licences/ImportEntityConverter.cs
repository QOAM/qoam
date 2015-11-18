using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace QOAM.Core.Import.Licences
{
    public class ImportEntityConverter : IImportEntityConverter
    {
        public List<UniversityLicense> Execute(DataSet data)
        {
            var licenses = (from row in data.Tables["Universities"].Rows.Cast<DataRow>()
                            select new UniversityLicense
                            {
                                Domain = row["Domein"].ToString(),
                                Licenses = ExtractJournalLicenses(row["Tabbladen"].ToString().Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries), data)
                            }).ToList();

            return licenses;
        }

        #region Private Methods

        static IList<LicenseInfo> ExtractJournalLicenses(IEnumerable<string> licenseNames, DataSet data)
        {
            return (from l in licenseNames
                    from row in data.Tables[l].Rows.Cast<DataRow>()
                    select new LicenseInfo
                    {
                        LicenseName = l,
                        ISSN = row["ISSN"].ToString(),
                        Text = row["Text"].ToString()
                    }).ToList();
        }  

        #endregion
    }
}