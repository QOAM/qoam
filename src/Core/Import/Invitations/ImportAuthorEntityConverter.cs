using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using QOAM.Core.Import.Licences;

namespace QOAM.Core.Import.Invitations
{
    public class ImportAuthorEntityConverter : IImportEntityConverter<AuthorToInvite>
    {
        public List<AuthorToInvite> Execute(DataSet data)
        {
            var authorsToInvite = (from row in data.Tables["Authors"].Rows.Cast<DataRow>()
                                   // Trim before splitting to remove any trailing ';' and ','. Splitting without removing empty entries so 
                                   // that we can present the user with a proper overview of who hasn't been invited and why
                                   from email in row["Author email address"].ToString().Trim(';', ',').Split(new[] { ";", "," }, StringSplitOptions.None)
                                   select new AuthorToInvite
                                   {
                                       ISSN = row["eissn"].ToString(),
                                       AuthorEmail = email.Replace(" ", ""),
                                       AuthorName = row["Author name"].ToString()
                                   }).ToList();

            return authorsToInvite;
        }
    }
}