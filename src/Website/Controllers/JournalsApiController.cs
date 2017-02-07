using System.Linq;
using System.Web.Http;
using PagedList;
using QOAM.Core.Repositories;

namespace QOAM.Website.Controllers
{
    [RoutePrefix("api/journals")]
    public class JournalsApiController : ApiController
    {
        readonly IJournalRepository _journalRepository;

        public JournalsApiController(IJournalRepository journalRepository)
        {
            _journalRepository = journalRepository;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get(int page = 1)
        {
            _journalRepository.EnableProxyCreation = false;

            var journals = _journalRepository.All.Skip(1000 * (page - 1)).Take(1000);
            return Ok(journals);
        }
    }
}
