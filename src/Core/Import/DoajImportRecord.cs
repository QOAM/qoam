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
        public bool NoFee { get; set; }

        public Journal ToJournal()
        {
            return new Journal
                       {
                           DateAdded = DateTime.Now,
                           Title = Title,
                           Link = URL,
                           ISSN = ISSN,
                           DoajSeal = HasSeal,
                           Country = new Country { Name = Country },
                           Publisher = new Publisher { Name = Publisher },
                           Subjects = Subjects.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim().ToLowerInvariant().RemovePreamble(Encoding)).Distinct().Select(s => new Subject { Name = s }).ToSet(),
                           Languages = Language.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim().ToLowerInvariant().RemovePreamble(Encoding)).Distinct().Select(s => new Language { Name = s }).ToSet(),
                           DataSource = JournalsImportSource.DOAJ.ToString(),
                           OpenAccess = true,
                           NoFee = NoFee
                       };
        }
    }
}