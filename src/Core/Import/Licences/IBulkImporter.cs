namespace QOAM.Core.Import.Licences
{
    using System.Collections.Generic;
    using System.IO;

    public interface IBulkImporter<T>
    {
        IList<T> Execute(Stream importFile);
    }
}