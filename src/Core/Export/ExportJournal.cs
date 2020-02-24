using CsvHelper.Configuration.Attributes;

namespace QOAM.Core.Export
{
    using System;

    public class ExportJournal
    {
        public string Title { get; set; }
        public string ISSN { get; set; }
        public string Link { get; set; }
        [Name("Date Added")]
        public DateTime DateAdded { get; set; }
        public string Country { get; set; }
        public string Publisher { get; set; }
        [Name("Data source")]
        public string DataSource { get; set; }
        public string Languages { get; set; }
        public string Subjects { get; set; }
        [Name("DOAJ Seal")]
        public string DoajSeal { get; set; }

        [Name("Score cards in 2019")]
        public int ScoreCardsIn2019 { get; set; }

        [Name("Articles in 2019")]
        public int ArticlesIn2019 { get; set; }

        [Name("Plan S Journal")]
        public string PlanSJournal { get; set; }
    }
}