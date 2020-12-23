using System.Linq;
using System.Web.Http;
using QOAM.Core.Repositories;
using QOAM.Website.ModelExtensions;
using QOAM.Website.ViewModels.Api;

namespace QOAM.Website.Areas.BonaFide.Controllers
{
    [RoutePrefix("api/bfj")]
    [Route("{action}")]
    public class BonaFideApiController : ApiController
    {
        readonly IJournalRepository _journalRepository;

        public BonaFideApiController(IJournalRepository journalRepository)
        {
            _journalRepository = journalRepository;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get(int page = 1)
        {
            //_trustedJournalRepository.EnableProxyCreation = false;

            var journals = _journalRepository.AllWhere(j => j.NoFee || j.DataSource == "DOAJ" || j.TrustingInstitutions.Any() || j.InstitutionJournalPrices.Any())
                .Skip(1000 * (page - 1))
                .Take(1000)
                .Select(j => new BfjApiDto
                {
                    eISSN = j.ISSN,
                    Title = j.Title,
                    Text = j.BonaFideText()
                });

            return Ok(journals);
        }
    }
}