using System.Linq;
using System.Web.Mvc;
using QOAM.Core.Repositories;
using QOAM.Website.Controllers;
using QOAM.Website.Helpers;
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

        public BonaFideJournalsController(IJournalRepository journalRepository, ISubjectRepository subjectRepository, IBaseScoreCardRepository baseScoreCardRepository, IValuationScoreCardRepository valuationScoreCardRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication) : base(baseScoreCardRepository, valuationScoreCardRepository, userProfileRepository, authentication)
        {
            _journalRepository = journalRepository;
            _subjectRepository = subjectRepository;
        }

        [HttpGet, Route("")]
        public ActionResult Index(IndexViewModel model)
        {
            model.Disciplines = _subjectRepository.Active.Where(s => !string.IsNullOrWhiteSpace(s.Name)).ToList().ToSelectListItems("Search by discipline"); //NormalizeSearchStrings(model.Disciplines);
            model.Languages = NormalizeSearchStrings(model.Languages);
            model.Journals = _journalRepository.Search(model.ToFilter());

            return View("JournalsIndex", model);
        }
    }
}