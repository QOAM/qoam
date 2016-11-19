using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Text.RegularExpressions;
using QOAM.Core.Import.SubmissionLinks;
using QOAM.Website.ViewModels.Admin;

namespace QOAM.Website.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using QOAM.Core;
    using QOAM.Core.Export;
    using QOAM.Core.Helpers;
    using QOAM.Core.Import;
    using QOAM.Core.Repositories;
    using QOAM.Website.Helpers;
    using QOAM.Website.Models;
    using QOAM.Website.ViewModels.Import;
    using QOAM.Website.ViewModels.Journals;
    using QOAM.Website.ViewModels.Institutions;

    using Validation;

    [RequireHttps]
    [RoutePrefix("admin")]
    [Authorize(Roles = ApplicationRole.Admin + "," + ApplicationRole.DataAdmin + "," + ApplicationRole.InstitutionAdmin)]
    public class AdminController : ApplicationController
    {
        private const string FoundISSNsSessionKey = "FoundISSNs";
        private const string NotFoundISSNsSessionKey = "NotFoundISSNs";
        private const int BlockedIssnsCount = 20;

        private readonly JournalsImport journalsImport;
        private readonly UlrichsImport ulrichsImport;
        private readonly DoajImport doajImport;
        private readonly IJournalRepository journalRepository;
        private readonly JournalsExport journalsExport;
        private readonly IInstitutionRepository institutionRepository;
        private readonly IBlockedISSNRepository blockedIssnRepository;

        readonly IBulkImporter<SubmissionPageLink> _bulkImporter;
        readonly IBulkImporter<Institution> _institutionImporter;
        readonly Regex _domainRegex = new Regex(@"(?<=(http[s]?:\/\/(.*?)[.?]))\b([a-z0-9]+(-[a-z0-9]+)*\.)+[a-z]{2,63}\b", RegexOptions.Compiled);
        readonly JournalTocsImport _journalsTocImport;

        public AdminController(JournalsImport journalsImport, UlrichsImport ulrichsImport, DoajImport doajImport, JournalTocsImport journalsTocImport, JournalsExport journalsExport, IJournalRepository journalRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication, IInstitutionRepository institutionRepository, IBlockedISSNRepository blockedIssnRepository, IBaseScoreCardRepository baseScoreCardRepository, IValuationScoreCardRepository valuationScoreCardRepository, IBulkImporter<SubmissionPageLink> bulkImporter, IBulkImporter<Institution> institutionImporter)
            : base(baseScoreCardRepository, valuationScoreCardRepository, userProfileRepository, authentication)
        {
            Requires.NotNull(journalsImport, nameof(journalsImport));
            Requires.NotNull(ulrichsImport, nameof(ulrichsImport));
            Requires.NotNull(journalsTocImport, nameof(journalsTocImport));
            Requires.NotNull(doajImport, nameof(doajImport));
            Requires.NotNull(journalsExport, nameof(journalsExport));
            Requires.NotNull(journalRepository, nameof(journalRepository));
            Requires.NotNull(institutionRepository, nameof(institutionRepository));
            Requires.NotNull(blockedIssnRepository, nameof(blockedIssnRepository));
            Requires.NotNull(bulkImporter, nameof(bulkImporter));
            Requires.NotNull(institutionImporter, nameof(institutionImporter));
            
            this.journalsImport = journalsImport;
            this.ulrichsImport = ulrichsImport;
            this.doajImport = doajImport;
            _journalsTocImport = journalsTocImport;
            this.journalsExport = journalsExport;
            this.journalRepository = journalRepository;
            this.institutionRepository = institutionRepository;
            this.blockedIssnRepository = blockedIssnRepository;

            _bulkImporter = bulkImporter;
            _institutionImporter = institutionImporter;
        }

        [HttpGet, Route("")]
        public ViewResult Index()
        {
            return this.View();
        }

        [HttpGet, Route("import")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ViewResult Import()
        {
            return this.View(new ImportViewModel());
        }

        [HttpPost, Route("import")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ActionResult Import(ImportViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var journals = this.GetJournalsFromSource(model.Source);
                var journalsISSNs = journals.Select(j => j.ISSN).ToSet(StringComparer.InvariantCultureIgnoreCase);

                var issns = GetISSNs(model);
                var issnsFound = issns.Intersect(journalsISSNs).ToList();
                var issnsNotFound = issns.Except(journalsISSNs).ToList();

                var journalsToImport = journals.Where(j => issnsFound.Contains(j.ISSN)).ToList();
                this.journalsImport.ImportJournals(journalsToImport, JournalsImportMode.InsertOnly);

                this.Session[FoundISSNsSessionKey] = issnsFound;
                this.Session[NotFoundISSNsSessionKey] = issnsNotFound;

                return this.RedirectToAction("Imported");
            }

            return this.View(model);
        }

        [HttpGet, Route("imported")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ViewResult Imported()
        {
            var model = new ImportedViewModel
                        {
                            FoundISSNs = (IEnumerable<string>)this.Session[FoundISSNsSessionKey],
                            NotFoundISSNs = (IEnumerable<string>)this.Session[NotFoundISSNsSessionKey]
                        };

            return this.View(model);
        }

        [HttpGet, Route("update")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ViewResult Update()
        {
            return this.View(new UpdateViewModel());
        }

        [HttpPost, Route("update")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ActionResult Update(UpdateViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var journals = this.GetJournalsFromSource(model.Source);
                var issns = GetISSNs(model);

                var journalsISSNs = journals.Select(j => j.ISSN).ToSet(StringComparer.InvariantCultureIgnoreCase);
                var issnsFound = issns.Intersect(journalsISSNs).ToList();
                var issnsNotFound = issns.Except(journalsISSNs).ToList();

                var journalsToImport = journals.Where(j => issnsFound.Contains(j.ISSN)).ToList();
                this.journalsImport.ImportJournals(journalsToImport, JournalsImportMode.UpdateOnly);

                this.Session[FoundISSNsSessionKey] = issnsFound;
                this.Session[NotFoundISSNsSessionKey] = issnsNotFound;

                return this.RedirectToAction("Updated");
            }

            return this.View(model);
        }

        [HttpGet, Route("updated")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ViewResult Updated()
        {
            var model = new UpdatedViewModel
                {
                    FoundISSNs = (IEnumerable<string>)this.Session[FoundISSNsSessionKey],
                    NotFoundISSNs = (IEnumerable<string>)this.Session[NotFoundISSNsSessionKey]
                };

            return this.View(model);
        }

        [HttpGet, Route("download")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public FileContentResult Download(string type)
        {
            using (var memoryStream = new MemoryStream())
            {
                if (type == "all")
                    journalsExport.ExportAllJournals(memoryStream);
                else
                    journalsExport.ExportOpenAccessJournals(memoryStream);

                return this.File(memoryStream.ToArray(), "application/csv", "journals.csv");
            }
        }

        [HttpGet, Route("delete")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ViewResult Delete()
        {
            return this.View(new DeleteViewModel());
        }

        [HttpPost, Route("delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ActionResult Delete(DeleteViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var journals = this.journalRepository.All;
                var issns = GetISSNs(model);

                var journalsISSNs = journals.Select(j => j.ISSN).ToSet(StringComparer.InvariantCultureIgnoreCase);
                var issnsFound = issns.Intersect(journalsISSNs).ToList();
                var issnsNotFound = issns.Except(journalsISSNs).ToList();

                var journalsToDelete = journals.Where(j => issnsFound.Contains(j.ISSN)).ToList();
                foreach (var journal in journalsToDelete)
                {
                    this.journalRepository.Delete(journal);
                }

                this.journalRepository.Save();

                this.Session[FoundISSNsSessionKey] = issnsFound;
                this.Session[NotFoundISSNsSessionKey] = issnsNotFound;

                return this.RedirectToAction("Deleted");
            }

            return this.View(model);
        }

        [HttpGet, Route("deleted")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ViewResult Deleted()
        {
            var model = new DeletedViewModel
            {
                FoundISSNs = (IEnumerable<string>)this.Session[FoundISSNsSessionKey],
                NotFoundISSNs = (IEnumerable<string>)this.Session[NotFoundISSNsSessionKey]
            };

            return this.View(model);
        }

        [HttpGet, Route("check")]
        [Authorize(Roles = ApplicationRole.Admin + "," + ApplicationRole.DataAdmin)]
        public ViewResult Check()
        {
            return this.View();
        }

        [HttpPost, Route("check")]
        [Authorize(Roles = ApplicationRole.Admin + "," + ApplicationRole.DataAdmin)]
        public ViewResult Check(CheckViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var parsedIssns = ParseISSNs(model.ISSNs);
                model.FoundISSNs = this.journalRepository.SearchByISSN(parsedIssns).Select(j => j.ISSN).ToSet();
                model.NotFoundISSNs = parsedIssns.Except(model.FoundISSNs).ToSet();
            }

            return this.View(model);
        }

        [HttpGet, Route("blockissn")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ViewResult BlockIssn()
        {
            var model = new BlockIssnViewModel { BlockedIssns = blockedIssnRepository.All };

            return this.View(model);
        }

        [HttpPost, Route("blockissn")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ActionResult BlockIssn(BlockIssnViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                foreach (var issn in ParseISSNs(model.ISSN))
                {
                    if (!blockedIssnRepository.Exists(issn))
                    {
                        this.blockedIssnRepository.InsertOrUpdate(new BlockedISSN { ISSN = issn });
                    }
                }

                this.blockedIssnRepository.Save();

                return this.RedirectToAction("BlockIssn");
            }

            //model.BlockedIssns = blockedIssnRepository.All.Take(BlockedIssnsCount);

            return this.View(model);
        }

        [HttpGet, Route("removeblockedissn")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ViewResult RemoveBlockedIssn()
        {
            var model = new RemoveBlockedIssnViewModel { BlockedIssns = blockedIssnRepository.All };
            return this.View(model);
        }

        [HttpPost, Route("removeblockedissn")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ActionResult RemoveBlockedIssn(RemoveBlockedIssnViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                foreach (var item in model.SelectedItems)
                {
                    blockedIssnRepository.Delete(blockedIssnRepository.Find(item));
                }

                blockedIssnRepository.Save();
                
                return this.RedirectToAction("RemoveBlockedIssn");
            }

            model.BlockedIssns = blockedIssnRepository.All;

            return this.View(model);
        }
        
        [HttpGet, Route("addinstitution")]
        [Authorize(Roles = ApplicationRole.Admin + "," + ApplicationRole.InstitutionAdmin)]
        public ViewResult AddInstitution()
        {
            return this.View();
        }

        [HttpPost, Route("addinstitution")]
        [Authorize(Roles = ApplicationRole.Admin + "," + ApplicationRole.InstitutionAdmin)]
        public ActionResult AddInstitution(UpsertViewModel model)
        {
            ModelState["File"].Errors.Clear();
            
            if (institutionRepository.Exists(model.Name))
                ModelState.AddModelError("Name", $"There is already an institution with the name \"{model.Name}\".");

            if (institutionRepository.DomainExists(model.ShortName))
                ModelState.AddModelError("ShortName", $"There is already an institution with the domain \"{model.ShortName}\".");

            if (!ModelState.IsValid)
                return View(model);

            institutionRepository.InsertOrUpdate(model.ToInstitution());
            institutionRepository.Save();

            return View("AddedInstitution", new InstitutionsAddedViewModel
            {
                AmountImported = 1
            });
        }

        [HttpPost, Route("bulkaddinstitution")]
        [Authorize(Roles = ApplicationRole.Admin + "," + ApplicationRole.InstitutionAdmin)]
        public ActionResult BulkAddInstitution(UpsertViewModel model)
        {
            if (model.File == null)
                return View("AddInstitution", model);

            try
            {
                var data = _institutionImporter.Execute(model.File.InputStream);

                List<Institution> invalidRecords = new List<Institution>(), existingNames = new List<Institution>(), existingDomains = new List<Institution>();
                var imported = 0;

                foreach (var institution in data)
                {
                    if (string.IsNullOrWhiteSpace(institution.Name) || string.IsNullOrWhiteSpace(institution.ShortName))
                    {
                        invalidRecords.Add(institution);
                        continue;
                    }

                    if (institutionRepository.Exists(institution.Name))
                    {
                        existingNames.Add(institution);
                        continue;
                    }

                    if (institutionRepository.DomainExists(institution.ShortName))
                    {
                        existingDomains.Add(institution);
                        continue;
                    }

                    institutionRepository.InsertOrUpdate(institution);
                    institutionRepository.Save();

                    imported++;
                }

                return View("AddedInstitution", new InstitutionsAddedViewModel
                {
                    AmountImported = imported,
                    Invalid = invalidRecords,
                    ExistingNames = existingNames,
                    ExistingDomains = existingDomains
                });
            }
            catch (ArgumentException invalidFileException)
            {
                ModelState.AddModelError("generalError", invalidFileException.Message);
                return View("AddInstitution", model);
            }
        }

        [HttpGet, Route("{id:int}/editinstitution")]
        [Authorize(Roles = ApplicationRole.Admin + "," + ApplicationRole.InstitutionAdmin)]
        public ActionResult EditInstitution(int id)
        {
            return FetchUpsertViewModel(id);
        }

        [HttpPost, Route("{id:int}/editinstitution")]
        [Authorize(Roles = ApplicationRole.Admin + "," + ApplicationRole.InstitutionAdmin)]
        [ValidateAntiForgeryToken]
        public ActionResult EditInstitution(UpsertViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.ShortName))
                return View(model);

            institutionRepository.InsertOrUpdate(model.ToInstitution());
            institutionRepository.Save();

            return RedirectToAction("Index", "Institutions");
        }

        [HttpGet, Route("{id:int}/deleteinstitution")]
        [Authorize(Roles = ApplicationRole.Admin + "," + ApplicationRole.InstitutionAdmin)]
        public ActionResult DeleteInstitution(int id)
        {
            return FetchUpsertViewModel(id);
        }

        [HttpPost, Route("{id:int}/deleteinstitution")]
        [Authorize(Roles = ApplicationRole.Admin + "," + ApplicationRole.InstitutionAdmin)]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteInstitution(UpsertViewModel model)
        {
            if (!model.Id.HasValue)
                return new HttpNotFoundResult();

            var institution = institutionRepository.Find(model.Id.Value);

            if (institution == null)
                return new HttpNotFoundResult();

            if (institution.UserProfiles.Any())
            {
                ModelState["File"].Errors.Clear();
                ModelState.AddModelError("noetempty", $"There are {institution.UserProfiles.Count} users registered under this domain. Institution {institution.Name} cannot be deleted.");
                return View("DeleteInstitution", model);
            }


            institutionRepository.Delete(institution);
            institutionRepository.Save();

            return RedirectToAction("InstitutionDeleted");
        }

        [HttpGet, Route("insitutiondeleted")]
        [Authorize(Roles = ApplicationRole.Admin + "," + ApplicationRole.InstitutionAdmin)]
        public ViewResult InstitutionDeleted()
        {
            return View();
        }

        [HttpGet, Route("movescorecards")]
        [Authorize(Roles = ApplicationRole.Admin)]
        public ViewResult MoveScoreCards(bool? saveSuccessful = null)
        {
            ViewBag.SaveSuccessful = saveSuccessful;

            return View();
        }

        [HttpPost, Route("movescorecards")]
        [Authorize(Roles = ApplicationRole.Admin)]
        [ValidateAntiForgeryToken]
        public ActionResult MoveScoreCards(MoveScoreCardsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var oldJournal = journalRepository.FindByIssn(model.OldIssn);
                var newJournal = journalRepository.FindByIssn(model.NewIssn);

                if (oldJournal != null && newJournal != null)
                {
                    baseScoreCardRepository.MoveScoreCards(oldJournal, newJournal);
                    valuationScoreCardRepository.MoveScoreCards(oldJournal, newJournal);

                    return RedirectToAction("MoveScoreCards", new { saveSuccessful = true });
                }

                if (oldJournal == null)
                {
                    ModelState.AddModelError(nameof(model.OldIssn), "ISSN does not exist.");
                }

                if (newJournal == null)
                {
                    ModelState.AddModelError(nameof(model.NewIssn), "ISSN does not exist.");
                }
            }

            return View();
        }

        [HttpGet, Route("removebasescorecard/{id:int}")]
        [Authorize(Roles = ApplicationRole.Admin)]
        public ActionResult RemoveBaseScoreCard(RemoveBaseScoreCardViewModel model)
        {
            var baseScoreCard = baseScoreCardRepository.Find(model.Id);

            if (baseScoreCard == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost, Route("removebasescorecard/{id:int}")]
        [Authorize(Roles = ApplicationRole.Admin)]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveBaseScoreCardPost(RemoveBaseScoreCardViewModel model)
        {
            var baseScoreCard = baseScoreCardRepository.Find(model.Id);

            if (baseScoreCard == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                baseScoreCardRepository.Delete(baseScoreCard);
                baseScoreCardRepository.Save();

                return RedirectToAction("RemovedBaseScoreCard");
            }

            return View("RemoveBaseScoreCard", model);
        }

        [HttpGet, Route("removedbasescorecard")]
        [Authorize(Roles = ApplicationRole.Admin)]
        public ViewResult RemovedBaseScoreCard()
        {
            return View();
        }

        [HttpGet, Route("removevaluationscorecard/{id:int}")]
        [Authorize(Roles = ApplicationRole.Admin)]
        public ActionResult RemoveValuationScoreCard(RemoveValuationScoreCardViewModel model)
        {
            var valuationScoreCard = valuationScoreCardRepository.Find(model.Id);

            if (valuationScoreCard == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost, Route("removevaluationscorecard/{id:int}")]
        [Authorize(Roles = ApplicationRole.Admin)]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveValuationScoreCardPost(RemoveValuationScoreCardViewModel model)
        {
            var valuationScoreCard = valuationScoreCardRepository.Find(model.Id);

            if (valuationScoreCard == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                valuationScoreCardRepository.Delete(valuationScoreCard);
                valuationScoreCardRepository.Save();

                return RedirectToAction("RemovedValuationScoreCard");
            }

            return View("RemoveValuationScoreCard", model);
        }

        [HttpGet, Route("removedvaluationscorecard")]
        [Authorize(Roles = ApplicationRole.Admin)]
        public ViewResult RemovedValuationScoreCard()
        {
            return View();
        }

        [HttpGet, Route("submissionlinks")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ActionResult ImportSubmissionLinks()
        {
            return View(new ImportSubmissionLinksViewModel());
        }

        [HttpPost, Route("submissionlinks")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        [ValidateAntiForgeryToken]
        public ActionResult ImportSubmissionLinks(ImportSubmissionLinksViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                int imported = 0, rejected = 0;
                var rejectedUrls = new List<SubmissionPageLink>();

                var data = _bulkImporter.Execute(model.File.InputStream);

                foreach (var submissionPageLink in data)
                {
                    var journal = journalRepository.FindByIssn(submissionPageLink.ISSN);

                    if (journal == null || !DomainMatches(journal.Link, submissionPageLink.Url))
                    {
                        rejected++;

                        rejectedUrls.Add(submissionPageLink);
                        continue;
                    }

                    journal.SubmissionPageLink = submissionPageLink.Url;
                    imported++;
                }

                journalRepository.Save();

                return View("SubmissionLinksImportSuccessful", new SubmissionLinksImportedViewModel
                {
                    AmountImported = imported,
                    AmountRejected = rejected,
                    RejectedUrls = rejectedUrls
                });
            }
            catch (ArgumentException invalidFileException)
            {
                ModelState.AddModelError("generalError", invalidFileException.Message);
            }
            catch (DbEntityValidationException)
            {
                //foreach (var eve in e.EntityValidationErrors)
                //{
                //    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                //    }
                //}
                //throw;
            }
            catch (Exception exception)
            {
                while (exception.InnerException != null)
                    exception = exception.InnerException;

                ModelState.AddModelError("generalError", $"An error has ocurred: {exception.Message}");
            }

            return View(model);
        }

        [HttpGet, Route("statistics")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin + "," + ApplicationRole.InstitutionAdmin)]
        public ActionResult Statistics()
        {
            var model = new StatisticsViewModel
            {
                BaseScoreCardCount = journalRepository.BaseScoredJournalsCount(),
                ValuationScoreCardCount = journalRepository.ValuationScoredJournalsCount(),
                SwotCount = journalRepository.JournalsWithSwotCount()
            };

            return View(model);
        }

        #region Private Methods

        private static HashSet<string> GetISSNs(ImportViewModel model)
        {
            return ParseISSNs(model.ISSNs);
        }

        private static HashSet<string> GetISSNs(UpdateViewModel model)
        {
            return ParseISSNs(model.ISSNs);
        }

        private static HashSet<string> GetISSNs(DeleteViewModel model)
        {
            return ParseISSNs(model.ISSNs);
        }

        private static HashSet<string> ParseISSNs(string issns)
        {
            return issns.ToLinesSet();
        }

        private IList<Journal> GetJournalsFromSource(JournalsImportSource importSource)
        {
            switch (importSource)
            {
                case JournalsImportSource.DOAJ:
                    return this.doajImport.GetJournals();
                case JournalsImportSource.Ulrichs:
                    return this.ulrichsImport.GetJournals(UlrichsImport.UlrichsJournalType.All);
                    case JournalsImportSource.JournalTOCs:
                    return _journalsTocImport.DownloadJournals();
                default:
                    throw new ArgumentOutOfRangeException(nameof(importSource));
            }
        }

        ActionResult FetchUpsertViewModel(int id)
        {
            var institution = institutionRepository.Find(id);

            if(institution == null)
                return new HttpNotFoundResult();

            var model = new UpsertViewModel
            {
                Id = id,
                Name = institution.Name,
                ShortName = institution.ShortName
            };

            return View(model);
        }

        bool DomainMatches(string journalLink, string submissionPageLink)
        {
            var submissionLinkDomain = _domainRegex.Match(submissionPageLink).Value;
            var noSubdomainRegex = new Regex($@"(?<=(http[s]?:\/\/))({submissionLinkDomain})");

            if ((submissionLinkDomain != _domainRegex.Match(journalLink).Value))
                return submissionLinkDomain == noSubdomainRegex.Match(journalLink).Value;

            return true;
        }

        #endregion
    }
}