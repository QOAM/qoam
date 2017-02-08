using System.Linq;
using System.Web.Http;
using QOAM.Core.Repositories;
using QOAM.Website.ViewModels.Api.ScoreCard;

namespace QOAM.Website.Controllers
{
    [RoutePrefix("api/score")]
    public class ScoreCardApiController : ApiController
    {
        readonly IBaseScoreCardRepository _baseScoreCardRepository;
        readonly IValuationScoreCardRepository _valuationScoreCardRepository;

        public ScoreCardApiController(IBaseScoreCardRepository baseScoreCardRepository, IValuationScoreCardRepository valuationScoreCardRepository)
        {
            _baseScoreCardRepository = baseScoreCardRepository;
            _valuationScoreCardRepository = valuationScoreCardRepository;
        }

        [HttpGet, Route("baseScoreCards")]
        public IHttpActionResult GetBaseScoreCards(int page = 1)
        {
            var scoreCards = _baseScoreCardRepository
                .AllPublished()
                .Skip(1000 * (page - 1))
                .Take(1000)
                .Select(c => new SerializableBaseScoreCard
                {
                    Id =c.Id,
                    Journal = c.Journal.Title,
                    ISSN = c.Journal.ISSN,
                    DateExpiration = c.DateExpiration,
                    DatePublished = c.DatePublished,
                    DateStarted = c.DateStarted,
                    Remarks = c.Remarks,
                    Score = c.Score,
                    State = c.State,
                    UserName = c.UserProfile.DisplayName,
                    Email = c.UserProfile.Email,
                    Editor = c.Editor,
                    Submitted = c.Submitted
                });

            return Ok(scoreCards);
        }

        [HttpGet, Route("valuationScoreCards")]
        public IHttpActionResult GetValuationScoreCards(int page = 1)
        {
            var scoreCards = _valuationScoreCardRepository
                .AllPublished()
                .Skip(1000 * (page - 1))
                .Take(1000)
                .Select(c => new SerializableValuationScoreCard
                {
                    Id = c.Id,
                    Journal = c.Journal.Title,
                    ISSN = c.Journal.ISSN,
                    DateExpiration = c.DateExpiration,
                    DatePublished = c.DatePublished,
                    DateStarted = c.DateStarted,
                    Remarks = c.Remarks,
                    Score = c.Score,
                    State = c.State,
                    UserName = c.UserProfile.DisplayName,
                    Email = c.UserProfile.Email,
                    Editor = c.Editor,
                    Submitted = c.Submitted
                });

            return Ok(scoreCards);
        }
    }
}
