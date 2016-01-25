using System.Data;
using QOAM.Core.Tests.Import.StubHelpers;

namespace QOAM.Core.Tests.Import.SubmissionLinks.Stubs
{
    public static class SubmissionLinkImportStubs
    {
        public static DataSet CompleteDataSet()
        {
            var ds = new DataSet();

            ds.Tables.Add(AuthorsTable());;

            return ds;
        }

        public static DataTable AuthorsTable()
        {
            var table = new DataTable("Links");

            table.AddColumns("eissn", "url");

            table.AddRow(new[] { "1687-8140", "http://ade.sagepub.com/submission" });
            table.AddRow(new[] { "2073-4395", "http://www.mdpi.com/journal/agronomy/submission" });
            table.AddRow(new[] { "2372-0484", "http://www.aimspress.com/journal/Materials/submission" });
            table.AddRow(new[] { "1687-7675", "http://www.hindawi.com/journals/aess/submission" });

            return table;
        }
    }
}