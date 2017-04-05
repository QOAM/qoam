using QOAM.Core.Import;

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
            
            ExportJournals(stream, GetExportJournals(false));
        }

        public void ExportOpenAccessJournals(Stream stream)
        {
            Requires.NotNull(stream, nameof(stream));

            ExportJournals(stream, GetExportJournals(true));
        }

        public void ExportJournalsNotInJournalTocs(Stream stream)
        {
            Requires.NotNull(stream, nameof(stream));

            var journals = journalRepository
                .AllWhereIncluding(j => j.DataSource != JournalsImportSource.JournalTOCs.ToString(), j => j.Country, j => j.Publisher, j => j.Languages, j => j.Subjects)
                .Select(j => new ExportJournal
                {
                    Title = j.Title,
                    ISSN = j.ISSN,
                    Link = j.Link,
                    DateAdded = j.DateAdded,
                    Country = j.Country.Name,
                    Publisher = j.Publisher.Name,
                    DataSource = j.DataSource,
                    Languages = string.Join(",", j.Languages.Select(l => l.Name)),
                    Subjects = string.Join(",", j.Subjects.Select(l => l.Name)),
                })
                .ToList();

            ExportJournals(stream, journals);
        }

        
        void ExportJournals(Stream stream, IEnumerable<ExportJournal> journals)
        {
            Requires.NotNull(stream, nameof(stream));

            using (var streamWriter = new StreamWriter(stream))
            using (var csvWriter = new CsvWriter(streamWriter, CreateCsvConfiguration()))
            {
                csvWriter.WriteRecords(journals);
            }
        }

        IEnumerable<ExportJournal> GetExportJournals(bool openAccessOnly)
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
                                            DataSource = j.DataSource,
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