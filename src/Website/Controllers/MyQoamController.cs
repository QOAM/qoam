using System;
using System.Web.Mvc;
using QOAM.Core;
using QOAM.Core.Import.Invitations;
using QOAM.Core.Repositories;
using QOAM.Website.Helpers;
using QOAM.Website.ViewModels.Journals;

namespace QOAM.Website.Controllers
{
    [RequireHttps, Authorize]
    [RoutePrefix("myqoam")]
    public class MyQoamController : ApplicationController
    {
        const int SubjectTruncationLength = 90;
        const string MyQoamMessage = "MyQoamMessage";

        readonly ILanguageRepository _languageRepository;
        readonly ISubjectRepository _subjectRepository;
        readonly IUserJournalRepository _userJournalRepository;
        readonly IJournalRepository _journalRepository;

        public MyQoamController(IBaseScoreCardRepository baseScoreCardRepository, IValuationScoreCardRepository valuationScoreCardRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication, ILanguageRepository languageRepository, ISubjectRepository subjectRepository, IUserJournalRepository userJournalRepository, IJournalRepository journalRepository) : base(baseScoreCardRepository, valuationScoreCardRepository, userProfileRepository, authentication)
        {
            _languageRepository = languageRepository;
            _subjectRepository = subjectRepository;
            _userJournalRepository = userJournalRepository;
            _journalRepository = journalRepository;
        }

        [HttpGet, Route("")]
        public ActionResult Index(IndexViewModel model)
        {
            model.Languages = _languageRepository.All.ToSelectListItems("<All languages>");
            model.Disciplines = _subjectRepository.All.ToSelectListItems("<All disciplines>", SubjectTruncationLength);
            model.Journals = _userJournalRepository.Search(model.ToFilter(Authentication.CurrentUserId));

            return View("JournalsIndex", model);
        }

        [HttpGet, Route("{id:int}/add")]
        public ActionResult Add(int id)
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

                TempData[MyQoamMessage] = "This journal has been added to My QOAM!";
            }

            return RedirectToAction("Details", "Journals", new { id = id });
        }

        [HttpGet, Route("{id:int}/delete")]
        public ActionResult Delete(int id)
        {
            var userJournal = _userJournalRepository.Find(id, Authentication.CurrentUserId);

            if (userJournal != null)
            {
                _userJournalRepository.Delete(userJournal);
                _userJournalRepository.Save();

                TempData[MyQoamMessage] = "This journal has been deleted from My QOAM.";
            }
            else
                TempData[MyQoamMessage] = "This journal isn't linked to My QOAM.";

            return RedirectToAction("Details", "Journals", new { id = id });
        }
    }
}