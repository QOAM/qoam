namespace RU.Uci.OAMarket.Website.ViewModels.Score
{
    using System.Collections.Generic;

    public class JournalViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISSN { get; set; }
        public string Publisher { get; set; }
        public string Link { get; set; }
        public virtual IEnumerable<string> Languages { get; set; }
        public virtual IEnumerable<string> Subjects { get; set; }
    }
}