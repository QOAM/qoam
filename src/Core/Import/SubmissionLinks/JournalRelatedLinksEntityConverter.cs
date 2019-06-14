using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using QOAM.Core.Import.Licences;

namespace QOAM.Core.Import.SubmissionLinks
{
    public class JournalRelatedLinksEntityConverter : IImportEntityConverter<JournalRelatedLink>
    {
        public List<JournalRelatedLink> Execute(DataSet data)
        {
            var authorsToInvite = (from row in data.Tables["Links"].Rows.Cast<DataRow>()
                                   select new JournalRelatedLink
                                   {
                                       ISSN = row["eissn"].ToString(),
                                       Url = row["url"].ToString(),
                                       Text = row.Table.Columns.Contains("text") ? row["text"]?.ToString() : ""
                                   }).ToList();

            return authorsToInvite;
        }
    }
}