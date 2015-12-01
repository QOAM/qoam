using System.Data;
using NPOI.SS.UserModel;

namespace QOAM.Core.Import
{
    public interface IFileImporter
    {
        DataSet Execute(IWorkbook workbook);
    }
}