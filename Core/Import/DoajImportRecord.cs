namespace QOAM.Core.Import
{
    using System;
    using System.Linq;

    using QOAM.Core.Helpers;

    public class DoajImportRecord
    {
        public string Title { get; set; }
        public string Identifier { get; set; }
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
                           Link = this.Identifier,
                           ISSN = this.ISSN,
                           Country = new Country { Name = this.Country },
                           Publisher = new Publisher { Name = this.Publisher },
                           Subjects = this.Subjects.Split(new[] { " --- " }, StringSplitOptions.RemoveEmptyEntries).Select(s => new Subject { Name = s.Trim().ToLowerInvariant() }).ToSet(),
                           Languages = this.Language.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).Select(s => new Language { Name = s }).ToSet()
                       };
        }
    }
}