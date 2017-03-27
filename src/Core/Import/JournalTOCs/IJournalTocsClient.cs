using System.Collections.Generic;

namespace QOAM.Core.Import.JournalTOCs
{
    public interface IJournalTocsClient
    {
        List<string> DownloadJournals(JournalTocsFetchMode action = JournalTocsFetchMode.Update);
    }
}