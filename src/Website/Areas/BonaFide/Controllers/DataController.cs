using System.IO;
using System.Linq;
using System.Web.Mvc;
using QOAM.Core.Export.BonaFide;
using QOAM.Core.Repositories;
using QOAM.Website.Controllers;
using QOAM.Website.Helpers;

namespace QOAM.Website.Areas.BonaFide.Controllers
{
    [RouteArea("BonaFide", AreaPrefix = "bfj")]
    [RoutePrefix("data")]
    [Route("{action}")]
    public class DataController : ApplicationController
    {
        readonly IJournalRepository _journalRepository;

        // GET
        public DataController(IBaseScoreCardRepository baseScoreCardRepository, IValuationScoreCardRepository valuationScoreCardRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication, IJournalRepository journalRepository) : base(baseScoreCardRepository, valuationScoreCardRepository, userProfileRepository, authentication)
        {
            _journalRepository = journalRepository;
        }

        [HttpGet, Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet, Route("dowload/{fileFormat}")]
        public FileContentResult Download(DataMiningFormats fileFormat)
        {
            _journalRepository.EnableProxyCreation = false;
            var journals = _journalRepository.AllWhere(j => j.NoFee || j.InDoaj || j.TrustingInstitutions.Any() || j.InstitutionJournalPrices.Any());

            var dataMiner = DataMiningFactory.Create(fileFormat);

            var result = dataMiner.GenerateFile(journals);

            return File(result.Bytes, result.ContentType, $"bfj-issns.{result.FileNameExtension}");

        }
    }
}