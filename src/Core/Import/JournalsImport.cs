namespace QOAM.Core.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
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
            Requires.NotNull(journalRepository, nameof(journalRepository));
            Requires.NotNull(languageRepository, nameof(languageRepository));
            Requires.NotNull(countryRepository, nameof(countryRepository));
            Requires.NotNull(subjectRepository, nameof(subjectRepository));
            Requires.NotNull(publisherRepository, nameof(publisherRepository));
            Requires.NotNull(importSettings, nameof(importSettings));

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
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Error importing chunk");
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
                    Logger.Error(ex, "Error updating chunk");
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

                if (journalUpdateProperties.Contains(JournalUpdateProperty.OpenAccess))
                {
                    currentJournal.OpenAccess = journal.OpenAccess;
                }

                if (journalUpdateProperties.Contains(JournalUpdateProperty.DataSource))
                {
                    currentJournal.DataSource = journal.DataSource;
                }

                if (journalUpdateProperties.Contains(JournalUpdateProperty.Country))
                {
                    currentJournal.Country = countries.First(p => string.Equals(p.Name, journal.Country.Name.Trim().ToLowerInvariant(), StringComparison.InvariantCultureIgnoreCase));
                }

                if (journalUpdateProperties.Contains(JournalUpdateProperty.Publisher))
                {
                    currentJournal.Publisher = publishers.First(p => string.Equals(p.Name, journal.Publisher.Name.Trim().ToLowerInvariant(), StringComparison.InvariantCultureIgnoreCase));
                }

                if (journalUpdateProperties.Contains(JournalUpdateProperty.Languages))
                {
                    currentJournal.Languages.Clear();

                    foreach (var language in journal.Languages.Select(l => languages.First(a => string.Equals(a.Name, l.Name.Trim().ToLowerInvariant(), StringComparison.InvariantCultureIgnoreCase))))
                    {
                        currentJournal.Languages.Add(language);
                    }
                }

                if (journalUpdateProperties.Contains(JournalUpdateProperty.Subjects))
                {
                    currentJournal.Subjects.Clear();

                    foreach (var subject in journal.Subjects.Select(s => subjects.First(u => string.Equals(u.Name, s.Name.Trim().ToLowerInvariant(), StringComparison.InvariantCultureIgnoreCase))))
                    {
                        currentJournal.Subjects.Add(subject);
                    }
                }

                currentJournal.LastUpdatedOn = DateTime.Now;
                this.journalRepository.InsertOrUpdate(currentJournal);
            }

            this.journalRepository.Save();
        }

        private void ImportJournalsInChunk(IList<Journal> newJournalsChunk, IList<Country> countries, IList<Publisher> publishers, IList<Language> languages, IList<Subject> subjects)
        {
            foreach (var journal in newJournalsChunk)
            {
                journal.Country = countries.First(p => string.Equals(p.Name, journal.Country.Name.Trim().ToLowerInvariant(), StringComparison.InvariantCultureIgnoreCase));
                journal.Publisher = publishers.First(p => string.Equals(p.Name, journal.Publisher.Name.Trim().ToLowerInvariant(), StringComparison.InvariantCultureIgnoreCase));
                journal.Languages = journal.Languages.Select(l => languages.First(a => string.Equals(a.Name, l.Name.Trim().ToLowerInvariant(), StringComparison.InvariantCultureIgnoreCase))).ToSet();
                journal.Subjects = journal.Subjects.Select(s => subjects.First(u => string.Equals(u.Name, s.Name.Trim().ToLowerInvariant(), StringComparison.InvariantCultureIgnoreCase))).ToSet();
                journal.DateAdded = DateTime.Now;
                journal.LastUpdatedOn = DateTime.Now;

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

        private IList<Country> GetNewCountries(IEnumerable<Journal> journals)
        {
            return this.GetNewCountryNames(journals).Select(name => new Country { Name = name }).ToList();
        }

        private IList<string> GetNewCountryNames(IEnumerable<Journal> journals)
        {
            var currentCountryNames = this.countryRepository.All.Select(c => c.Name.Trim().ToLowerInvariant()).ToSet(StringComparer.InvariantCultureIgnoreCase);
            var importCountryNames = journals.Select(j => j.Country?.Name.Trim().ToLowerInvariant()).Where(c => c != null).ToSet(StringComparer.InvariantCultureIgnoreCase);

            if(!importCountryNames.Any())
                return new List<string>();

            return importCountryNames.Except(currentCountryNames, StringComparer.InvariantCultureIgnoreCase).ToList();
        }

        private IList<Language> ImportLanguages(IEnumerable<Journal> journals)
        {
            Logger.Info("Importing languages...");

            var newLanguages = this.GetNewLanguages(journals);

            Logger.Info("Inserting {0} new languages into database...", newLanguages.Count());

            this.languageRepository.InsertBulk(newLanguages);
            
            return this.languageRepository.All;
        }

        private IList<Language> GetNewLanguages(IEnumerable<Journal> journals)
        {
            return this.GetNewLanguageNames(journals).Select(name => new Language { Name = name }).ToList();
        }

        private IList<string> GetNewLanguageNames(IEnumerable<Journal> journals)
        {
            var currentLanguageNames = this.languageRepository.All.Select(c => c.Name.ToLowerInvariant()).ToSet(StringComparer.InvariantCultureIgnoreCase);
            var importLanguageNames = journals.SelectMany(j => j.Languages).Select(l => l.Name?.ToLowerInvariant()).ToSet(StringComparer.InvariantCultureIgnoreCase);

            return importLanguageNames.Except(currentLanguageNames, StringComparer.InvariantCultureIgnoreCase).ToList();
        }

        private IList<Subject> ImportSubjects(IEnumerable<Journal> journals)
        {
            Logger.Info("Importing subjects...");

            var newSubjects = this.GetNewSubjects(journals);

            Logger.Info("Inserting {0} new subjects into database...", newSubjects.Count());

            this.subjectRepository.InsertBulk(newSubjects);

            return this.subjectRepository.All;
        }

        private IList<Subject> GetNewSubjects(IEnumerable<Journal> journals)
        {
            return this.GetNewSubjectNames(journals).Select(name => new Subject { Name = name }).ToList();
        }

        private IList<string> GetNewSubjectNames(IEnumerable<Journal> journals)
        {
            var currentSubjectNames = this.subjectRepository.All.Select(c => c.Name.Trim()).ToSet(StringComparer.InvariantCultureIgnoreCase);
            var importSubjectNames = journals.SelectMany(j => j.Subjects).Select(s => s.Name.Trim()).ToSet(StringComparer.InvariantCultureIgnoreCase);
            
            return importSubjectNames.Except(currentSubjectNames, StringComparer.InvariantCultureIgnoreCase).ToList();
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
            var currentPublisherNames = this.publisherRepository.All.Select(c => c.Name.Trim().ToLowerInvariant()).ToSet(StringComparer.InvariantCultureIgnoreCase);
            var importPublisherNames = journals.Select(j => j.Publisher.Name.Trim().ToLowerInvariant()).ToSet(StringComparer.InvariantCultureIgnoreCase);

            return importPublisherNames.Except(currentPublisherNames, StringComparer.InvariantCultureIgnoreCase).ToList();
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