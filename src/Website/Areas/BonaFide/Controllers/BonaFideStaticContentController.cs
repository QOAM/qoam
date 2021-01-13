using System.Web.Mvc;
using QOAM.Core.Repositories;
using QOAM.Website.Controllers;
using QOAM.Website.Helpers;

namespace QOAM.Website.Areas.BonaFide.Controllers
{
    [RouteArea("BonaFide", AreaPrefix = "bfj")]
    [RoutePrefix("")]
    [Route("{action}")]
    public class BonaFideStaticContentController : ApplicationController
    {
        // GET
        public BonaFideStaticContentController(IBaseScoreCardRepository baseScoreCardRepository, IValuationScoreCardRepository valuationScoreCardRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication) : base(baseScoreCardRepository, valuationScoreCardRepository, userProfileRepository, authentication)
        {
        }

        [HttpGet, Route("why")]
        public ActionResult Why()
        {
            return View();
        }

        [HttpGet, Route("how")]
        public ActionResult How()
        {
            return View();
        }
    }
}