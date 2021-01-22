using System.Web.Mvc;
using QOAM.Core.Repositories;
using QOAM.Website.Controllers;
using QOAM.Website.Helpers;

namespace QOAM.Website.Areas.BonaFide.Controllers
{
    [RouteArea("BonaFide", AreaPrefix = "bfj")]
    [RoutePrefix("")]
    [Route("{action}")]
    public class BonaFideHomeController : ApplicationController
    {
        // GET
        public BonaFideHomeController(IBaseScoreCardRepository baseScoreCardRepository, IValuationScoreCardRepository valuationScoreCardRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication) : base(baseScoreCardRepository, valuationScoreCardRepository, userProfileRepository, authentication)
        {
        }

        [HttpGet, Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}