using System.Collections.Generic;
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
        [Name("DOAJ Seal"), BooleanTrueValues("Yes"), BooleanFalseValues("No")]
        public string DoajSeal { get; set; }

        //[Name("Score cards in 2018")]
        //public int ScoreCardsIn2018 { get; set; }

        //[Name("Score cards in 2019")]
        //public int ScoreCardsIn2019 { get; set; }

        [Name("Plan S Journal"), BooleanTrueValues("Yes"), BooleanFalseValues("No")]
        public string PlanSJournal { get; set; }

        public string Score { get; set; }
        [Name("No-Fee Journal"), BooleanTrueValues("Yes"), BooleanFalseValues("No")]
        public string NoFee { get; set; }

        [Ignore] // We will write this field manually to the csv file
        public Dictionary<int, int> ArticlesPerYear { get; set; }

        [Ignore] // We will write this field manually to the csv file
        public Dictionary<int, int> ScoreCardsPerYear { get; set; }
    }
}