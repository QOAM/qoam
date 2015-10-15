using System.Data;

namespace QOAM.Core.Import.Licences
{
    public interface ILicenseFileImporter
    {
        DataSet Extract();
    }
}