namespace QOAM.Core
{

    public class ArticlesPerYear : Entity
    {
        public int JournalId { get; set; }
        public int Year { get; set; }
        public int NumberOfArticles { get; set; }

        public virtual Journal Journal { get; set; }
    }
}