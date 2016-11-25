namespace QOAM.Core.Import
{
    public enum JournalsImportMode
    {
        InsertOnly = 0,
        UpdateOnly,
        InsertAndUpdate,
        UpdateSealOnly,
    }

    public enum JournalTocsFetchMode
    {
        Setup,
        Update
    }
}