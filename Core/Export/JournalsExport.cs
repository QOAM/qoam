namespace QOAM.Core.Export
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using CsvHelper;
    using CsvHelper.Configuration;

    using QOAM.Core.Repositories;

    using Validation;

    public class JournalsExport
    {
        private static readonly CsvConfiguration CsvConfiguration = new CsvConfiguration
                                                                    {
                                                                        HasHeaderRecord = true,
                                                                        Delimiter = ";",
                                                                        TrimFields = true
                                                                    };

        private readonly IJournalRepository journalRepository;

        public JournalsExport(IJournalRepository journalRepository)
        {
            Requires.NotNull(journalRepository, "journalRepository");

            this.journalRepository = journalRepository;
        }

        public void ExportAllJournals(Stream stream)
        {
            Requires.NotNull(stream, "stream");
            
            using (var streamWriter = new StreamWriter(stream))
            using (var csvWriter = new CsvWriter(streamWriter, CsvConfiguration))
            {
                csvWriter.WriteRecords((IEnumerable)this.GetExportJournals());
            }
        }

        private IEnumerable<ExportJournal> GetExportJournals()
        {
            var journals = this.journalRepository.AllIncluding(j => j.Country, j => j.Publisher, j => j.Languages, j => j.Subjects);
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
    }
}