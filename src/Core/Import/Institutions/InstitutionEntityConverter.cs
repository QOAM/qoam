using System.Collections.Generic;
using System.Data;
using System.Linq;
using QOAM.Core.Import.Licences;

namespace QOAM.Core.Import.Institutions
{
    public class InstitutionEntityConverter : IImportEntityConverter<Institution>
    {
        public List<Institution> Execute(DataSet data)
        {
            var authorsToInvite = (from row in data.Tables["Institutions"].Rows.Cast<DataRow>()
                                   select new Institution
                                   {
                                       Name = row["Institution"].ToString(),
                                       ShortName = row["Domains"].ToString(),
                                   }).ToList();

            return authorsToInvite;
        }
    }
}