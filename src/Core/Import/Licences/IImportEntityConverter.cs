using System.Collections.Generic;
using System.Data;

namespace QOAM.Core.Import.Licences
{
    public interface IImportEntityConverter
    {
        List<UniversityLicense> Convert(DataSet data);
    }
}