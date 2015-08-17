namespace QOAM.Core.Import
{
    using System;
    using System.Linq;
    using CsvHelper.Configuration;
    using QOAM.Core.Helpers;

    public class DoajImportRecord
    {
        public string Title { get; set; }
        public string URL { get; set; }
        public string Publisher { get; set; }
        public string Language { get; set; }
        public string ISSN { get; set; }
        public string Subjects { get; set; }
        public string Country { get; set; }

        public Journal ToJournal()
        {
            return new Journal
                       {
                           DateAdded = DateTime.Now,
                           Title = this.Title,
                           Link = this.URL,
                           ISSN = this.ISSN,
                           Country = new Country { Name = this.Country },
                           Publisher = new Publisher { Name = this.Publisher },
                           Subjects = this.Subjects.Split(new[] { " --- " }, StringSplitOptions.RemoveEmptyEntries).Select(s => new Subject { Name = s.Trim().ToLowerInvariant() }).ToSet(),
                           Languages = this.Language.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).Select(s => new Language { Name = s }).ToSet()
                       };
        }
    }

    public sealed class DoajImportRecordMap : CsvClassMap<DoajImportRecord>
    {
        public DoajImportRecordMap()
        {
            Map(m => m.Title).Name("Journal title");
            Map(m => m.URL).Name("Journal URL");
            Map(m => m.Publisher).Name("Publisher");
            Map(m => m.Language).Name("Full text language");
            Map(m => m.ISSN).Name("Journal ISSN (print version)");
            Map(m => m.Subjects).Name("Keywords");
            Map(m => m.Country).Name("Country of publisher");


            
        }
    }
}