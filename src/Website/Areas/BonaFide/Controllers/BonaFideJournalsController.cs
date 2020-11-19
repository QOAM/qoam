using System.Linq;
using System.Net;
using System.Web.Http.Results;
using System.Web.Mvc;
using QOAM.Core.Repositories;
using QOAM.Website.Controllers;
using QOAM.Website.Helpers;
using QOAM.Website.ViewModels.BonaFideJournals;
using QOAM.Website.ViewModels.Journals;

namespace QOAM.Website.Areas.BonaFide.Controllers
{
    [RouteArea("BonaFide", AreaPrefix = "bfj")]
    [RoutePrefix("")]
    [Route("{action}")]
    public class BonaFideJournalsController : ApplicationController
    {
        readonly IJournalRepository _journalRepository;
        readonly ISubjectRepository _subjectRepository;
        readonly ITrustedJournalRepository _trustedJournalRepository;

        public BonaFideJournalsController(IJournalRepository journalRepository, ISubjectRepository subjectRepository, IBaseScoreCardRepository baseScoreCardRepository, ITrustedJournalRepository trustedJournalRepository, IValuationScoreCardRepository valuationScoreCardRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication) : base(baseScoreCardRepository, valuationScoreCardRepository, userProfileRepository, authentication)
        {
            _journalRepository = journalRepository;
            _subjectRepository = subjectRepository;
            _trustedJournalRepository = trustedJournalRepository;
        }

        [HttpGet, Route("")]
        public ActionResult Index(BonaFideJournalsViewModel model)
        {
            model.Disciplines = _subjectRepository.Active.Where(s => !string.IsNullOrWhiteSpace(s.Name)).ToList().ToSelectListItems("Search by discipline"); //NormalizeSearchStrings(model.Disciplines);
            model.Languages = NormalizeSearchStrings(model.Languages);
            model.Journals = _journalRepository.Search(model.ToFilter());

            return View("Index", model);
        }

        [HttpPost, Route("submit-trust")]
        public ActionResult SubmitTrust(SubmitTrustViewModel model)
        {
            _trustedJournalRepository.InsertOrUpdate(model.ToTrustedJournal());
            _trustedJournalRepository.Save();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}