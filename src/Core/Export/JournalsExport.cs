using System;
using System.Globalization;
using QOAM.Core.Import;

namespace QOAM.Core.Export
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using CsvHelper;
    using CsvHelper.Configuration;

    using Repositories;

    using Validation;

    public class JournalsExport
    {
        readonly IJournalRepository journalRepository;

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
                .Select(ToExportJournal)
                .ToList();

            ExportJournals(stream, journals);
        }
        
        void ExportJournals(Stream stream, List<ExportJournal> journals)
        {
            Requires.NotNull(stream, nameof(stream));

            using (var streamWriter = new StreamWriter(stream))
                using (var csvWriter = new CsvWriter(streamWriter, CreateCsvConfiguration()))
                {
                    // Add delimiter to help excel open the file in a readable format
                    csvWriter.WriteField($"sep={csvWriter.Configuration.Delimiter}");
                    csvWriter.NextRecord();

                    var (minYear, maxYear) = GetMinAndMaxColumnYears(journals);

                    WriteHeaders(csvWriter, minYear, maxYear);

                    foreach (var j in journals)
                    {
                        csvWriter.WriteField(j.Title);
                        csvWriter.WriteField(j.ISSN);
                        csvWriter.WriteField(j.Link);
                        csvWriter.WriteField(j.DateAdded);
                        csvWriter.WriteField(j.Country);
                        csvWriter.WriteField(j.Publisher);
                        csvWriter.WriteField(j.DataSource);
                        csvWriter.WriteField(j.Languages);
                        csvWriter.WriteField(j.Subjects);
                        csvWriter.WriteField(j.DoajSeal);
                        csvWriter.WriteField(j.PlanSJournal);
                        csvWriter.WriteField(j.Score);
                        csvWriter.WriteField(j.NoFee);
                        csvWriter.WriteField(j.NumberOfArticles);

                        WriteDynamicFields(csvWriter, j.ScoreCardsPerYear, minYear, maxYear);

                        csvWriter.NextRecord();
                    }
                }
        }

        static void WriteHeaders(IWriter csvWriter, int minYear, int maxYear)
        {
            csvWriter.WriteHeader<ExportJournal>();

            WriteDynamicHeaders(csvWriter, "Score cards in", minYear, maxYear);
            //WriteDynamicHeaders(csvWriter, "Articles in", minYear, maxYear);
            
            csvWriter.NextRecord();
        }

        static (int minYear, int maxYear) GetMinAndMaxColumnYears(List<ExportJournal> journals)
        {
            var scoreCardsPerYear = journals.SelectMany(j => j.ScoreCardsPerYear).ToList();
            var minYear = scoreCardsPerYear.Any() ? scoreCardsPerYear.Min(x => x.Key) : DateTime.Now.Year - 2;

            // some journals have incorrect data on the year field, so we set the current year as a max failsafe
            var maxYear = scoreCardsPerYear.Any() ? Math.Min(scoreCardsPerYear.Max(x => x.Key), DateTime.Now.Year) : DateTime.Now.Year;

            return (minYear, maxYear);
        }

        static void WriteDynamicHeaders(IWriterRow csvWriter, string text, int minYear, int maxYear)
        {
            for (var year = minYear; year < maxYear + 1; year++)
            {
                csvWriter.WriteField($"{text} {year}");
            }
        }
        static void WriteDynamicFields(IWriterRow csvWriter, Dictionary<int, int> dictionary, int minYear, int maxYear)
        {
            for (var year = minYear; year < maxYear + 1; year++)
            {
                csvWriter.WriteField(dictionary.FirstOrDefault(x => x.Key == year).Value);
            }
        }

        List<ExportJournal> GetExportJournals(bool openAccessOnly)
        {
            var journals = openAccessOnly
                ? journalRepository.AllWhereIncluding(j => j.OpenAccess, j => j.Country, j => j.Publisher, j => j.Languages, j => j.Subjects, j => j.NumberOfArticles, j => j.ValuationScoreCards)
                : journalRepository.AllIncluding(j => j.Country, j => j.Publisher, j => j.Languages, j => j.Subjects, j => j.NumberOfArticles, j => j.ValuationScoreCards);

            return journals.Select(ToExportJournal).ToList();
        }

        static CsvConfiguration CreateCsvConfiguration()
        {
            return new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";",
                TrimOptions = TrimOptions.Trim
            };
        }

        static ExportJournal ToExportJournal(Journal j)
        {
            var minYear = DateTime.Now.Year - 2;

            return new ExportJournal
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
                DoajSeal = j.DoajSeal ? "Yes" : "No",
                PlanSJournal = j.PlanS ? "Yes" : "No",
                Score = (j.ValuationScore?.AverageScore ?? 0).ToString("0.0"),
                NoFee = j.NoFee ? "Yes" : "No",
                ScoreCardsPerYear = j.ValuationScoreCards
                    .Where(vsc => vsc.DatePublished.HasValue && vsc.DatePublished.Value.Year >= minYear)
                    .GroupBy(vsc => vsc.DatePublished.Value.Year)
                    .ToDictionary(grouping => grouping.Key, grouping => grouping.Count()),
                NumberOfArticles = j.NumberOfArticles
            };
        }
    }
}