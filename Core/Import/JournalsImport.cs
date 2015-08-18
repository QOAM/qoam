namespace QOAM.Core.Import
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NLog;

    using QOAM.Core.Helpers;
    using QOAM.Core.Repositories;

    using Validation;

    public class JournalsImport
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IJournalRepository journalRepository;
        private readonly ILanguageRepository languageRepository;
        private readonly ICountryRepository countryRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly IPublisherRepository publisherRepository;
        private readonly GeneralImportSettings importSettings;

        public JournalsImport(IJournalRepository journalRepository, ILanguageRepository languageRepository, ICountryRepository countryRepository, ISubjectRepository subjectRepository, IPublisherRepository publisherRepository, GeneralImportSettings importSettings)
        {
            Requires.NotNull(journalRepository, "journalRepository");
            Requires.NotNull(languageRepository, "languageRepository");
            Requires.NotNull(countryRepository, "countryRepository");
            Requires.NotNull(subjectRepository, "subjectRepository");
            Requires.NotNull(publisherRepository, "publisherRepository");
            Requires.NotNull(importSettings, "importSettings");

            this.journalRepository = journalRepository;
            this.languageRepository = languageRepository;
            this.countryRepository = countryRepository;
            this.subjectRepository = subjectRepository;
            this.publisherRepository = publisherRepository;
            this.importSettings = importSettings;
        }

        public JournalsImportResult ImportJournals(IList<Journal> journals, JournalsImportMode journalsImportMode)
        {
            var journalUpdateProperties = new HashSet<JournalUpdateProperty>((JournalUpdateProperty[])Enum.GetValues(typeof(JournalUpdateProperty)));
            journalUpdateProperties.Remove(JournalUpdateProperty.DoajSeal);

            return ImportJournals(journals, journalsImportMode, journalUpdateProperties);
        }

        public JournalsImportResult ImportJournals(IList<Journal> journals, JournalsImportMode journalsImportMode, ISet<JournalUpdateProperty> journalUpdateProperties)
        {
            var distinctJournals = journals.Distinct(new JournalIssnEqualityComparer()).ToList();
            
            var countries = this.ImportCountries(distinctJournals);
            var languages = this.ImportLanguages(distinctJournals);
            var subjects = this.ImportSubjects(distinctJournals);
            var publishers = this.ImportPublishers(distinctJournals);

            Logger.Info("Retrieving existing journals from database...");
            var allJournals = this.journalRepository.All;
            
            var currentJournalIssns = this.journalRepository.AllIssns.ToList();
            
            var newJournals = distinctJournals.Where(j => !currentJournalIssns.Contains(j.ISSN, StringComparer.InvariantCultureIgnoreCase)).ToList();

            Logger.Info("Found {0} new journals", newJournals.Count);

            var existingJournals = distinctJournals.Where(j => currentJournalIssns.Contains(j.ISSN, StringComparer.InvariantCultureIgnoreCase)).ToList();

            Logger.Info("Found {0} existing journals", existingJournals.Count);

            if (ShouldInsertJournals(journalsImportMode))
            {
                this.InsertJournals(newJournals, countries, publishers, languages, subjects);
            }

            if (ShouldUpdateJournals(journalsImportMode))
            {
                this.UpdateJournals(existingJournals, countries, publishers, languages, subjects, allJournals, journalUpdateProperties);
            }

            return new JournalsImportResult { NumberOfImportedJournals = distinctJournals.Count, NumberOfNewJournals = newJournals.Count };
        }

        private void InsertJournals(List<Journal> newJournals, IList<Country> countries, IList<Publisher> publishers, IList<Language> languages, IList<Subject> subjects)
        {
            Logger.Info("Importing journals in batches of {0}...", this.importSettings.BatchSize);

            var index = 1;

            foreach (var newJournalsChunk in newJournals.Chunk(this.importSettings.BatchSize).ToList())
            {
                Logger.Info("Importing chunk {0}", index++);

                try
                {
                    this.ImportJournalsInChunk(newJournalsChunk.ToList(), countries, publishers, languages, subjects);
                    this.AddJournalScoreToImportedJournalsInChunk(newJournalsChunk.ToList());
                }
                catch (Exception ex)
                {
                    Logger.Error("Error importing chunk", ex);
                }
            }
        }

        private void UpdateJournals(List<Journal> existingJournals, IList<Country> countries, IList<Publisher> publishers, IList<Language> languages, IList<Subject> subjects, IList<Journal> allJournals, ISet<JournalUpdateProperty> journalUpdateProperties)
        {
            Logger.Info("Updating journals in batches of {0}...", this.importSettings.BatchSize);

            var index = 1;

            foreach (var existingJournalsChunk in existingJournals.Chunk(this.importSettings.BatchSize).ToList())
            {
                Logger.Info("Updating chunk {0}", index++);

                try
                {
                    this.UpdateJournalsInChunk(existingJournalsChunk.ToList(), countries, publishers, languages, subjects, allJournals, journalUpdateProperties);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error updating chunk", ex);
                }
            }
        }

        private void UpdateJournalsInChunk(IList<Journal> existingJournalsChunk, IList<Country> countries, IList<Publisher> publishers, IList<Language> languages, IList<Subject> subjects, IList<Journal> allJournals, ISet<JournalUpdateProperty> journalUpdateProperties)
        {
            foreach (var journal in existingJournalsChunk)
            {
                var currentJournal = allJournals.First(j => string.Equals(j.ISSN, journal.ISSN, StringComparison.InvariantCultureIgnoreCase));

                if (journalUpdateProperties.Contains(JournalUpdateProperty.DoajSeal))
                {
                    currentJournal.DoajSeal = journal.DoajSeal;
                }

                if (journalUpdateProperties.Contains(JournalUpdateProperty.Title))
                {
                    currentJournal.Title = journal.Title;
                }

                if (journalUpdateProperties.Contains(JournalUpdateProperty.Link))
                {
                    currentJournal.Link = journal.Link;
                }

                if (journalUpdateProperties.Contains(JournalUpdateProperty.Country))
                {
                    currentJournal.Country = countries.First(p => string.Equals(p.Name, journal.Country.Name, StringComparison.InvariantCultureIgnoreCase));
                }

                if (journalUpdateProperties.Contains(JournalUpdateProperty.Publisher))
                {
                    currentJournal.Publisher = publishers.First(p => string.Equals(p.Name, journal.Publisher.Name, StringComparison.InvariantCultureIgnoreCase));
                }

                if (journalUpdateProperties.Contains(JournalUpdateProperty.Languages))
                {
                    currentJournal.Languages.Clear();

                    foreach (var language in journal.Languages.Select(l => languages.First(a => string.Equals(a.Name, l.Name, StringComparison.InvariantCultureIgnoreCase))))
                    {
                        currentJournal.Languages.Add(language);
                    }
                }

                if (journalUpdateProperties.Contains(JournalUpdateProperty.Subjects))
                {
                    currentJournal.Subjects.Clear();

                    foreach (var subject in journal.Subjects.Select(s => subjects.First(u => string.Equals(u.Name, s.Name, StringComparison.InvariantCultureIgnoreCase))))
                    {
                        currentJournal.Subjects.Add(subject);
                    }
                }

                this.journalRepository.InsertOrUpdate(currentJournal);
            }

            this.journalRepository.Save();
        }

        private void ImportJournalsInChunk(IList<Journal> newJournalsChunk, IList<Country> countries, IList<Publisher> publishers, IList<Language> languages, IList<Subject> subjects)
        {
            foreach (var journal in newJournalsChunk)
            {
                journal.Country = countries.First(p => string.Equals(p.Name, journal.Country.Name, StringComparison.InvariantCultureIgnoreCase));
                journal.Publisher = publishers.First(p => string.Equals(p.Name, journal.Publisher.Name, StringComparison.InvariantCultureIgnoreCase));
                journal.Languages = journal.Languages.Select(l => languages.First(a => string.Equals(a.Name, l.Name, StringComparison.InvariantCultureIgnoreCase))).ToSet();
                journal.Subjects = journal.Subjects.Select(s => subjects.First(u => string.Equals(u.Name, s.Name, StringComparison.InvariantCultureIgnoreCase))).ToSet();
                journal.DateAdded = DateTime.Now;

                this.journalRepository.InsertOrUpdate(journal);
            }

            this.journalRepository.Save();
        }

        private void AddJournalScoreToImportedJournalsInChunk(IList<Journal> newJournalsChunk)
        {
            foreach (var journal in newJournalsChunk)
            {
                journal.JournalScore = new JournalScore { JournalId = journal.Id };

                this.journalRepository.InsertOrUpdate(journal);
            }

            this.journalRepository.Save();
        }

        private IList<Country> ImportCountries(IEnumerable<Journal> journals)
        {
            Logger.Info("Importing countries...");

            var newCountries = this.GetNewCountries(journals);

            Logger.Info("Inserting {0} new countries into database...", newCountries.Count());

            this.countryRepository.InsertBulk(newCountries);
            
            return this.countryRepository.All;
        }

        private IEnumerable<Country> GetNewCountries(IEnumerable<Journal> journals)
        {
            return this.GetNewCountryNames(journals).Select(name => new Country { Name = name });
        }

        private IEnumerable<string> GetNewCountryNames(IEnumerable<Journal> journals)
        {
            var currentCountryNames = this.countryRepository.All.Select(c => c.Name);
            var importCountryNames = journals.Select(j => j.Country.Name.Trim()).Distinct();

            return importCountryNames.Except(currentCountryNames, StringComparer.InvariantCultureIgnoreCase);
        }

        private IList<Language> ImportLanguages(IEnumerable<Journal> journals)
        {
            Logger.Info("Importing languages...");

            var newLanguages = this.GetNewLanguages(journals);

            Logger.Info("Inserting {0} new languages into database...", newLanguages.Count());

            this.languageRepository.InsertBulk(newLanguages);
            
            return this.languageRepository.All;
        }

        private IEnumerable<Language> GetNewLanguages(IEnumerable<Journal> journals)
        {
            return this.GetNewLanguageNames(journals).Select(name => new Language { Name = name });
        }

        private IEnumerable<string> GetNewLanguageNames(IEnumerable<Journal> journals)
        {
            var currentLanguageNames = this.languageRepository.All.Select(c => c.Name);
            var importLanguageNames = journals.SelectMany(j => j.Languages).Select(l => l.Name).Distinct();

            return importLanguageNames.Except(currentLanguageNames, StringComparer.InvariantCultureIgnoreCase);
        }

        private IList<Subject> ImportSubjects(IEnumerable<Journal> journals)
        {
            Logger.Info("Importing subjects...");

            var newSubjects = this.GetNewSubjects(journals);

            Logger.Info("Inserting {0} new subjects into database...", newSubjects.Count());

            this.subjectRepository.InsertBulk(newSubjects);

            return this.subjectRepository.All;
        }

        private IEnumerable<Subject> GetNewSubjects(IEnumerable<Journal> journals)
        {
            return this.GetNewSubjectNames(journals).Select(name => new Subject { Name = name });
        }

        private IEnumerable<string> GetNewSubjectNames(IEnumerable<Journal> journals)
        {
            var currentSubjectName = this.subjectRepository.All.Select(c => c.Name);
            var importSubjectNames = journals.SelectMany(j => j.Subjects).Select(s => s.Name).Distinct();

            return importSubjectNames.Except(currentSubjectName, StringComparer.InvariantCultureIgnoreCase);
        }

        private IList<Publisher> ImportPublishers(IEnumerable<Journal> journals)
        {
            Logger.Info("Importing publishers...");

            var newPublishers = this.GetNewPublishers(journals);

            Logger.Info("Inserting {0} new publishers into database...", newPublishers.Count());

            this.publisherRepository.InsertBulk(newPublishers);

            return this.publisherRepository.All;
        }

        private IEnumerable<Publisher> GetNewPublishers(IEnumerable<Journal> journals)
        {
            return this.GetNewPublisherNames(journals).Select(name => new Publisher { Name = name });
        }

        private IEnumerable<string> GetNewPublisherNames(IEnumerable<Journal> journals)
        {
            var currentPublisherNames = this.publisherRepository.All.Select(c => c.Name);
            var importPublisherNames = journals.Select(j => j.Publisher.Name).Distinct();

            return importPublisherNames.Except(currentPublisherNames, StringComparer.InvariantCultureIgnoreCase);
        }

        private static bool ShouldInsertJournals(JournalsImportMode journalsImportMode)
        {
            switch (journalsImportMode)
            {
                case JournalsImportMode.InsertOnly:
                case JournalsImportMode.InsertAndUpdate:
                    return true;
                default:
                    return false;
            }
        }

        private static bool ShouldUpdateJournals(JournalsImportMode journalsImportMode)
        {
            switch (journalsImportMode)
            {
                case JournalsImportMode.InsertAndUpdate:
                case JournalsImportMode.UpdateSealOnly:
                case JournalsImportMode.UpdateOnly:
                    return true;
                default:
                    return false;
            }
        }
    }
}