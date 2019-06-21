namespace QOAM.Core
{
    public class ListPrice
    {
        public string Link { get; set; }
        public string Text { get; set; }
        public int JournalId { get; set; }
        public virtual Journal Journal { get; set; }
    }
}