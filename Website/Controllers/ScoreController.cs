namespace QOAM.Website.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using AttributeRouting;
    using AttributeRouting.Web.Mvc;
    using QOAM.Core;
    using QOAM.Core.Repositories;
    using QOAM.Website.Helpers;
    using QOAM.Website.Models;
    using QOAM.Website.ViewModels.Score;
    using vmProfile = QOAM.Website.ViewModels.Profiles;
    using System.Net.Mail;
    using System.Web.UI.WebControls;
    using QOAM.Core.Services;
    using WebMatrix.WebData;
    using MailMessage = System.Net.Mail.MailMessage;
    using Validation;

    [RoutePrefix("score")]
    public class ScoreController : ApplicationController
    {
        private readonly IBaseScoreCardRepository baseScoreCardRepository;
        private readonly IScoreCardVersionRepository scoreCardVersionRepository;
        private readonly IJournalRepository journalRepository;
        private readonly ILanguageRepository languageRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly IQuestionRepository questionRepository;
        private readonly IBaseJournalPriceRepository baseJournalPriceRepository;
        private readonly GeneralSettings generalSettings;
        private readonly IValuationScoreCardRepository valuationScoreCardRepository;
        private readonly IValuationJournalPriceRepository valuationJournalPriceRepository;
        private readonly IInstitutionRepository institutionRepository;

        public ScoreController(IBaseScoreCardRepository baseScoreCardRepository, IBaseJournalPriceRepository baseJournalPriceRepository, IValuationScoreCardRepository valuationScoreCardRepository, IValuationJournalPriceRepository valuationJournalPriceRepository, IScoreCardVersionRepository scoreCardVersionRepository, IJournalRepository journalRepository, ILanguageRepository languageRepository, ISubjectRepository subjectRepository, IQuestionRepository questionRepository, GeneralSettings generalSettings, IUserProfileRepository userProfileRepository, IInstitutionRepository institutionRepository, IAuthentication authentication, IMailSender mailSender, ContactSettings contactSettings)
            : base(userProfileRepository, authentication)
        {
            
            Requires.NotNull(baseScoreCardRepository, "baseScoreCardRepository");
            Requires.NotNull(baseJournalPriceRepository, "baseJournalPriceRepository");
            Requires.NotNull(valuationScoreCardRepository, "valuationScoreCardRepository");
            Requires.NotNull(valuationJournalPriceRepository, "valuationJournalPriceRepository");
            Requires.NotNull(scoreCardVersionRepository, "scoreCardVersionRepository");
            Requires.NotNull(journalRepository, "journalRepository");
            Requires.NotNull(languageRepository, "languageRepository");
            Requires.NotNull(subjectRepository, "keywordRepository");
            Requires.NotNull(questionRepository, "questionRepository");
            Requires.NotNull(institutionRepository, "institutionRepository");
            Requires.NotNull(generalSettings, "generalSettings");

            this.baseScoreCardRepository = baseScoreCardRepository;
            this.scoreCardVersionRepository = scoreCardVersionRepository;
            this.valuationJournalPriceRepository = valuationJournalPriceRepository;
            this.valuationScoreCardRepository = valuationScoreCardRepository;
            this.journalRepository = journalRepository;
            this.languageRepository = languageRepository;
            this.subjectRepository = subjectRepository;
            this.questionRepository = questionRepository;
            this.baseJournalPriceRepository = baseJournalPriceRepository;
            this.institutionRepository = institutionRepository;
            this.generalSettings = generalSettings;
        }

        [GET("")]
        public ActionResult Index(IndexViewModel model)
        {
            model.Languages = this.languageRepository.All.ToSelectListItems("<All languages>");
            model.Disciplines = this.subjectRepository.All.ToSelectListItems("<All disciplines>");
            model.Journals = this.journalRepository.Search(model.ToFilter());

            return this.View(model);
        }

        [GET("basescorecard/{id:int}")]
        [Authorize]
        public ViewResult BaseScoreCard(int id)
        {
            var scoreCard = this.baseScoreCardRepository.Find(id, this.Authentication.CurrentUserId);
            
            if (scoreCard == null)
            {
                scoreCard = this.CreateNewBaseScoreCard(id);

                this.baseScoreCardRepository.InsertOrUpdate(scoreCard);
                this.baseScoreCardRepository.Save();
            }

            var journalPrice = this.baseJournalPriceRepository.Find(id, this.Authentication.CurrentUserId);

            if (journalPrice == null)
            {
                journalPrice = this.CreateNewBaseJournalPrice(scoreCard);

                this.baseJournalPriceRepository.InsertOrUpdate(journalPrice);
                this.baseJournalPriceRepository.Save();
            }

            var scoreViewModel = scoreCard.ToBaseScoreCardViewModel();
            scoreViewModel.Price = journalPrice.ToViewModel();
            scoreViewModel.Currencies = ((Currency[])Enum.GetValues(typeof(Currency))).Select(c => new KeyValuePair<Currency, string>(c, c.GetName()));

            return this.View(scoreViewModel);
        }

        [POST("basescorecard/{id:int}")]
        [Authorize]
        [ValidateJsonAntiForgeryToken]
        public ActionResult BaseScoreCard(int id, BaseScoreCardViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var scoreCard = this.baseScoreCardRepository.Find(id, this.Authentication.CurrentUserId);
            model.UpdateScoreCard(scoreCard, this.generalSettings.ScoreCardLifeTime);

            // It is important to note that the JournalScore of the Journal is updated in a 
            // trigger in the datavaluation and not in the code. This is done to prevent concurrency 
            // issues from leading to incorrect totals and averages for the journal score

            this.baseScoreCardRepository.InsertOrUpdate(scoreCard);
            this.baseScoreCardRepository.Save();

            var journalPrice = this.baseJournalPriceRepository.Find(id, this.Authentication.CurrentUserId);
            model.UpdateJournalPrice(journalPrice);

            this.baseJournalPriceRepository.InsertOrUpdate(journalPrice);
            this.valuationJournalPriceRepository.Save();

            return this.Json(true);
        }
        
        [GET("valuationscorecard/{id:int}")]
        [Authorize]
        public ViewResult ValuationScoreCard(int id)
        {
            var scoreCard = this.valuationScoreCardRepository.Find(id, this.Authentication.CurrentUserId);
            var journalPrice = this.valuationJournalPriceRepository.Find(id, this.Authentication.CurrentUserId);

            if (scoreCard == null)
            {
                scoreCard = this.CreateNewValuationScoreCard(id);

                this.valuationScoreCardRepository.InsertOrUpdate(scoreCard);
                this.valuationScoreCardRepository.Save();
            }

            var scoreViewModel = scoreCard.ToValuationScoreCardViewModel();
            scoreViewModel.Price = journalPrice.ToViewModel();
            scoreViewModel.Currencies = ((Currency[])Enum.GetValues(typeof(Currency))).Select(c => new KeyValuePair<Currency, string>(c, c.GetName()));

            return this.View(scoreViewModel);
        }

        [POST("valuationscorecard/{id:int}")]
        [Authorize]
        [ValidateJsonAntiForgeryToken]
        public ActionResult ValuationScoreCard(int id, ValuationScoreCardViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var scoreCard = this.valuationScoreCardRepository.Find(id, this.Authentication.CurrentUserId);

            model.UpdateScoreCard(scoreCard, this.generalSettings.ScoreCardLifeTime);

            // It is important to note that the JournalScore of the Journal is updated in a 
            // trigger in the datavaluation and not in the code. This is done to prevent concurrency 
            // issues from leading to incorrect totals and averages for the journal score

            this.valuationScoreCardRepository.InsertOrUpdate(scoreCard);
            this.valuationScoreCardRepository.Save();

            var journalPrice = this.valuationJournalPriceRepository.Find(id, this.Authentication.CurrentUserId) ?? this.CreateNewValuationJournalPrice(scoreCard);

            model.UpdateJournalPrice(journalPrice);

            if (model.HasPrice)
            {
                this.valuationJournalPriceRepository.InsertOrUpdate(journalPrice);    
            }
            else
            {
                this.valuationJournalPriceRepository.Delete(journalPrice);
            }

            this.valuationJournalPriceRepository.Save();

            return this.Json(true);
        }

        [GET("reqvaluation/{id:int}")]
        public ViewResult ReqValuation(int id)
        {
            Journal journal = this.journalRepository.Find(id);
            ReqValuationViewModel model = new ReqValuationViewModel()
            {
                JournalId = id,
                JournalTitle = journal.Title,
                JournalISSN = journal.ISSN,
                EMailFrom = string.Empty,
                EMailTo = string.Empty,
                EMailBody = string.Empty,
                Message = string.Empty,
                IsKnowDomain = false,
                IsKnowTo = false
            };
            model.EMailSubject = "Please take a moment to valuate journal: " + model.JournalTitle + " (ISSN: " + model.JournalISSN + ")";
            var currentUser = UserProfileRepository.Find(WebSecurity.CurrentUserId);
            if (currentUser != null)
            {
                model.EMailFrom = currentUser.Email;
            }
            var host = "https://qoam.eu";
            if (Request.Url != null && (Request.Url.Scheme != "" && Request.Url.Host != ""))
            {
                host = Request.Url.Scheme + "://" + Request.Url.Host;
            }
            if (System.IO.File.Exists(Server.MapPath(@"~/App_Data/valuationinvitation.txt")))
            {
                model.EMailBody = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/valuationinvitation.txt"));
                model.EMailBody = model.EMailBody.Replace("<<QOAM>>", host);
                model.EMailBody = model.EMailBody.Replace("<<Journal>>", journal.Title);
            }

            return this.View(model);
        }

        [POST("reqvaluation/{id:int}")]
        public ActionResult ReqValuation(ReqValuationViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            model.Message = string.Empty;
            var mailTo = Request.Form["EMailTo"];

            if (mailTo != "")
            {
                if (CheckMailExists(mailTo))
                {
                    model.IsKnowTo = true;
                    ReqValuationMailOk(model);
                    return View("ReqValuationMailOk", model);
                }
                if (CheckMailDomain(mailTo))
                {
                    model.IsKnowDomain = true;
                    ReqValuationMailOk(model);
                    return View("ReqValuationDomainOk", model);
                }
            }
            model.Message =
                string.Format(
                    "* Sorry. The domain name in this email address does not match the name of an academic institution known to us.\r\n" +
                    "If you want the institution to be included in our list, " +
                    "please enter it’s name and web address in our <a href='/contact' target='blank'>contact box</a> and we will respond promptly.");
            return View("ReqValuation", model);
        }

        private void ReqValuationMailOk(ReqValuationViewModel model)
        {
            var host = "https://qoam.eu";
            if (Request.Url != null && (Request.Url.Scheme != "" && Request.Url.Host != ""))
            {
                host = Request.Url.Scheme + "://" + Request.Url.Host;
            }
            if (model.IsKnowTo)
            {
                model.EMailBody = String.Concat(model.EMailBody, 
                    string.Format("\r\n\r\nUse the link below to go to QOAM and publish your Valuation Score Card."+
                                  "\r\n{2}/Login?ReturnUrl=%2fscore%2fvaluationscorecard%2f{0}&" +
                                  "loginAddress={1}", model.JournalId, model.EMailTo, host));
            }
            else if (model.IsKnowDomain)
            {
                model.EMailBody = String.Concat(model.EMailBody,
                    string.Format("\r\n\r\nClick the link below to register yourself in QOAM.\r\n" +
                        "After registration you will proceed to the Valuation Score Card of {0} (ISSN: {1}).\r\n" +
                        "{2}/Register?loginAddress={3}&AddLink=%2fscore%2fvaluationscorecard%2f{4}", 
                        model.JournalTitle, model.JournalISSN, host, model.EMailTo, model.JournalId));
            }
            var txt = new Literal()
            {
                Text = model.EMailBody
            };
            var ml = new SmtpClient();
            var mail = new MailMessage()
            {
                From = new MailAddress(model.EMailFrom),
                To = { new MailAddress(model.EMailTo) },
                ReplyToList = { new MailAddress(model.EMailFrom) },
                Sender = new MailAddress(model.EMailFrom),
                Subject = model.EMailSubject,
                Body = txt.Text,
                IsBodyHtml = false
            };
            //TODO: for testing
            //mail.To = { new MailAddress("henk.drent@wur.nl") },
            try
            {
                model.Message = string.Format("Succesfully sent mail to {0}", model.EMailTo);
                ml.Send(mail);
            }
            catch (Exception ex)
            {
                model.Message = string.Format("Sending mail to {0} failed\nError:\n{1}", model.EMailTo, ex.Message);
            }
        }

        [GET("basescorecard/details/{id:int}")]
        public ActionResult BaseScoreCardDetails(int id)
        {
            var scoreCard = this.baseScoreCardRepository.Find(id);

            if (scoreCard.UserProfileId == this.Authentication.CurrentUserId)
            {
                return this.RedirectToAction("BaseScoreCard", scoreCard.JournalId);
            }

            if (scoreCard.State != ScoreCardState.Published)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            this.ViewBag.JournalPrice = this.baseJournalPriceRepository.Find(scoreCard.JournalId, scoreCard.UserProfileId);

            return this.View(scoreCard);
        }

        [GET("valuationscorecard/details/{id:int}")]
        public ActionResult ValuationScoreCardDetails(int id)
        {
            var scoreCard = this.valuationScoreCardRepository.Find(id);

            if (scoreCard.UserProfileId == this.Authentication.CurrentUserId)
            {
                return this.RedirectToAction("ValuationScoreCard", scoreCard.JournalId);
            }

            if (scoreCard.State != ScoreCardState.Published)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            this.ViewBag.JournalPrice = this.valuationJournalPriceRepository.Find(scoreCard.JournalId, scoreCard.UserProfileId);

            return this.View(scoreCard);
        }

        private bool CheckMailDomain(string addresses )
        {
            var ads = addresses.Split(',');
            if (ads.Length == 1)
                ads = addresses.Split(';');
            foreach (var address in ads)
            {
                if (address != "" && address.Contains('@'))
                {
                    var adres = address.Trim();
                    var maildomain = adres.Split('@')[1].ToUpper();
                    Institution found = this.institutionRepository.Find(maildomain);
                    if (found == null) return false;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckMailExists(string addresses)
        {
            var ads = addresses.Split(',');
            if (ads.Length == 1)
                ads = addresses.Split(';');
            foreach (var address in ads)
            {
                var adres = address.Trim();
                if (adres == "") return false;
                if (this.UserProfileRepository.FindByEmail(adres) == null) return false;
            }
            return true;
        }

        private BaseScoreCard CreateNewBaseScoreCard(int id)
        {
            return new BaseScoreCard
                       {
                           DateStarted = DateTime.Now,
                           UserProfileId = this.Authentication.CurrentUserId,
                           Version = this.scoreCardVersionRepository.FindCurrent(),
                           Journal = this.journalRepository.Find(id),
                           QuestionScores = this.questionRepository.BaseScoreCardQuestions.Select(q => new BaseQuestionScore { Question = q, Score = Score.Undecided }).ToList(),
                           Score = new BaseScoreCardScore(),
                       };
        }

        private ValuationScoreCard CreateNewValuationScoreCard(int id)
        {
            return new ValuationScoreCard
            {
                DateStarted = DateTime.Now,
                UserProfileId = this.Authentication.CurrentUserId,
                Version = this.scoreCardVersionRepository.FindCurrent(),
                Journal = this.journalRepository.Find(id),
                QuestionScores = this.questionRepository.ValuationScoreCardQuestions.Select(q => new ValuationQuestionScore { Question = q, Score = Score.Undecided }).ToList(),
                Score = new ValuationScoreCardScore(),
            };
        }

        private BaseJournalPrice CreateNewBaseJournalPrice(BaseScoreCard scoreCard)
        {
            return new BaseJournalPrice
                   {
                       BaseScoreCardId = scoreCard.Id, 
                       JournalId = scoreCard.JournalId,
                       UserProfileId = this.Authentication.CurrentUserId,
                       DateAdded = DateTime.Now,
                   };
        }

        private ValuationJournalPrice CreateNewValuationJournalPrice(ValuationScoreCard scoreCard)
        {
            return new ValuationJournalPrice { ValuationScoreCardId = scoreCard.Id, JournalId = scoreCard.JournalId, UserProfileId = this.Authentication.CurrentUserId };
        }
    }
}