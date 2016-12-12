using System;
using System.Linq;
using System.Web.Mvc;
using QOAM.Core;
using QOAM.Core.Repositories;
using QOAM.Website.Helpers;
using QOAM.Website.ViewModels.Journals;

namespace QOAM.Website.Controllers
{
    [RequireHttps]
    [RoutePrefix("myqoam")]
    [Authorize]
    public class MyQoamController : ApplicationController
    {
        const int SubjectTruncationLength = 90;
        const string MyQoamMessage = "MyQoamMessage";

        readonly IUserJournalRepository _userJournalRepository;
        readonly ISubjectRepository _subjectRepository;

        public MyQoamController(IBaseScoreCardRepository baseScoreCardRepository, IValuationScoreCardRepository valuationScoreCardRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication, IUserJournalRepository userJournalRepository, ISubjectRepository subjectRepository) : base(baseScoreCardRepository, valuationScoreCardRepository, userProfileRepository, authentication)
        {
            _userJournalRepository = userJournalRepository;
            _subjectRepository = subjectRepository;
        }

        [HttpGet, Route("")]
        public ActionResult Index(IndexViewModel model)
        {
            try
            {
                model.Disciplines = _subjectRepository.Active.Where(s => !string.IsNullOrWhiteSpace(s.Name)).ToList().ToSelectListItems("Search by discipline"); //NormalizeSearchStrings(model.Disciplines);
                model.Languages = NormalizeSearchStrings(model.Languages);
                model.Journals = _userJournalRepository.Search(model.ToFilter(Authentication.CurrentUserId));
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("", exception);
            }

            return View("JournalsIndex", model);
        }

        [HttpGet, Route("{id:int}/add")]
        public ActionResult Add(int id, string returnUrl = "")
        {
            var currentUserId = Authentication.CurrentUserId;

            var existing = _userJournalRepository.Find(id, currentUserId);

            if (existing != null)
                TempData[MyQoamMessage] = "This journal had already been added to My QOAM.";
            else
            {
                var entity = new UserJournal
                {
                    UserProfileId = currentUserId,
                    JournalId = id,
                    DateAdded = DateTime.Now
                };

                _userJournalRepository.InsertOrUpdate(entity);
                _userJournalRepository.Save();

                TempData[MyQoamMessage] = "Journal has been added to My QOAM!";
            }

            return string.IsNullOrWhiteSpace(returnUrl) ? (ActionResult)RedirectToAction("Details", "Journals", new { id }) : Redirect(returnUrl);
        }

        [HttpGet, Route("{id:int}/delete")]
        public ActionResult Delete(int id, string returnUrl = "")
        {
            var userJournal = _userJournalRepository.Find(id, Authentication.CurrentUserId);

            if (userJournal != null)
            {
                _userJournalRepository.Delete(userJournal);
                _userJournalRepository.Save();

                TempData[MyQoamMessage] = "Journal has been deleted from My QOAM.";
            }
            else
                TempData[MyQoamMessage] = "This journal isn't linked to My QOAM.";

            return string.IsNullOrWhiteSpace(returnUrl) ? (ActionResult)RedirectToAction("Details", "Journals", new { id }) : Redirect(returnUrl);
        }

        [HttpGet, Route("empty")]
        public ActionResult Empty()
        {
            var userJournals = _userJournalRepository.All(Authentication.CurrentUserId);

            foreach (var userJournal in userJournals)
                _userJournalRepository.Delete(userJournal);

            _userJournalRepository.Save();

            return RedirectToAction("Index");
        }
    }
}