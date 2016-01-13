using QOAM.Core.Tests.Import.StubHelpers;

namespace QOAM.Core.Tests.Import.Licenses.Stubs
{
    using System.Data;

    public static class ImportFileStubs
    {
        public static DataSet CompleteDataSet()
        {
            var ds = new DataSet();

            ds.Tables.Add(UniversitiesTable());
            ds.Tables.Add(LicenseTable("Springer"));
            ds.Tables.Add(LicenseTable("Sage"));

            return ds;
        }

        public static DataTable UniversitiesTable()
        {
            var table = new DataTable("Universities");

            table.AddColumns("Domain", "Tabs");

            table.AddRow(new[] { "ru.nl", "Sage" });
            table.AddRow(new[] { "uu.nl", "Springer" });
            table.AddRow(new[] { "uva.nl", "Springer, Sage" });
            table.AddRow(new[] { "rug.nl", "Springer,Sage" });
            table.AddRow(new[] { "ugent.be", "Springer; Sage" });
            table.AddRow(new[] { "upc.cat", "Springer;Sage" });

            return table;
        }

        public static DataTable LicenseTable(string licenseName)
        {
            var table = new DataTable(licenseName);

            table.AddColumns("ISSN", "text");

            table.AddRow(new[] { "0219-3094", "Discount of 100% on publication fee. Please contact your library for more information." });
            table.AddRow(new[] { "0219-3116", "Some random text" });
            table.AddRow(new[] { "0814-6039", "I'm Batman" });
            table.AddRow(new[] { "0942-0940", "No, really." });

            return table;
        
       }
    }
}