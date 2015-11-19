namespace QOAM.Core.Import.Licences
{
    using System.Data;
    using NPOI.SS.UserModel;

    public interface ILicenseFileImporter
    {
        DataSet Execute(IWorkbook workbook);
    }
}