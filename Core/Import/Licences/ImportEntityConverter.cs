using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace QOAM.Core.Import.Licences
{
    public class ImportEntityConverter
    {
        readonly DataSet _data;

        public ImportEntityConverter(DataSet data)
        {
            _data = data;
        }

        public List<UniversityLicense> Convert()
        {
            var licenses = (from row in _data.Tables["Universities"].Rows.Cast<DataRow>()
                select new UniversityLicense
                {
                    Domain = row["Domein"].ToString(),
                    Licenses = ExtractJournalLicenses(row["Tabbladen"].ToString().Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries))
                }).ToList();

            return licenses;
        }

        #region Private Methods

        IList<LicenseInfo> ExtractJournalLicenses(IEnumerable<string> licenseNames)
        {
            return (from l in licenseNames
                from row in _data.Tables[l].Rows.Cast<DataRow>()
                select new LicenseInfo
                {
                    LicenseName = l,
                    ISSN = row["ISSN"].ToString(),
                    Text = row["text"].ToString()
                }).ToList();
        }  

        #endregion
    }
}