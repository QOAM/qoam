using System;
using System.Globalization;
using NPOI.SS.Formula.Functions;
using QOAM.Core.Helpers;
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
                    DoajSeal = j.DoajSeal ? "Yes" : "No",
                    ScoreCardsIn2019 = j.ValuationScoreCards.Count(vsc => vsc.DatePublished.HasValue && vsc.DatePublished.Value.Year == 2019),
                    //ArticlesIn2019 = j.ArticlesPerYear.SingleOrDefault(x => x.Year == 2019)?.NumberOfArticles ?? 0,
                    PlanSJournal = j.PlanS ? "Yes" : "No",
                    Score = (j.ValuationScore?.AverageScore ?? 0).ToString("0.0"),
                    NoFee = j.NoFee ? "Yes" : "No",
                    ArticlesPerYear = j.ArticlesPerYear.DistinctBy(a => a.Year).ToDictionary(key => key.Year, value => value.NumberOfArticles)
                })
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
                        csvWriter.WriteField(j.ScoreCardsIn2019);
                        csvWriter.WriteField(j.PlanSJournal);
                        csvWriter.WriteField(j.Score);
                        csvWriter.WriteField(j.NoFee);

                        for (var year = minYear; year < maxYear + 1; year++)
                        {
                            csvWriter.WriteField(j.ArticlesPerYear.FirstOrDefault(x => x.Key == year).Value);
                        }

                        csvWriter.NextRecord();
                    }
                }
        }

        static void WriteHeaders(IWriter csvWriter, int minYear, int maxYear)
        {
            csvWriter.WriteHeader<ExportJournal>();

            for (var year = minYear; year < maxYear + 1; year++)
            {
                csvWriter.WriteField($"Articles in {year}");
            }

            csvWriter.NextRecord();
        }

        static (int minYear, int maxYear) GetMinAndMaxColumnYears(List<ExportJournal> journals)
        {
            var articlesPerYear = journals.SelectMany(j => j.ArticlesPerYear).ToList();
            var minYear = articlesPerYear.Any() ? articlesPerYear.Min(x => x.Key) : 2018;

            // some journals have incorrect data on the year field, so we set the current year as a max failsafe
            var maxYear = articlesPerYear.Any() ? Math.Min(articlesPerYear.Max(x => x.Key), DateTime.Now.Year) : DateTime.Now.Year;

            return (minYear, maxYear);
        }

        List<ExportJournal> GetExportJournals(bool openAccessOnly)
        {
            var journals = openAccessOnly
                ? journalRepository.AllWhereIncluding(j => j.OpenAccess, j => j.Country, j => j.Publisher, j => j.Languages, j => j.Subjects, j => j.ArticlesPerYear, j => j.ValuationScoreCards)
                : journalRepository.AllIncluding(j => j.Country, j => j.Publisher, j => j.Languages, j => j.Subjects, j => j.ArticlesPerYear, j => j.ValuationScoreCards);

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
                                            DoajSeal = j.DoajSeal ? "Yes" : "No",
                                            ScoreCardsIn2019 = j.ValuationScoreCards.Count(vsc => vsc.DatePublished.HasValue && vsc.DatePublished.Value.Year == 2019),
                                            //ArticlesIn2019 = j.ArticlesPerYear.FirstOrDefault(x => x.Year == 2019)?.NumberOfArticles ?? 0,
                                            PlanSJournal = j.PlanS ? "Yes" : "No",
                                            Score = (j.ValuationScore?.AverageScore ?? 0).ToString("0.0"),
                                            NoFee = j.NoFee ? "Yes" : "No",
                                            ArticlesPerYear = j.ArticlesPerYear.DistinctBy(a => a.Year).ToDictionary(key => key.Year, value => value.NumberOfArticles)
                                        }).ToList();
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
    }
}