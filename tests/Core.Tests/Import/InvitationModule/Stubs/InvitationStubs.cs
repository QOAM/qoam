using System.Collections.Generic;
using System.Data;
using System.Linq;
using QOAM.Core.Tests.Import.StubHelpers;

namespace QOAM.Core.Tests.Import.InvitationModule.Stubs
{
    public static class InvitationStubs
    {
        public static DataSet CompleteDataSet()
        {
            var ds = new DataSet();

            ds.Tables.Add(AuthorsTable());;

            return ds;
        }

        public static DataTable AuthorsTable()
        {
            var table = new DataTable("Authors");

            table.AddColumns("eissn", "Author email address", "Author name");

            table.AddRow(new[] { "1687-8140", "annette.caenen@ugent.be", "A. Caenen" });
            table.AddRow(new[] { "2073-4395", "jeroen.dewaele@ugent.be", "Jeroen De Waele"});
            table.AddRow(new[] { "2372-0484", "maesquiv.EsquivelMerino@ugent.be", "Dolores Esquivel"});
            table.AddRow(new[] { "1687-7675", "eric.vanranst@ugent.be", "E. Van Ranst"});
            table.AddRow(new[] { "1234-7675", "kees.test@ugent.be,k.test@gmail.com;kees.t@outlook.com", "K. Test"});

            return table;
        }
    }
}