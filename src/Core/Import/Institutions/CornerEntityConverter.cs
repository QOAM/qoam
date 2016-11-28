using System.Collections.Generic;
using System.Data;
using System.Linq;
using QOAM.Core.Import.Licences;
using QOAM.Core.Import.QOAMcorners;

namespace QOAM.Core.Import.Institutions
{
    public class CornerEntityConverter : IImportEntityConverter<CornerToImport>
    {
        public List<CornerToImport> Execute(DataSet data)
        {
            var corners = (from col in data.Tables["QOAMcorners"].Columns.Cast<DataColumn>()
                           select new CornerToImport
                           {
                               Name = col.ColumnName,
                               ISSNs = (from row in data.Tables["QOAMcorners"].Rows.Cast<DataRow>()
                                        let value = row[col.ColumnName].ToString()
                                        where !string.IsNullOrEmpty(value)
                                        select row[col.ColumnName].ToString()).ToList()
                           }).ToList();

            return corners;
        }
    }
}