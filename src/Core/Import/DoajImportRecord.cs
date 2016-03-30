namespace QOAM.Core.Import
{
    using System;
    using System.Linq;
    using System.Text;
    using QOAM.Core.Helpers;

    public class DoajImportRecord
    {
        private static readonly Encoding Encoding = new UTF8Encoding(true);

        public string Title { get; set; }
        public string URL { get; set; }
        public string Publisher { get; set; }
        public string Language { get; set; }
        public string ISSN { get; set; }
        public string Subjects { get; set; }
        public string Country { get; set; }
        public bool HasSeal { get; set; }

        public Journal ToJournal()
        {
            return new Journal
                       {
                           DateAdded = DateTime.Now,
                           Title = this.Title,
                           Link = this.URL,
                           ISSN = this.ISSN,
                           DoajSeal = this.HasSeal,
                           Country = new Country { Name = this.Country },
                           Publisher = new Publisher { Name = this.Publisher },
                           Subjects = this.Subjects.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim().ToLowerInvariant().RemovePreamble(Encoding)).Distinct().Select(s => new Subject { Name = s }).ToSet(),
                           Languages = this.Language.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim().ToLowerInvariant().RemovePreamble(Encoding)).Distinct().Select(s => new Language { Name = s }).ToSet()
                       };
        }
    }
}