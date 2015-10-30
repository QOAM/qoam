using System.Data;
using NPOI.SS.UserModel;

namespace QOAM.Core.Import.Licences
{
    public interface ILicenseFileImporter
    {
        DataSet Extract(IWorkbook workbook);
    }
}