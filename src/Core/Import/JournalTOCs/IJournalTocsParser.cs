using System.Collections.Generic;

namespace QOAM.Core.Import.JournalTOCs
{
    public interface IJournalTocsParser
    {
        IList<Journal> Parse(IEnumerable<string> data);
    }
}