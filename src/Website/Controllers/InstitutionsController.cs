namespace QOAM.Website.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Core;
    using Core.Repositories.Filters;
    using QOAM.Core.Repositories;
    using QOAM.Website.Helpers;
    using QOAM.Website.Models;
    using QOAM.Website.ViewModels.Institutions;

    using Validation;

    [RoutePrefix("institutions")]
    [Authorize(Roles = ApplicationRole.Admin + "," + ApplicationRole.DataAdmin + "," + ApplicationRole.InstitutionAdmin)]
    public class InstitutionsController : ApplicationController
    {
        private readonly IInstitutionRepository institutionRepository;

        public InstitutionsController(IInstitutionRepository institutionRepository, IBaseScoreCardRepository baseScoreCardRepository, IValuationScoreCardRepository valuationScoreCardRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication)
            : base(baseScoreCardRepository, valuationScoreCardRepository, userProfileRepository, authentication)
        {
            Requires.NotNull(institutionRepository, nameof(institutionRepository));
            
            this.institutionRepository = institutionRepository;
        }

        [HttpGet, Route("")]
        public ViewResult Index(IndexViewModel model)
        {
            model.Institutions = this.institutionRepository.Search(model.ToFilter());
            model.NumberOfBaseScoreCards = this.baseScoreCardRepository.Count(new ScoreCardFilter { State = ScoreCardState.Published });
            model.NumberOfValuationScoreCards = this.valuationScoreCardRepository.Count(new ScoreCardFilter { State = ScoreCardState.Published });

            return this.View(model);
        }

        [HttpGet, Route("{id:int}")]
        public ViewResult Details(DetailsViewModel model)
        {
            model.Institution = this.institutionRepository.Find(model.Id);
            model.UserProfiles = this.UserProfileRepository.Search(model.ToUserProfileFilter());
            model.BaseScoreCardStats = this.baseScoreCardRepository.CalculateStats(model.Institution);
            model.ValuationScoreCardStats = this.valuationScoreCardRepository.CalculateStats(model.Institution);

            return this.View(model);
        }

        [HttpGet, Route("names")]
        [OutputCache(CacheProfile = CacheProfile.OneQuarter)]
        public JsonResult Names(string query)
        {
            return this.Json(this.institutionRepository.Names(query).Select(s => new { value = s }).Take(AutoCompleteItemsCount).ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}