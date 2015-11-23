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
        private const int SubjectTruncationLength = 90;

        private readonly IJournalRepository journalRepository;
        private readonly IBaseJournalPriceRepository baseJournalPriceRepository;
        private readonly ILanguageRepository languageRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly IInstitutionJournalRepository institutionJournalRepository;
        private readonly IValuationJournalPriceRepository valuationJournalPriceRepository;
        private readonly IInstitutionRepository institutionRepository;
        private readonly IBulkImporter _bulkImporter;

        public JournalsController(IJournalRepository journalRepository, IBaseJournalPriceRepository baseJournalPriceRepository, IValuationJournalPriceRepository valuationJournalPriceRepository,
            IValuationScoreCardRepository valuationScoreCardRepository, ILanguageRepository languageRepository, ISubjectRepository subjectRepository,
            IInstitutionJournalRepository institutionJournalRepository, IBaseScoreCardRepository baseScoreCardRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication,
            IInstitutionRepository institutionRepository, IBulkImporter bulkImporter)
            : base(baseScoreCardRepository, valuationScoreCardRepository, userProfileRepository, authentication)
        {
            Requires.NotNull(journalRepository, nameof(journalRepository));
            Requires.NotNull(baseJournalPriceRepository, nameof(baseJournalPriceRepository));
            Requires.NotNull(valuationJournalPriceRepository, nameof(valuationJournalPriceRepository));
            Requires.NotNull(languageRepository, nameof(languageRepository));
            Requires.NotNull(subjectRepository, nameof(subjectRepository));
            Requires.NotNull(institutionJournalRepository, nameof(institutionJournalRepository));
            Requires.NotNull(institutionRepository, nameof(institutionRepository));
            Requires.NotNull(valuationJournalPriceRepository, nameof(valuationJournalPriceRepository));
            Requires.NotNull(bulkImporter, nameof(bulkImporter));

            this.journalRepository = journalRepository;
            this.baseJournalPriceRepository = baseJournalPriceRepository;
            this.languageRepository = languageRepository;
            this.subjectRepository = subjectRepository;
            this.institutionJournalRepository = institutionJournalRepository;
            this.institutionRepository = institutionRepository;
            this.valuationJournalPriceRepository = valuationJournalPriceRepository;
            _bulkImporter = bulkImporter;
        }

        [HttpGet, Route("")]
        public ViewResult Index(IndexViewModel model)
        {
            model.Languages = languageRepository.All.ToSelectListItems("<All languages>");
            model.Disciplines = subjectRepository.All.ToSelectListItems("<All disciplines>", SubjectTruncationLength);
            model.Journals = journalRepository.Search(model.ToFilter());

            return View(model);
        }

        [HttpGet, Route("{id:int}")]
        public ActionResult Details(int id, string returnUrl)
        {
            var journal = journalRepository.Find(id);

            if (journal == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReturnUrl = returnUrl;

            return View(journal);
        }

        [HttpGet, Route("{id:int}/prices")]
        public PartialViewResult Prices(PricesViewModel model)
        {
            ViewBag.RefererUrl = model.RefererUrl;

            model.InstitutionJournals = institutionJournalRepository.Find(model.ToInstitutionJournalPriceFilter());
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

            var institutionJournals = institutionJournalRepository.Find(model.ToInstitutionJournalPriceFilter());

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
        [Authorize(Roles = ApplicationRole.InstitutionAdmin + "," + ApplicationRole.Admin)]
        public ActionResult BulkImportInstitutionalPrices()
        {
            return View(new InstitutionalPricesViewModel());
        }

        [HttpPost, Route("institutionalPrices")]
        [Authorize(Roles = ApplicationRole.InstitutionAdmin + "," + ApplicationRole.Admin)]
        [ValidateAntiForgeryToken]
        public ActionResult BulkImportInstitutionalPrices(InstitutionalPricesViewModel model)
        {
            try
            {
                var data = _bulkImporter.Execute(model.File.InputStream);

                var institutionJournals = (from u in data
                    let institution = institutionRepository.Find(u.Domain)
                    where institution != null
                    from info in u.Licenses
                    let journal = journalRepository.FindByIssn(info.ISSN)
                    where journal != null
                    select new InstitutionJournal
                    {
                        DateAdded = DateTime.Now,
                        Link = info.Text,
                        JournalId = journal.Id,
                        UserProfileId = Authentication.CurrentUserId,
                        InstitutionId = institution.Id
                    }).ToList();

                // This is gonna be quite an expensive operation... Rethink!
                foreach (var institutionJournal in institutionJournals)
                {
                    var existing = institutionJournalRepository.Find(institutionJournal.JournalId, institutionJournal.InstitutionId);

                    if (existing != null)
                    {
                        existing.DateAdded = DateTime.Now;
                        existing.Link = institutionJournal.Link;
                        existing.UserProfileId = institutionJournal.UserProfileId;

                        institutionJournalRepository.InsertOrUpdate(existing);
                    }
                    else
                        institutionJournalRepository.InsertOrUpdate(institutionJournal);
                }

                institutionJournalRepository.Save();

                return RedirectToAction("BulkImportSuccessful", new { amountImported = institutionJournals.Count });
            }
            catch (ArgumentException invalidFileException)
            {
                ModelState.AddModelError("generalError", invalidFileException.Message);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("generalError", $"An error has ocurred: {exception.Message}");
            }

            return View(model);
        }

        [HttpGet, Route("bulkImportSuccessful")]
        [Authorize(Roles = ApplicationRole.InstitutionAdmin + "," + ApplicationRole.Admin)]
        public ActionResult BulkImportSuccessful(InstitutionalPricesImportedViewModel model)
        {
            return View(model);
        }

        [HttpGet, Route("{id:int}/institutionJournalText")]
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
    }
}