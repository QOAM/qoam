using Validation;
namespace QOAM.Website.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using System.Globalization;

    using AttributeRouting;
    using AttributeRouting.Web.Mvc;

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
        private readonly IBaseScoreCardRepository baseScoreCardRepository;
        private readonly IValuationScoreCardRepository valuationScoreCardRepository;

        public AdminController(JournalsImport journalsImport, UlrichsImport ulrichsImport, DoajImport doajImport, JournalsExport journalsExport, IJournalRepository journalRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication, IInstitutionRepository institutionRepository, IBlockedISSNRepository blockedIssnRepository, IBaseScoreCardRepository baseScoreCardRepository, IValuationScoreCardRepository valuationScoreCardRepository)
            : base(userProfileRepository, authentication)
        {
            Requires.NotNull(journalsImport, "journalsImport");
            Requires.NotNull(ulrichsImport, "ulrichsImport");
            Requires.NotNull(doajImport, "doajImport");
            Requires.NotNull(journalsExport, "journalsExport");
            Requires.NotNull(journalRepository, "journalRepository");
            Requires.NotNull(institutionRepository, "institutionRepository");
            Requires.NotNull(blockedIssnRepository, "blockedIssnRepository");
            Requires.NotNull(baseScoreCardRepository, "baseScoreCardRepository");
            Requires.NotNull(valuationScoreCardRepository, "valuationScoreCardRepository");
            
            this.journalsImport = journalsImport;
            this.ulrichsImport = ulrichsImport;
            this.doajImport = doajImport;
            this.journalsExport = journalsExport;
            this.journalRepository = journalRepository;
            this.institutionRepository = institutionRepository;
            this.blockedIssnRepository = blockedIssnRepository;
            this.valuationScoreCardRepository = valuationScoreCardRepository;
            this.baseScoreCardRepository = baseScoreCardRepository;
        }

        [GET("")]
        public ViewResult Index()
        {
            return this.View();
        }

        [GET("import")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ViewResult Import()
        {
            return this.View(new ImportViewModel());
        }

        [POST("import")]
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

        [GET("imported")]
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

        [GET("update")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ViewResult Update()
        {
            return this.View(new UpdateViewModel());
        }

        [POST("update")]
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

        [GET("updated")]
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

        [GET("download")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public FileContentResult Download()
        {
            using (var memoryStream = new MemoryStream())
            {
                this.journalsExport.ExportAllJournals(memoryStream);
                
                return this.File(memoryStream.ToArray(), "application/csv", "journals.csv");
            }
        }

        [GET("delete")]
        [Authorize(Roles = ApplicationRole.Admin)]
        public ViewResult Delete()
        {
            return this.View(new DeleteViewModel());
        }

        [POST("delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRole.Admin)]
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

        [GET("deleted")]
        [Authorize(Roles = ApplicationRole.Admin)]
        public ViewResult Deleted()
        {
            var model = new DeletedViewModel
            {
                FoundISSNs = (IEnumerable<string>)this.Session[FoundISSNsSessionKey],
                NotFoundISSNs = (IEnumerable<string>)this.Session[NotFoundISSNsSessionKey]
            };

            return this.View(model);
        }

        [GET("check")]
        [Authorize(Roles = ApplicationRole.InstitutionAdmin + "," + ApplicationRole.Admin)]
        public ViewResult Check()
        {
            return this.View();
        }

        [POST("check")]
        [Authorize(Roles = ApplicationRole.InstitutionAdmin + "," + ApplicationRole.Admin)]
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

        [GET("blockissn")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ViewResult BlockIssn()
        {
            var model = new BlockIssnViewModel { BlockedIssns = blockedIssnRepository.All.Take(BlockedIssnsCount) };

            return this.View(model);
        }

        [POST("blockissn")]
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

            model.BlockedIssns = blockedIssnRepository.All.Take(BlockedIssnsCount);

            return this.View(model);
        }

        [GET("removeblockedissn")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ViewResult RemoveBlockedIssn()
        {
            var model = new RemoveBlockedIssnViewModel { BlockedIssns = blockedIssnRepository.All.Take(BlockedIssnsCount) };
            return this.View(model);
        }

        [POST("removeblockedissn")]
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

            model.BlockedIssns = blockedIssnRepository.All.Take(BlockedIssnsCount);

            return this.View(model);
        }
        
        [GET("addinstitution")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ViewResult AddInstitution()
        {
            return this.View();
        }

        [POST("addinstitution")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ActionResult AddInstitution(UpsertViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                this.institutionRepository.InsertOrUpdate(model.ToInstitution());
                this.institutionRepository.Save();
                
                return this.RedirectToAction("AddedInstitution");
            }

            return this.View(model);
        }

        [GET("addedinstitution")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ViewResult AddedInstitution()
        {
            return this.View();
        }

        [GET("{id:int}/editinstitution")]
        [Authorize(Roles = ApplicationRole.Admin)]
        public ViewResult EditInstitution(int id)
        {
            var institution = institutionRepository.Find(id);
            var model = new UpsertViewModel
            {
                Name = institution.Name,
                ShortName = institution.ShortName
            };

            return View(model);
        }

        [POST("{id:int}/editinstitution")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        [ValidateAntiForgeryToken]
        public ActionResult EditInstitution(UpsertViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            institutionRepository.InsertOrUpdate(model.ToInstitution());
            institutionRepository.Save();

            return RedirectToAction("Index", "Institutions");
        }

        [GET("movescorecards")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        public ViewResult MoveScoreCards(bool? saveSuccessful = null)
        {
            ViewBag.SaveSuccessful = saveSuccessful;

            return View();
        }

        [POST("movescorecards")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
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
                default:
                    throw new ArgumentOutOfRangeException("importSource");
            }
        }
    }
}