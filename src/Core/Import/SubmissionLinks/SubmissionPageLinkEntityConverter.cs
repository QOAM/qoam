using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using QOAM.Core.Import.Licences;

namespace QOAM.Core.Import.SubmissionLinks
{
    public class SubmissionPageLinkEntityConverter : IImportEntityConverter<SubmissionPageLink>
    {
        public List<SubmissionPageLink> Execute(DataSet data)
        {
            var authorsToInvite = (from row in data.Tables["Links"].Rows.Cast<DataRow>()
                                   select new SubmissionPageLink()
                                   {
                                       ISSN = row["eissn"].ToString(),
                                       Url = row["url"].ToString()
                                   }).ToList();

            return authorsToInvite;
        }
    }
}