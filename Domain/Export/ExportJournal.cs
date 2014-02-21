namespace RU.Uci.OAMarket.Domain.Export
{
    using System;

    public class ExportJournal
    {
        public string Title { get; set; }
        public string ISSN { get; set; }
        public string Link { get; set; }
        public DateTime DateAdded { get; set; }
        public string Country { get; set; }
        public string Publisher { get; set; }
        public string Languages { get; set; }
        public string Subjects { get; set; }
    }
}