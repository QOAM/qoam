using System.Data;
using QOAM.Core.Tests.Import.StubHelpers;

namespace QOAM.Core.Tests.Import.Corners.Stubs
{
    public static class CornerStubs
    {
        public static DataSet CompleteDataSet()
        {
            var ds = new DataSet();

            ds.Tables.Add(MainTable());
            
            return ds;
        }

        public static DataTable MainTable()
        {
            var table = new DataTable("QOAMcorners");

            table.AddColumns("Test Corner", "Another Corner");

            table.AddRow(new[] { "123-123", "777-888" });
            table.AddRow(new[] { "456-456", "" });
            
            return table;
        }
    }
}