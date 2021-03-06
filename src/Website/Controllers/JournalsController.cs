﻿using System.Data.Entity.Validation;
using QOAM.Core.Helpers;
using QOAM.Core.Import;
using QOAM.Core.Import.SubmissionLinks;
using QOAM.Website.ViewModels.Admin;

namespace QOAM.Website.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Core;
    using Core.Import.Licences;
    using Core.Repositories;
    using Core.Repositories.Filters;
    using Helpers;
    using Models;
    using Validation;
    using ViewModels.Journals;

    [RoutePrefix("journals")]
    public class JournalsController : ApplicationController
    {
        const int SubjectTruncationLength = 90;

        readonly IJournalRepository journalRepository;
        readonly IBaseJournalPriceRepository baseJournalPriceRepository;
        readonly IInstitutionJournalRepository institutionJournalRepository;
        readonly IValuationJournalPriceRepository valuationJournalPriceRepository;
        readonly IInstitutionRepository institutionRepository;
        readonly IBulkImporter<UniversityLicense> _bulkImporter;
        readonly ISubjectRepository _subjectRepository;
        readonly IBulkImporter<JournalRelatedLink> _journalRelatedLinkBulkImporter;

        public JournalsController(IJournalRepository journalRepository, IBaseJournalPriceRepository baseJournalPriceRepository, IValuationJournalPriceRepository valuationJournalPriceRepository,
            IValuationScoreCardRepository valuationScoreCardRepository, ILanguageRepository languageRepository,
            IInstitutionJournalRepository institutionJournalRepository, IBaseScoreCardRepository baseScoreCardRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication,
            IInstitutionRepository institutionRepository, IBulkImporter<UniversityLicense> bulkImporter, ISubjectRepository subjectRepository,
            IBulkImporter<JournalRelatedLink> journalRelatedLinkBulkImporter)
            : base(baseScoreCardRepository, valuationScoreCardRepository, userProfileRepository, authentication)
        {
            Requires.NotNull(journalRepository, nameof(journalRepository));
            Requires.NotNull(baseJournalPriceRepository, nameof(baseJournalPriceRepository));
            Requires.NotNull(valuationJournalPriceRepository, nameof(valuationJournalPriceRepository));
            Requires.NotNull(languageRepository, nameof(languageRepository));
            Requires.NotNull(institutionJournalRepository, nameof(institutionJournalRepository));
            Requires.NotNull(institutionRepository, nameof(institutionRepository));
            Requires.NotNull(valuationJournalPriceRepository, nameof(valuationJournalPriceRepository));
            Requires.NotNull(bulkImporter, nameof(bulkImporter));

            this.journalRepository = journalRepository;
            this.baseJournalPriceRepository = baseJournalPriceRepository;
            this.institutionJournalRepository = institutionJournalRepository;
            this.institutionRepository = institutionRepository;
            this.valuationJournalPriceRepository = valuationJournalPriceRepository;
            _bulkImporter = bulkImporter;
            _subjectRepository = subjectRepository;
            _journalRelatedLinkBulkImporter = journalRelatedLinkBulkImporter;
        }

        [HttpGet, Route("")]
        public ViewResult Index(IndexViewModel model)
        {
            model.Disciplines = _subjectRepository.Active.Where(s => !string.IsNullOrWhiteSpace(s.Name)).ToList().ToSelectListItems("Search by discipline"); //NormalizeSearchStrings(model.Disciplines);
            model.Languages = NormalizeSearchStrings(model.Languages);
            model.Journals = journalRepository.Search(model.ToFilter());

            return View("JournalsIndex", model);
        }

        [HttpGet, Route("for-institution/{institutionId:int}")]
        public ViewResult JournalsForInstitution(int institutionId, JournalsForInstitutionViewModel model)
        {
            model.Disciplines = _subjectRepository.Active.Where(s => !string.IsNullOrWhiteSpace(s.Name)).ToList().ToSelectListItems("Search by discipline"); //NormalizeSearchStrings(model.Disciplines);
            model.Languages = NormalizeSearchStrings(model.Languages);
            model.Journals = journalRepository.Search(model.ToFilter());
            model.Institution = institutionRepository.Find(institutionId);
            model.InstitutionJournalIds = institutionJournalRepository
                .FindAll(new InstitutionJournalFilter { InstitutionId = institutionId, AssociatedInstitutionIds = model.Institution.CorrespondingInstitutionIds })
                .Select(ij => ij.JournalId)
                .ToList();
            model.OpenAccessJournalIds = model.Journals.Where(j => j.OpenAccess).Select(j => j.Id).ToList();

            return View(model);
        }

        [HttpGet, Route("{id:int}")]
        public ActionResult Details(int id)
        {
            var journal = journalRepository.Find(id);

            if (journal == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReturnUrl = HttpContext.Request.UrlReferrer?.ToString();

            object saved;

            if (TempData.TryGetValue("MyQoamMessage", out saved))
                ViewBag.MyQoamMessage = saved.ToString();

            return View(journal);
        }

        [HttpGet, Route("{id:int}/prices")]
        public PartialViewResult Prices(PricesViewModel model)
        {
            ViewBag.RefererUrl = model.RefererUrl;

            model.InstitutionJournals = institutionJournalRepository.FindAll(model.ToInstitutionJournalPriceFilter());
            model.BaseJournalPrices = baseJournalPriceRepository.Find(model.ToJournalPriceFilter(null));
            model.ValuationJournalPrices = valuationJournalPriceRepository.Find(model.ToJournalPriceFilter(FeeType.Article));
            model.Journal = journalRepository.Find(model.Id);

            return PartialView(model);
        }

        [HttpGet, Route("{id:int}/basejournalprices")]
        public PartialViewResult BaseJournalPrices(PricesViewModel model)
        {
            ViewBag.RefererUrl = model.RefererUrl;

            var journalPrices = baseJournalPriceRepository.Find(model.ToJournalPriceFilter(null));

            return PartialView(journalPrices);
        }

        [HttpGet, Route("{id:int}/valuationjournalprices")]
        public PartialViewResult ValuationJournalPrices(PricesViewModel model)
        {
            ViewBag.RefererUrl = model.RefererUrl;

            var journalPrices = valuationJournalPriceRepository.Find(model.ToJournalPriceFilter(FeeType.Article));

            return PartialView(journalPrices);
        }

        [HttpGet, Route("{id:int}/institutionalprices")]
        public PartialViewResult InstitutionJournalPrices(PricesViewModel model)
        {
            ViewBag.RefererUrl = model.RefererUrl;

            var institutionJournals = institutionJournalRepository.Find(model.ToInstitutionJournalPriceFilter()).ToList();

            return PartialView(institutionJournals);
        }

        [HttpGet, Route("{id:int}/scorecards")]
        public PartialViewResult ScoreCards(ScoreCardsViewModel model)
        {
            model.BaseScoreCards = baseScoreCardRepository.Find(model.ToFilter());
            model.ValuationScoreCards = valuationScoreCardRepository.Find(model.ToFilter());

            return PartialView(model);
        }

        [HttpGet, Route("{id:int}/basescorecards")]
        public PartialViewResult BaseScoreCards(ScoreCardsViewModel model)
        {
            model.BaseScoreCards = baseScoreCardRepository.Find(model.ToFilter());

            return PartialView(model);
        }

        [HttpGet, Route("{id:int}/valuationscorecards")]
        public PartialViewResult ValuationScoreCards(ScoreCardsViewModel model)
        {
            model.ValuationScoreCards = valuationScoreCardRepository.Find(model.ToFilter());

            return PartialView(model);
        }

        [HttpGet, Route("{id:int}/comments")]
        public PartialViewResult Comments(CommentsViewModel model)
        {
            model.CommentedBaseScoreCards = baseScoreCardRepository.Find(model.ToFilter());
            model.CommentedValuationScoreCards = valuationScoreCardRepository.Find(model.ToFilter());

            return PartialView(model);
        }

        [HttpGet, Route("{id:int}/basescorecardcomments")]
        public PartialViewResult BaseScoreCardComments(CommentsViewModel model)
        {
            model.CommentedBaseScoreCards = baseScoreCardRepository.Find(model.ToFilter());

            return PartialView(model);
        }

        [HttpGet, Route("{id:int}/valuationscorecardcomments")]
        public PartialViewResult ValuationScoreCardComments(CommentsViewModel model)
        {
            model.CommentedValuationScoreCards = valuationScoreCardRepository.Find(model.ToFilter());

            return PartialView(model);
        }

        [HttpGet, Route("{id:int}/institutionjournallicense")]
        [Authorize(Roles = ApplicationRole.InstitutionAdmin + "," + ApplicationRole.Admin)]
        public ViewResult InstitutionJournalLicense(int id, int institutionId, string refererUrl)
        {
            var model = new InstitutionJournalLicenseViewModel(
                journalRepository.Find(id),
                institutionJournalRepository.Find(id, institutionId),
                refererUrl,
                institutionRepository.All.ToSelectListItems("<Select institution>"));

            return View(model);
        }

        [HttpPost, Route("{id:int}/institutionjournallicense")]
        [Authorize(Roles = ApplicationRole.InstitutionAdmin + "," + ApplicationRole.Admin)]
        [ValidateAntiForgeryToken]
        public ActionResult InstitutionJournalLicense(int id, InstitutionJournalLicenseViewModel model)
        {
            // Ensure that only admin and institutional admin users can use the update all journals of a publisher
            if (model.UpdateAllJournalsOfPublisher && (!User.IsInRole(ApplicationRole.Admin) && !User.IsInRole(ApplicationRole.InstitutionAdmin)))
            {
                return new HttpUnauthorizedResult();
            }

            var journal = journalRepository.Find(id);

            if (ModelState.IsValid)
            {
                var institutionJournalsToModify = new List<InstitutionJournal>();

                if (model.UpdateAllJournalsOfPublisher)
                {
                    var institutionJournals = institutionJournalRepository.FindAll(new InstitutionJournalFilter
                    {
                        InstitutionId = model.Institution,
                        PublisherId = journal.PublisherId
                    });

                    var publisherJournals = journalRepository.SearchAll(new JournalFilter { PublisherId = journal.PublisherId });

                    foreach (var publisherJournal in publisherJournals)
                    {
                        var institutionJournal = institutionJournals.FirstOrDefault(i => i.JournalId == publisherJournal.Id) ?? new InstitutionJournal();
                        institutionJournal.DateAdded = DateTime.Now;
                        institutionJournal.Link = model.Link;
                        institutionJournal.JournalId = publisherJournal.Id;
                        institutionJournal.UserProfileId = Authentication.CurrentUserId;
                        institutionJournal.InstitutionId = model.Institution;

                        institutionJournalsToModify.Add(institutionJournal);
                    }
                }
                else
                {
                    var institutionJournal = institutionJournalRepository.Find(id, model.Institution) ?? new InstitutionJournal();
                    institutionJournal.DateAdded = DateTime.Now;
                    institutionJournal.Link = model.Link;
                    institutionJournal.JournalId = journal.Id;
                    institutionJournal.UserProfileId = Authentication.CurrentUserId;
                    institutionJournal.InstitutionId = model.Institution;

                    institutionJournalsToModify.Add(institutionJournal);
                }

                foreach (var institutionJournal in institutionJournalsToModify)
                {
                    institutionJournalRepository.InsertOrUpdate(institutionJournal);
                }

                institutionJournalRepository.Save();

                return Redirect(model.RefererUrl);
            }

            model.JournalTitle = journal.Title;
            model.JournalLink = journal.Link;
            model.JournalPublisher = journal.Publisher.Name;

            return View(model);
        }

        [HttpPost, Route("{id:int}/institutionjournallicensedelete")]
        [Authorize(Roles = ApplicationRole.InstitutionAdmin + "," + ApplicationRole.Admin)]
        [ValidateAntiForgeryToken]
        public ActionResult InstitutionJournalLicenseDelete(int id, InstitutionJournalLicenseDeleteViewModel model)
        {
            if (!User.IsInRole(ApplicationRole.Admin) && !User.IsInRole(ApplicationRole.InstitutionAdmin))
            {
                return new HttpUnauthorizedResult();
            }

            if (ModelState.IsValid)
            {
                var institutionJournal = institutionJournalRepository.Find(id, model.Institution);
                if (institutionJournal == null)
                {
                    return new HttpNotFoundResult();
                }

                institutionJournalRepository.Delete(institutionJournal);
                institutionJournalRepository.Save();

                return Redirect(model.RefererUrl);
            }

            return RedirectToAction("InstitutionJournalLicense", new { id, InstitutionId = model.Institution, model.RefererUrl });
        }

        [HttpGet, Route("institutionalPrices")]
        [Authorize(Roles = ApplicationRole.Admin + "," + ApplicationRole.InstitutionAdmin)]
        public ActionResult BulkImportInstitutionalPrices()
        {
            return View(new InstitutionalPricesViewModel());
        }

        [HttpPost, Route("institutionalprices")]
        [Authorize(Roles = ApplicationRole.Admin + "," + ApplicationRole.InstitutionAdmin)]
        [ValidateAntiForgeryToken]
        public ActionResult BulkImportInstitutionalPrices(InstitutionalPricesViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var data = _bulkImporter.Execute(model.File.InputStream);
                var currentUserId = Authentication.CurrentUserId;
                
                var (imported, updated, deleted) = ProcessImportedLicenses(data, currentUserId);

                return RedirectToAction("BulkImportSuccessful", new { amountImported = imported, amountDeleted = deleted, amountUpdated = updated });
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

        [HttpGet, Route("bulkimportsuccessful")]
        [Authorize(Roles = ApplicationRole.Admin + "," + ApplicationRole.InstitutionAdmin)]
        public ActionResult BulkImportSuccessful(InstitutionalPricesImportedViewModel model)
        {
            return View(model);
        }

        [HttpGet, Route("import-list-prices")]
        [Authorize(Roles = ApplicationRole.Admin + "," + ApplicationRole.InstitutionAdmin)]
        public ActionResult BulkImportListPrices()
        {
            return View(new ImportJournalRelatedLinksViewModel());
        }

        [HttpPost, Route("import-list-prices")]
        [Authorize(Roles = ApplicationRole.DataAdmin + "," + ApplicationRole.Admin)]
        [ValidateAntiForgeryToken]
        public ActionResult BulkImportListPrices(ImportJournalRelatedLinksViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                int imported = 0, rejected = 0;
                var rejectedUrls = new List<JournalRelatedLink>();

                var data = _journalRelatedLinkBulkImporter.Execute(model.File.InputStream);

                foreach (var listPriceLink in data)
                {
                    var journal = journalRepository.FindByIssn(listPriceLink.ISSN);

                    if (journal == null)
                    {
                        rejected++;

                        rejectedUrls.Add(listPriceLink);
                        continue;
                    }

                    var listPrice = journalRepository.FindListPriceByJournalId(journal.Id) ?? new ListPrice { JournalId = journal.Id };

                    listPrice.Link = listPriceLink.Url;
                    listPrice.Text = listPriceLink.Text;

                    if (journal.ListPrice == null)
                        journalRepository.DbContext.ListPrices.Add(listPrice);

                    imported++;
                }

                journalRepository.Save();

                return View("ListPricesImportSuccessful", new JournalRelatedLinksImportedViewModel
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

        [HttpGet, Route("{id:int}/institutionjournaltext")]
        public ActionResult InstitutionJournalText(int id, int institutionId)
        {
            var model = institutionJournalRepository.Find(id, institutionId);

            if (model == null)
                return new HttpNotFoundResult();

            return View(model);
        }

        [HttpGet, Route("titles")]
        [OutputCache(CacheProfile = CacheProfile.OneQuarter)]
        public JsonResult Titles(string query)
        {
            return Json(journalRepository.Titles(query).Select(s => new { value = s }).Take(AutoCompleteItemsCount).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("issns")]
        [OutputCache(CacheProfile = CacheProfile.OneQuarter)]
        public JsonResult Issns(string query)
        {
            return Json(journalRepository.Issns(query).Select(s => new { value = s }).Take(AutoCompleteItemsCount).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("publishers")]
        [OutputCache(CacheProfile = CacheProfile.OneQuarter)]
        public JsonResult Publishers(string query)
        {
            return Json(journalRepository.Publishers(query).Select(s => new { value = s }).Take(AutoCompleteItemsCount).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("subjects")]
        [OutputCache(CacheProfile = CacheProfile.OneQuarter)]
        public JsonResult Subjects(string query)
        {
            return Json(journalRepository.Subjects(query).Select(s => new { value = s }).Take(AutoCompleteItemsCount).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("languages")]
        [OutputCache(CacheProfile = CacheProfile.OneQuarter)]
        public JsonResult Languages(string query)
        {
            return Json(journalRepository.Languages(query).Select(s => new { value = s }).Take(AutoCompleteItemsCount).ToList(), JsonRequestBehavior.AllowGet);
        }

        (int imported, int updated, int deleted) ProcessImportedLicenses(IList<UniversityLicense> data, int currentUserId)
        {
            int imported = 0, deleted = 0, updated = 0;

            var domains = data.Select(x => x.Domain).ToList();
            var institutions = institutionRepository.FindWhere(i => domains.Contains(i.ShortName)).ToList();
            var issns = data.SelectMany(x => x.Licenses).Select(l => l.ISSN).ToList();
            var journals = journalRepository.AllWhereIncluding(j => issns.Contains(j.ISSN)).ToList();

            var institutionJournals = (from u in data
                                       let institution = institutions.FirstOrDefault(i => i.ShortName == u.Domain)
                                       where institution != null
                                       from info in u.Licenses
                                       let journal = journals.FirstOrDefault(j => j.ISSN == info.ISSN)
                                       where journal != null
                                       select new InstitutionJournal
                                       {
                                           DateAdded = DateTime.Now,
                                           Link = info.Text,
                                           JournalId = journal.Id,
                                           UserProfileId = currentUserId,
                                           InstitutionId = institution.Id
                                       }).ToList();

            institutionJournalRepository.RefreshContext();

            foreach (var batch in institutionJournals.Distinct().Chunk(1000))
            {
                foreach (var institutionJournal in batch.ToList())
                {
                    var existing = institutionJournalRepository.Find(institutionJournal.JournalId, institutionJournal.InstitutionId);

                    if (existing != null)
                    {
                        if (string.IsNullOrWhiteSpace(institutionJournal.Link))
                        {
                            institutionJournalRepository.Delete(existing);
                            deleted++;
                        }
                        else
                        {
                            existing.DateAdded = DateTime.Now;
                            existing.Link = institutionJournal.Link;
                            existing.UserProfileId = institutionJournal.UserProfileId;

                            institutionJournalRepository.InsertOrUpdate(existing);

                            updated++;
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(institutionJournal.Link))
                    {
                        institutionJournalRepository.InsertOrUpdate(institutionJournal);
                        imported++;
                    }
                }

                institutionJournalRepository.Save();
                institutionJournalRepository.RefreshContext();
            }

            return (imported, updated, deleted);
        }
    }
}