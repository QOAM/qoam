namespace RU.Uci.OAMarket.Website.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using AttributeRouting;
    using AttributeRouting.Web.Mvc;

    using RU.Uci.OAMarket.Domain;
    using RU.Uci.OAMarket.Domain.Helpers;
    using RU.Uci.OAMarket.Domain.Repositories;
    using RU.Uci.OAMarket.Website.Helpers;
    using RU.Uci.OAMarket.Website.Models;
    using RU.Uci.OAMarket.Website.ViewModels.Score;

    using Validation;

    using Strings = RU.Uci.OAMarket.Website.Resources.Strings;

    [RoutePrefix("score")]
    public class ScoreController : ApplicationController
    {
        private readonly IScoreCardRepository scoreCardRepository;
        private readonly IScoreCardVersionRepository scoreCardVersionRepository;
        private readonly IJournalRepository journalRepository;
        private readonly ILanguageRepository languageRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly IQuestionRepository questionRepository;
        private readonly IJournalPriceRepository journalPriceRepository;
        private readonly GeneralSettings generalSettings;

        public ScoreController(IScoreCardRepository scoreCardRepository, IScoreCardVersionRepository scoreCardVersionRepository, IJournalRepository journalRepository, ILanguageRepository languageRepository, ISubjectRepository subjectRepository, IQuestionRepository questionRepository, IJournalPriceRepository journalPriceRepository, GeneralSettings generalSettings, IUserProfileRepository userProfileRepository, IAuthentication authentication)
            : base(userProfileRepository, authentication)
        {
            Requires.NotNull(scoreCardRepository, "scoreCardRepository");
            Requires.NotNull(scoreCardVersionRepository, "scoreCardVersionRepository");
            Requires.NotNull(journalRepository, "journalRepository");
            Requires.NotNull(languageRepository, "languageRepository");
            Requires.NotNull(subjectRepository, "keywordRepository");
            Requires.NotNull(questionRepository, "questionRepository");
            Requires.NotNull(journalPriceRepository, "institutionJournalRepository");
            Requires.NotNull(generalSettings, "generalSettings");

            this.scoreCardRepository = scoreCardRepository;
            this.scoreCardVersionRepository = scoreCardVersionRepository;
            this.journalRepository = journalRepository;
            this.languageRepository = languageRepository;
            this.subjectRepository = subjectRepository;
            this.questionRepository = questionRepository;
            this.journalPriceRepository = journalPriceRepository;
            this.generalSettings = generalSettings;
        }

        [GET("")]
        public ActionResult Index(IndexViewModel model)
        {
            model.Languages = this.languageRepository.All.ToSelectListItems(Strings.AllLanguages);
            model.Disciplines = this.subjectRepository.All.ToSelectListItems(Strings.AllDisciplines);
            model.Journals = this.journalRepository.Search(model.ToFilter());

            return this.View(model);
        }

        [GET("journal/{id:int}")]
        [Authorize]
        public ViewResult Journal(int id)
        {
            var scoreCard = this.scoreCardRepository.Find(id, this.Authentication.CurrentUserId);
            var journalPrice = this.journalPriceRepository.Find(id, this.Authentication.CurrentUserId);

            if (scoreCard == null)
            {
                scoreCard = this.CreateNewScoreCard(id);

                this.scoreCardRepository.Insert(scoreCard);
                this.scoreCardRepository.Save();
            }

            var scoreViewModel = scoreCard.ToViewModel();
            scoreViewModel.Price = journalPrice.ToViewModel();
            scoreViewModel.Currencies = ((Currency[])Enum.GetValues(typeof(Currency))).Select(c => new KeyValuePair<Currency, string>(c, c.GetName()));

            return this.View(scoreViewModel);
        }

        [POST("journal/{id:int}")]
        [Authorize]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult Journal(int id, ScoreViewModel model)
        {
            var scoreCard = this.scoreCardRepository.Find(id);

            if (scoreCard.UserProfileId != this.Authentication.CurrentUserId)
            {
                return new HttpUnauthorizedResult();
            }

            if (this.ModelState.IsValid)
            {
                // Update the score card using the values of the model
                model.UpdateScoreCard(scoreCard, this.generalSettings.ScoreCardLifeTime);

                // It is important to note that the JournalScore of the Journal is updated in a 
                // trigger in the database and not in the code. This is done to prevent concurrency 
                // issues from leading to incorrect totals and averages for the journal score

                this.scoreCardRepository.Update(scoreCard);
                this.scoreCardRepository.Save();

                var isNewJournalPrice = model.Price.JournalPriceId == 0;

                if (isNewJournalPrice)
                {
                    if (model.Price.Amount.HasValue)
                    {
                        if (model.Submitted)
                            model.Price.FeeType = FeeType.Article; // set the default for backwards compatibility

                        var journalPrice = model.Price.ToJournalPrice();
                        journalPrice.ScoreCardId = scoreCard.Id;
                        journalPrice.JournalId = model.Journal.Id;
                        journalPrice.UserProfileId = this.Authentication.CurrentUserId;
                        journalPrice.DateAdded = DateTime.Now;
                        journalPrice.Price.Amount = model.Price.Amount;
                        journalPrice.Price.Currency = model.Price.Currency;
                        journalPrice.Price.FeeType = model.Price.FeeType;

                        if (!model.Submitted)
                        {
                            if (model.Price.FeeType == FeeType.NoFee || model.Price.FeeType == FeeType.Absent)
                            {
                                journalPrice.Price.Amount = 0;
                                journalPrice.Price.Currency = null;
                            }
                        }

                        this.journalPriceRepository.Insert(journalPrice);

                        var journal = this.journalRepository.Find(model.Journal.Id);
                        journal.JournalPrice = journalPrice;

                        this.journalRepository.Update(journal);
                    }
                    else
                    {
                        var journalPrice = new JournalPrice();
                        journalPrice.ScoreCardId = scoreCard.Id;
                        journalPrice.UserProfileId = this.Authentication.CurrentUserId;
                        journalPrice.DateAdded = DateTime.Now;
                        journalPrice.Price.FeeType = model.Price.FeeType;
                        journalPrice.Price.Amount = 0;
                        journalPrice.JournalId = model.Journal.Id;
                        journalPrice.Price.Currency = null;
                        this.journalPriceRepository.Insert(journalPrice);

                    }
                }
                else
                {
                    var journalPrice = this.journalPriceRepository.Find(model.Price.JournalPriceId);
                    if (journalPrice.JournalId != model.Journal.Id)
                    {
                        return new HttpUnauthorizedResult();
                    }

                    if (journalPrice.UserProfileId != this.Authentication.CurrentUserId)
                    {
                        return new HttpUnauthorizedResult();
                    }

                    if (model.Price.Amount.HasValue)
                    {
                        if (model.Submitted)
                            model.Price.FeeType = FeeType.Article; // set the default for backwards compatibility

                        journalPrice.ScoreCardId = scoreCard.Id;
                        journalPrice.DateAdded = DateTime.Now;
                        journalPrice.Price.Amount = model.Price.Amount;
                        journalPrice.Price.Currency = model.Price.Currency;
                        journalPrice.Price.FeeType = model.Price.FeeType;

                        if (!model.Submitted)
                        {
                            if (model.Price.FeeType == FeeType.NoFee || model.Price.FeeType == FeeType.Absent)
                            {
                                journalPrice.Price.Amount = 0;
                                journalPrice.Price.Currency = null;
                            }                            
                        }
                        this.journalPriceRepository.Update(journalPrice);

                        var journal = this.journalRepository.Find(model.Journal.Id);
                        journal.JournalPrice = journalPrice;

                        this.journalRepository.Update(journal);
                    }
                    else
                    {
                        this.journalPriceRepository.Delete(journalPrice);
                    }
                }

                this.journalPriceRepository.Save();

                return this.Json(true);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [GET("{id:int}")]
        public ActionResult Details(int id)
        {
            var scoreCard = this.scoreCardRepository.Find(id);

            if (scoreCard.UserProfileId == this.Authentication.CurrentUserId)
            {
                return this.RedirectToAction("Journal", scoreCard.JournalId);
            }

            if (scoreCard.State != ScoreCardState.Published)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            ViewBag.JournalPrice = this.journalPriceRepository.Find(scoreCard.JournalId, scoreCard.UserProfileId);

            return this.View(scoreCard);
        }

        private ScoreCard CreateNewScoreCard(int id)
        {
            return new ScoreCard
                       {
                           DateStarted = DateTime.Now,
                           UserProfileId = this.Authentication.CurrentUserId,
                           Version = this.scoreCardVersionRepository.FindCurrent(),
                           Journal = this.journalRepository.Find(id),
                           QuestionScores = this.questionRepository.All.Select(q => new QuestionScore { Question = q, Score = Score.Undecided }).ToSet(),
                           Score = new ScoreCardScore(),
                       };
        }
    }
}