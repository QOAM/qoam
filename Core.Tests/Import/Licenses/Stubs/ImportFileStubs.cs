using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace QOAM.Core.Tests.Import.Licenses.Stubs
{
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

            table.AddColumns("Domein", "Tabbladen");

            table.AddRow(new [] { "ru.nl", "Sage"});
            table.AddRow(new [] { "uu.nl", "Springer" });
            table.AddRow(new [] { "uva.nl", "Springer, Sage"});
            table.AddRow(new [] { "rug.nl", "Springer, Sage"});

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

        #region Private Methods

        static void AddColumns(this DataTable table, params string[] columns)
        {
            foreach (var column in columns)
            {
                table.Columns.Add(column);
            }
        }

        static void AddRow(this DataTable table, Dictionary<string, string> values)
        {
            var row = table.NewRow();

            foreach (var kvp in values)
                row[kvp.Key] = kvp.Value;
        }

        static void AddRow(this DataTable table, IEnumerable<string> values)
        {
            var row = table.NewRow();

            foreach (var kvp in values.Select((v, i) => new { Value = v, Index = i }))
                row[kvp.Index] = kvp.Value;

            table.Rows.Add(row);
        }

        #endregion

    }
}