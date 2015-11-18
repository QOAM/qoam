using System.Collections.Generic;
using System.IO;

namespace QOAM.Core.Import.Licences
{
    public interface IBulkImporter
    {
        IList<UniversityLicense> Execute(Stream importFile);
    }
}