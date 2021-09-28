using System.Collections.Generic;

namespace QOAM.Core.Import.CrossRef
{
    public interface ICrossRefClient
    {
        List<Item> DownloadJournals();
        List<Item> DownloadJournal(string issn);
    }
}