namespace QOAM.Website.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AttributeRouting;
    using AttributeRouting.Web.Mvc;

    using QOAM.Core;
    using QOAM.Core.Repositories;
    using QOAM.Core.Repositories.Filters;
    using QOAM.Website.Helpers;
    using QOAM.Website.Models;
    using QOAM.Website.ViewModels.Journals;

    using Validation;

    [RoutePrefix("journals")]
    public class JournalsController : ApplicationController
    {
        private readonly IJournalRepository journalRepository;
        private readonly IJournalPriceRepository journalPriceRepository;
        private readonly ILanguageRepository languageRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly IInstitutionJournalRepository institutionJournalRepository;
        private readonly IScoreCardRepository scoreCardRepository;

        public JournalsController(IJournalRepository journalRepository, IJournalPriceRepository journalPriceRepository, ILanguageRepository languageRepository, ISubjectRepository subjectRepository, IInstitutionJournalRepository institutionJournalRepository, IScoreCardRepository scoreCardRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication)
            : base(userProfileRepository, authentication)
        {
            Requires.NotNull(journalRepository, "journalRepository");
            Requires.NotNull(journalPriceRepository, "journalPriceRepository");
            Requires.NotNull(languageRepository, "languageRepository");
            Requires.NotNull(subjectRepository, "keywordRepository");
            Requires.NotNull(institutionJournalRepository, "institutionJournalRepository");
            Requires.NotNull(scoreCardRepository, "scoreCardRepository");

            this.journalRepository = journalRepository;
            this.journalPriceRepository = journalPriceRepository;
            this.languageRepository = languageRepository;
            this.subjectRepository = subjectRepository;
            this.institutionJournalRepository = institutionJournalRepository;
            this.scoreCardRepository = scoreCardRepository;
        }
        
        [GET("")]
        public ViewResult Index(IndexViewModel model)
        {
            model.Languages = this.languageRepository.All.ToSelectListItems(Resources.Strings.AllLanguages);
            model.Disciplines = this.subjectRepository.All.ToSelectListItems(Resources.Strings.AllDisciplines);
            model.Journals = this.journalRepository.Search(model.ToFilter());
            
            return this.View(model);
        }

        [GET("{id:int}/prices")]
        public PartialViewResult Prices(PricesViewModel model)
        {
            this.ViewBag.RefererUrl = model.RefererUrl;

            model.InstitutionJournals = this.institutionJournalRepository.Find(model.ToInstitutionJournalPriceFilter());
            model.InstitutionJournal = this.institutionJournalRepository.Find(model.Id, this.Authentication.CurrentUserId);
            model.JournalPrices = this.journalPriceRepository.Find(model.ToJournalPriceFilter());
            model.Journal = this.journalRepository.Find(model.Id);

            return this.PartialView(model);
        }

        [GET("{id:int}/journalprices")]
        public PartialViewResult JournalPrices(PricesViewModel model)
        {
            this.ViewBag.RefererUrl = model.RefererUrl;

            var journalPrices = this.journalPriceRepository.Find(model.ToJournalPriceFilter());

            return this.PartialView(journalPrices);
        }

        [GET("{id:int}/institutionalprices")]
        public PartialViewResult InstitutionJournalPrices(PricesViewModel model)
        {
            this.ViewBag.RefererUrl = model.RefererUrl;

            var institutionJournals = this.institutionJournalRepository.Find(model.ToInstitutionJournalPriceFilter());

            return this.PartialView(institutionJournals);
        }

        [GET("{id:int}/scores")]
        public PartialViewResult Scores(ScoresViewModel model)
        {
            model.ScoreCards = this.scoreCardRepository.Find(model.ToFilter());

            return this.PartialView(GetScoresViewName(model), model);
        }

        [GET("{id:int}/comments")]
        public PartialViewResult Comments(CommentsViewModel model)
        {
            model.CommentedScoreCards = this.scoreCardRepository.Find(model.ToFilter());

            return this.PartialView(GetCommentsViewName(model), model);
        }

        [GET("{id:int}/institutionjournallicense")]
        [Authorize(Roles = ApplicationRole.InstitutionAdmin + "," + ApplicationRole.Admin)]
        public ViewResult InstitutionJournalLicense(int id, string refererUrl)
        {
            var model = new InstitutionJournalLicenseViewModel(
                this.journalRepository.Find(id),
                this.institutionJournalRepository.Find(id, this.Authentication.CurrentUserId), 
                refererUrl);
            
            return this.View(model);
        }

        [POST("{id:int}/institutionjournallicense")]
        [Authorize(Roles = ApplicationRole.InstitutionAdmin + "," + ApplicationRole.Admin)]
        [ValidateAntiForgeryToken]
        public ActionResult InstitutionJournalLicense(int id, InstitutionJournalLicenseViewModel model)
        {
            // Ensure that only admin and institutional admin users can use the update all journals of a publisher
            if (model.UpdateAllJournalsOfPublisher && (!this.User.IsInRole(ApplicationRole.Admin) && !this.User.IsInRole(ApplicationRole.InstitutionAdmin)))
            {
                return new HttpUnauthorizedResult();
            }

            var journal = this.journalRepository.Find(id);
            
            if (this.ModelState.IsValid)
            {
                var institutionJournalsToModify = new List<InstitutionJournal>();

                if (model.UpdateAllJournalsOfPublisher)
                {
                    var institutionJournals = this.institutionJournalRepository.FindAll(new InstitutionJournalFilter
                                                                                            {
                                                                                                InstitutionId = this.ViewBag.User.InstitutionId,
                                                                                                PublisherId = journal.PublisherId
                                                                                            });

                    var publisherJournals = this.journalRepository.SearchAll(new JournalFilter { PublisherId = journal.PublisherId });

                    foreach (var publisherJournal in publisherJournals)
                    {
                        var institutionJournal = institutionJournals.FirstOrDefault(i => i.JournalId == publisherJournal.Id) ?? new InstitutionJournal();
                        institutionJournal.DateAdded = DateTime.Now;
                        institutionJournal.Link = model.Link;
                        institutionJournal.JournalId = publisherJournal.Id;
                        institutionJournal.UserProfileId = this.Authentication.CurrentUserId;
                        institutionJournal.InstitutionId = this.ViewBag.User.InstitutionId;

                        institutionJournalsToModify.Add(institutionJournal);
                    }
                }
                else
                {
                    var institutionJournal = this.institutionJournalRepository.Find(id, this.Authentication.CurrentUserId) ?? new InstitutionJournal();
                    institutionJournal.DateAdded = DateTime.Now;
                    institutionJournal.Link = model.Link;
                    institutionJournal.JournalId = journal.Id;
                    institutionJournal.UserProfileId = this.Authentication.CurrentUserId;
                    institutionJournal.InstitutionId = this.ViewBag.User.InstitutionId;

                    institutionJournalsToModify.Add(institutionJournal);
                }

                foreach (var institutionJournal in institutionJournalsToModify)
                {
                    if (institutionJournal.Id == 0)
                    {
                        this.institutionJournalRepository.Insert(institutionJournal);
                    }
                    else
                    {
                        this.institutionJournalRepository.Update(institutionJournal);
                    }
                }

                this.institutionJournalRepository.Save();

                return this.Redirect(model.RefererUrl);
            }

            model.JournalTitle = journal.Title;
            model.JournalLink = journal.Link;
            model.JournalPublisher = journal.Publisher.Name;

            return this.View(model);
        }

        [GET("titles")]
        [OutputCache(CacheProfile = CacheProfile.OneQuarter)]
        public JsonResult Titles(string query)
        {
            return this.Json(this.journalRepository.Titles(query).Take(AutoCompleteItemsCount).ToList(), JsonRequestBehavior.AllowGet);
        }

        [GET("issns")]
        [OutputCache(CacheProfile = CacheProfile.OneQuarter)]
        public JsonResult Issns(string query)
        {
            return this.Json(this.journalRepository.Issns(query).Take(AutoCompleteItemsCount).ToList(), JsonRequestBehavior.AllowGet);
        }

        [GET("publishers")]
        [OutputCache(CacheProfile = CacheProfile.OneQuarter)]
        public JsonResult Publishers(string query)
        {
            return this.Json(this.journalRepository.Publishers(query).Take(AutoCompleteItemsCount).ToList(), JsonRequestBehavior.AllowGet);
        }

        private static string GetScoresViewName(ScoresViewModel model)
        {
            return model.ScoreCards.IsFirstPage ? "Scores" : "ScoresRows";
        }

        private static string GetCommentsViewName(CommentsViewModel model)
        {
            return model.CommentedScoreCards.IsFirstPage ? "Comments" : "CommentsRows";
        }
    }
}