namespace QOAM.Website.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;

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
    using QOAM.Website.ViewModels.Institutions;

    using Validation;

    [RoutePrefix("admin")]
    [Authorize(Roles = ApplicationRole.Admin + "," + ApplicationRole.DataAdmin + "," + ApplicationRole.InstitutionAdmin)]
    public class AdminController : ApplicationController
    {
        private const string FoundISSNsSessionKey = "FoundISSNs";
        private const string NotFoundISSNsSessionKey = "NotFoundISSNs";

        private readonly JournalsImport journalsImport;
        private readonly UlrichsImport ulrichsImport;
        private readonly DoajImport doajImport;
        private readonly IJournalRepository journalRepository;
        private readonly JournalsExport journalsExport;
        private readonly IInstitutionRepository institutionRepository;

        public AdminController(JournalsImport journalsImport, UlrichsImport ulrichsImport, DoajImport doajImport, JournalsExport journalsExport, IJournalRepository journalRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication, IInstitutionRepository institutionRepository)
            : base(userProfileRepository, authentication)
        {
            Requires.NotNull(journalsImport, "journalsImport");
            Requires.NotNull(ulrichsImport, "ulrichsImport");
            Requires.NotNull(doajImport, "doajImport");
            Requires.NotNull(journalsExport, "journalsExport");
            Requires.NotNull(journalRepository, "journalRepository");
            Requires.NotNull(institutionRepository, "institutionRepository");

            this.journalsImport = journalsImport;
            this.ulrichsImport = ulrichsImport;
            this.doajImport = doajImport;
            this.journalsExport = journalsExport;
            this.journalRepository = journalRepository;
            this.institutionRepository = institutionRepository;
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

        [GET("addinstitution")]
        [Authorize(Roles = ApplicationRole.Admin)]
        public ViewResult AddInstitution()
        {
            return this.View();
        }

        [POST("addinstitution")]
        [Authorize(Roles = ApplicationRole.Admin)]
        public ActionResult AddInstitution(AddViewModel model)
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
        [Authorize(Roles = ApplicationRole.Admin)]
        public ViewResult AddedInstitution()
        {
            return this.View();
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
            return issns.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Where(s => s.Trim().Length > 0).Select(s => s.Trim()).ToSet(StringComparer.InvariantCultureIgnoreCase);
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