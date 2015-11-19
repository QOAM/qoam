namespace QOAM.Core.Import.Licences
{
    using System.Collections.Generic;
    using System.IO;

    public interface IBulkImporter
    {
        IList<UniversityLicense> Execute(Stream importFile);
    }
}