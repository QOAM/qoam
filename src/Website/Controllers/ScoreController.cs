using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using QOAM.Core;
using QOAM.Core.Helpers;
using QOAM.Core.Import;
using QOAM.Core.Import.Invitations;
using QOAM.Core.Repositories;
using QOAM.Website.Helpers;
using QOAM.Website.Models;
using QOAM.Website.ViewModels.Score;
using Validation;

namespace QOAM.Website.Controllers
{
    using NLog;

    [RoutePrefix("score")]
    public class ScoreController : ApplicationController
    {
        private const int SubjectTruncationLength = 90;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IScoreCardVersionRepository scoreCardVersionRepository;
        private readonly IJournalRepository journalRepository;
        private readonly ILanguageRepository languageRepository;
        private readonly IQuestionRepository questionRepository;
        private readonly IBaseJournalPriceRepository baseJournalPriceRepository;
        private readonly GeneralSettings generalSettings;
        private readonly IValuationJournalPriceRepository valuationJournalPriceRepository;
        private readonly IInstitutionRepository institutionRepository;
        readonly IBulkImporter<AuthorToInvite> _bulkImporter;
        readonly IUserJournalRepository _userJournalRepository;
        readonly ISubjectRepository _subjectRepository;

        public ScoreController(IBaseScoreCardRepository baseScoreCardRepository, IBaseJournalPriceRepository baseJournalPriceRepository, IValuationScoreCardRepository valuationScoreCardRepository, IValuationJournalPriceRepository valuationJournalPriceRepository, IScoreCardVersionRepository scoreCardVersionRepository, IJournalRepository journalRepository, ILanguageRepository languageRepository, IQuestionRepository questionRepository, GeneralSettings generalSettings, IUserProfileRepository userProfileRepository, IInstitutionRepository institutionRepository, IAuthentication authentication, IBulkImporter<AuthorToInvite> bulkImporter, IUserJournalRepository userJournalRepository, ISubjectRepository subjectRepository)
            : base(baseScoreCardRepository, valuationScoreCardRepository, userProfileRepository, authentication)
        {
            Requires.NotNull(baseJournalPriceRepository, nameof(baseJournalPriceRepository));
            Requires.NotNull(valuationJournalPriceRepository, nameof(valuationJournalPriceRepository));
            Requires.NotNull(scoreCardVersionRepository, nameof(scoreCardVersionRepository));
            Requires.NotNull(journalRepository, nameof(journalRepository));
            Requires.NotNull(languageRepository, nameof(languageRepository));
            Requires.NotNull(questionRepository, nameof(questionRepository));
            Requires.NotNull(institutionRepository, nameof(institutionRepository));
            Requires.NotNull(generalSettings, nameof(generalSettings));
            
            this.scoreCardVersionRepository = scoreCardVersionRepository;
            this.valuationJournalPriceRepository = valuationJournalPriceRepository;
            this.journalRepository = journalRepository;
            this.languageRepository = languageRepository;
            this.questionRepository = questionRepository;
            this.baseJournalPriceRepository = baseJournalPriceRepository;
            this.institutionRepository = institutionRepository;
            this.generalSettings = generalSettings;

            _bulkImporter = bulkImporter;
            _userJournalRepository = userJournalRepository;
            _subjectRepository = subjectRepository;
        }

        [HttpGet, Route("")]
        public ActionResult Index(IndexViewModel model)
        {
            model.Disciplines = _subjectRepository.Active.Where(s => !string.IsNullOrWhiteSpace(s.Name)).ToList().ToSelectListItems("Search by discipline"); //NormalizeSearchStrings(model.Disciplines);
            model.Languages = NormalizeSearchStrings(model.Languages);
            model.Journals = journalRepository.Search(model.ToFilter());
            model.JournalIdsInMyQOAM = _userJournalRepository.Search(model.ToFilter(Authentication.CurrentUserId)).Select(x => x.Id);

            object saved;

            if (TempData.TryGetValue("MyQoamMessage", out saved))
                ViewBag.MyQoamMessage = saved.ToString();

            return this.View(model);
        }

        [HttpGet, Route("basescorecard/{id:int}")]
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

        [HttpPost, Route("basescorecard/{id:int}")]
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

        [HttpGet, Route("scorecard/{id:int}")]
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

        [HttpPost, Route("scorecard/{id:int}")]
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

        [HttpGet, Route("bulkrequestscore")]
        public ActionResult BulkRequestValuation()
        {
            var regex = new Regex(@"Dear Sir/Madam,\s*[\r\n]*", RegexOptions.Compiled);
            var model = new BulkRequestValuationViewModel
            {
                EmailBody = regex.Replace(Resources.RequestValuation.Body, ""),
                EmailSubject = Resources.RequestValuation.Subject
            };

            var currentUser = UserProfileRepository.Find(Authentication.CurrentUserId);

            if (currentUser != null)
                model.EmailFrom = currentUser.Email;

            return View(model);
        }

        [HttpPost, Route("bulkrequestscore")]
        [ValidateAntiForgeryToken]
        public ActionResult BulkRequestValuation(BulkRequestValuationViewModel model)
        {
            if (model.File == null)
                return View(model);

            try
            {
                var invited = 0;
                var notInvited = new List<NotInvitedViewModel>();
                var errorWhenInvited = new List<ErrorInvitedViewModel>();
                
                var data = _bulkImporter.Execute(model.File.InputStream);

                Logger.Info("Sending bulk validation requests...");

                var emails = (from a in data
                              where !string.IsNullOrWhiteSpace(a.ISSN) && !string.IsNullOrWhiteSpace(a.AuthorEmail) && EmailValidator.IsValid(a.AuthorEmail)
                              let journal = journalRepository.FindByIssn(a.ISSN)
                              where journal != null
                              select new RequestValuationViewModel
                              {
                                  JournalId = journal.Id,
                                  JournalTitle = journal.Title,
                                  JournalISSN = journal.ISSN,
                                  EmailFrom = "no-reply@qoam.eu",
                                  EmailTo = a.AuthorEmail,
                                  RecipientName = a.AuthorName,
                                  EmailBody = $"Dear {a.AuthorName},\r\n\r\n{model.EmailBody.Replace("<<JournalTitle>>", journal.Title)}\r\n\r\n-----------\r\n\r\nYou are being invited by: {model.EmailFrom}",
                                  EmailSubject = model.EmailSubject.Replace("<<JournalTitle>>", journal.Title).Replace("<<JournalISSN>>", journal.ISSN),
                                  IsKnownEmailAddress = IsKnownEmailAddress(a.AuthorEmail),
                                  HasKnownEmailDomain = HasKnownEmailDomain(a.AuthorEmail)
                              }).ToList();

                foreach (var item in emails.Select(e => new { Email = e.ToRequestValuationEmail(), Model = e }))
                {
                    // NOTE: Requirements have changed. Leo Waaijers requested that e-mail addresses with unknown domains could be invited to fill in a Score Card - Sergi Papaseit (2015-12-10)
                    try
                    {
                        item.Email.Url = FillEmailUrl(item.Model);
                        item.Email.Send();

                        Logger.Info($"Sent validation request to {item.Email.To}.");

                        invited++;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Error sending validation request to {item.Email.To}: {ex}.", ex);

                        errorWhenInvited.Add(new ErrorInvitedViewModel
                        {
                            ISSN = string.IsNullOrWhiteSpace(item.Model.JournalISSN) ? "Unknown" : item.Model.JournalISSN,
                            AuthorName = item.Model.RecipientName,
                            AuthorEmail = string.IsNullOrWhiteSpace(item.Model.EmailTo) ? "Unknown" : item.Model.EmailTo
                        });
                    }
                }

                notInvited.AddRange(
                    from a in data
                    where string.IsNullOrWhiteSpace(a.ISSN) || string.IsNullOrWhiteSpace(a.AuthorEmail) || !EmailValidator.IsValid(a.AuthorEmail)
                    select new NotInvitedViewModel
                    {
                        ISSN = string.IsNullOrWhiteSpace(a.ISSN) ? "Unknown" : a.ISSN,
                        AuthorName = a.AuthorName,
                        AuthorEmail = string.IsNullOrWhiteSpace(a.AuthorEmail) ? "Unknown" : a.AuthorEmail + " (invalid)"
                    });

                foreach (var notInvitedViewModel in notInvited)
                {
                    Logger.Info($"Skipped sending validation request to {notInvitedViewModel.AuthorEmail}.");
                }

                return View("BulkInviteSuccessful", new AuthorsInvitedViewModel
                {
                    AmountInvited = invited,
                    AuthorsNotInvited = notInvited,
                    AuthorsInvitedWithError = errorWhenInvited
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
            catch (FormatException fe)
            {
                ModelState.AddModelError("generalError", $"An error has ocurred: {fe.Message}");
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("generalError", $"An error has ocurred: {exception.InnermostMessage()}");
            }

            return View(model);
        }

        [HttpGet, Route("requestscore/{id:int}")]
        public ViewResult RequestValuation(int id)
        {
            var journal = this.journalRepository.Find(id);
            var model = new RequestValuationViewModel
                            {
                                JournalId = id, 
                                JournalTitle = journal.Title, 
                                JournalISSN = journal.ISSN, 
                                EmailBody = Resources.RequestValuation.Body
                                                .Replace("<<JournalTitle>>", journal.Title), 
                                EmailSubject = Resources.RequestValuation.Subject
                                                .Replace("<<JournalTitle>>", journal.Title)
                                                .Replace("<<JournalISSN>>", journal.ISSN),
                            };

            var currentUser = this.UserProfileRepository.Find(this.Authentication.CurrentUserId);
            if (currentUser != null)
            {
                model.EmailFrom = currentUser.Email;
            }

            return this.View(model);
        }

        [HttpPost, Route("requestscore/{id:int}")]
        [ValidateAntiForgeryToken]
        public ActionResult RequestValuation(RequestValuationViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            model.IsKnownEmailAddress = IsKnownEmailAddress(model.EmailTo);
            model.HasKnownEmailDomain = HasKnownEmailDomain(model.EmailTo);

            // NOTE: Requirements have changed. Leo Waaijers requested that e-mail addresses with unknown domains could be invited to fill in a Score Card - Sergi Papaseit (2015-12-10)
            //if (!model.IsKnownEmailAddress && !model.HasKnownEmailDomain)
            //{
            //    ModelState.AddModelError("", "Sorry, the domain name in the email address of the addressee does not match the name of an academic institution known to us. If you want this institution to be included in our list, please enter it’s name and web address in our contact box and we will respond promptly.");

            //    return View(model);
            //}

            var email = model.ToRequestValuationEmail();
            email.Url = FillEmailUrl(model);

            try
            {
                email.Send();

                return this.RedirectToAction("RequestScoreResult", new { success = true });
            }
            catch
            {
                return this.RedirectToAction("RequestScoreResult");
            }
        }

        [HttpGet, Route("requestscoreresult")]
        public ActionResult RequestScoreResult(bool success = false)
        {
            this.ViewBag.Success = success;

            return this.View();
        }

        [HttpGet, Route("basescorecard/details/{id:int}")]
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

        [HttpGet, Route("valuationscorecard/details/{id:int}")]
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

        private BaseScoreCard CreateNewBaseScoreCard(int id)
        {
            return new BaseScoreCard { DateStarted = DateTime.Now, UserProfileId = this.Authentication.CurrentUserId, Version = this.scoreCardVersionRepository.FindCurrent(), Journal = this.journalRepository.Find(id), QuestionScores = this.questionRepository.BaseScoreCardQuestions.Select(q => new BaseQuestionScore { Question = q, Score = Score.Undecided }).ToList(), Score = new BaseScoreCardScore(), };
        }

        private ValuationScoreCard CreateNewValuationScoreCard(int id)
        {
            return new ValuationScoreCard { DateStarted = DateTime.Now, UserProfileId = this.Authentication.CurrentUserId, Version = this.scoreCardVersionRepository.FindCurrent(), Journal = this.journalRepository.Find(id), QuestionScores = this.questionRepository.ValuationScoreCardQuestions.Select(q => new ValuationQuestionScore { Question = q, Score = Score.Undecided }).ToList(), Score = new ValuationScoreCardScore(), };
        }

        private BaseJournalPrice CreateNewBaseJournalPrice(BaseScoreCard scoreCard)
        {
            return new BaseJournalPrice { BaseScoreCardId = scoreCard.Id, JournalId = scoreCard.JournalId, UserProfileId = this.Authentication.CurrentUserId, DateAdded = DateTime.Now, };
        }

        private ValuationJournalPrice CreateNewValuationJournalPrice(ValuationScoreCard scoreCard)
        {
            return new ValuationJournalPrice { ValuationScoreCardId = scoreCard.Id, JournalId = scoreCard.JournalId, UserProfileId = this.Authentication.CurrentUserId };
        }

        private bool HasKnownEmailDomain(string address)
        {
            var addressList = address.Split(new char[] { ',', ';' });

            return addressList.Select(a => new MailAddress(a)).Select(mailAddress => this.institutionRepository.All.FirstOrDefault(i => mailAddress.Host.Contains(i.ShortName))).All(institution => institution != null);
        }

        private bool IsKnownEmailAddress(string address)
        {
            var addressList = address.Split(new[] { ',', ';' });

            return addressList.All(a => !string.IsNullOrEmpty(a) && this.UserProfileRepository.FindByEmail(a) != null);
        }

        string FillEmailUrl(RequestValuationViewModel model)
        {
            return model.IsKnownEmailAddress
                ? Url.Action("Login", "Account", new { ReturnUrl = Url.Action("ValuationScoreCard", new { id = model.JournalId }), loginAddress = model.EmailTo }, Request.Url?.Scheme)
                : Url.Action("Register", "Account", new { loginAddress = model.EmailTo, addLink = Url.Action("ValuationScoreCard", new { id = model.JournalId }) }, Request.Url?.Scheme);
        }
    }
}