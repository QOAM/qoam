using System.Collections.Generic;
using System.IO;

namespace QOAM.Core.Import
{
    public interface IBulkImporter<T>
    {
        IList<T> Execute(Stream importFile);
    }
}