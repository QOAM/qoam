namespace QOAM.Core.Export
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using CsvHelper;
    using CsvHelper.Configuration;

    using QOAM.Core.Repositories;

    using Validation;

    public class JournalsExport
    {
        private readonly IJournalRepository journalRepository;

        public JournalsExport(IJournalRepository journalRepository)
        {
            Requires.NotNull(journalRepository, nameof(journalRepository));

            this.journalRepository = journalRepository;
        }

        public void ExportAllJournals(Stream stream)
        {
            Requires.NotNull(stream, nameof(stream));
            
            ExportJournals(stream, false);
        }

        public void ExportOpenAccessJournals(Stream stream)
        {
            Requires.NotNull(stream, nameof(stream));

            ExportJournals(stream, true);
        }

        private void ExportJournals(Stream stream, bool openAccessOnly)
        {
            Requires.NotNull(stream, nameof(stream));

            using (var streamWriter = new StreamWriter(stream))
            using (var csvWriter = new CsvWriter(streamWriter, CreateCsvConfiguration()))
            {
                csvWriter.WriteRecords(this.GetExportJournals(openAccessOnly));
            }
        }

        private IEnumerable<ExportJournal> GetExportJournals(bool openAccessOnly)
        {
            var journals = openAccessOnly ? 
                journalRepository.AllWhereIncluding(j => j.OpenAccess, j => j.Country, j => j.Publisher, j => j.Languages, j => j.Subjects) : 
                this.journalRepository.AllIncluding(j => j.Country, j => j.Publisher, j => j.Languages, j => j.Subjects);

            return journals.Select(j => new ExportJournal
                                        {
                                            Title = j.Title,
                                            ISSN = j.ISSN,
                                            Link = j.Link,
                                            DateAdded = j.DateAdded,
                                            Country = j.Country.Name,
                                            Publisher = j.Publisher.Name,
                                            Languages = string.Join(",", j.Languages.Select(l => l.Name)),
                                            Subjects = string.Join(",", j.Subjects.Select(l => l.Name)),
                                        });
        }

        private static CsvConfiguration CreateCsvConfiguration()
        {
            return new CsvConfiguration
            {
                HasHeaderRecord = true,
                Delimiter = ";",
                TrimFields = true,
            };
        }
    }
}