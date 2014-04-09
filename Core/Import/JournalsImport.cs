namespace QOAM.Core.Import
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using QOAM.Core.Helpers;
    using QOAM.Core.Repositories;

    using Validation;

    public class JournalsImport
    {
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
            var distinctJournals = journals.Distinct(new JournalIssnEqualityComparer()).ToList();

            var allJournals = this.journalRepository.All;
            var countries = this.ImportCountries(distinctJournals);
            var languages = this.ImportLanguages(distinctJournals);
            var subjects = this.ImportSubjects(distinctJournals);
            var publishers = this.ImportPublishers(distinctJournals);
            
            var currentJournalIssns = this.journalRepository.AllIssns.ToList();
            var newJournals = distinctJournals.Where(j => !currentJournalIssns.Contains(j.ISSN, StringComparer.InvariantCultureIgnoreCase)).ToList();
            var existingJournals = distinctJournals.Where(j => currentJournalIssns.Contains(j.ISSN, StringComparer.InvariantCultureIgnoreCase)).ToList();

            if (ShouldInsertJournals(journalsImportMode))
            {
                foreach (var newJournalsChunk in newJournals.Chunk(this.importSettings.BatchSize).ToList())
                {
                    this.ImportJournalsInChunk(newJournalsChunk.ToList(), countries, publishers, languages, subjects);
                    this.AddJournalScoreToImportedJournalsInChunk(newJournalsChunk.ToList());
                }
            }

            if (ShouldUpdateJournals(journalsImportMode))
            {
                foreach (var existingJournalsChunk in existingJournals.Chunk(this.importSettings.BatchSize).ToList())
                {
                    this.UpdateJournalsInChunk(existingJournalsChunk.ToList(), countries, publishers, languages, subjects, allJournals);
                }    
            }

            return new JournalsImportResult { NumberOfImportedJournals = distinctJournals.Count, NumberOfNewJournals = newJournals.Count };
        }

        private void UpdateJournalsInChunk(IList<Journal> existingJournalsChunk, IList<Country> countries, IList<Publisher> publishers, IList<Language> languages, IList<Subject> subjects, IList<Journal> allJournals)
        {
            foreach (var journal in existingJournalsChunk)
            {
                var currentJournal = allJournals.First(j => string.Equals(j.ISSN, journal.ISSN, StringComparison.InvariantCultureIgnoreCase));
                currentJournal.Title = journal.Title;
                currentJournal.Link = journal.Link;

                currentJournal.Country = countries.First(p => string.Equals(p.Name, journal.Country.Name, StringComparison.InvariantCultureIgnoreCase));
                currentJournal.Publisher = publishers.First(p => string.Equals(p.Name, journal.Publisher.Name, StringComparison.InvariantCultureIgnoreCase));

                currentJournal.Languages.Clear();

                foreach (var language in journal.Languages.Select(l => languages.First(a => string.Equals(a.Name, l.Name, StringComparison.InvariantCultureIgnoreCase))))
                {
                    currentJournal.Languages.Add(language);    
                }
                
                currentJournal.Subjects.Clear();
                foreach (var subject in journal.Subjects.Select(s => subjects.First(u => string.Equals(u.Name, s.Name, StringComparison.InvariantCultureIgnoreCase))))
                {
                    currentJournal.Subjects.Add(subject);
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
            this.countryRepository.InsertBulk(this.GetNewCountries(journals));
            
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
            this.languageRepository.InsertBulk(this.GetNewLanguages(journals));
            
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
            this.subjectRepository.InsertBulk(this.GetNewSubjects(journals));

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
            this.publisherRepository.InsertBulk(this.GetNewPublishers(journals));

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
            return journalsImportMode == JournalsImportMode.InsertAndUpdate || journalsImportMode == JournalsImportMode.InsertOnly;
        }

        private static bool ShouldUpdateJournals(JournalsImportMode journalsImportMode)
        {
            return journalsImportMode == JournalsImportMode.InsertAndUpdate || journalsImportMode == JournalsImportMode.UpdateOnly;
        }
    }
}