namespace QOAM.Website.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AttributeRouting;
    using AttributeRouting.Web.Mvc;
    using Core;
    using Core.Repositories.Filters;
    using QOAM.Core.Repositories;
    using QOAM.Website.Helpers;
    using QOAM.Website.Models;
    using QOAM.Website.ViewModels.Institutions;

    using Validation;

    [RoutePrefix("institutions")]
    public class InstitutionsController : ApplicationController
    {
        private readonly IInstitutionRepository institutionRepository;
        private readonly IBaseScoreCardRepository baseScoreCardRepository;
        private readonly IValuationScoreCardRepository valuationScoreCardRepository;

        public InstitutionsController(IInstitutionRepository institutionRepository, IBaseScoreCardRepository baseScoreCardRepository, IValuationScoreCardRepository valuationScoreCardRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication)
            : base(userProfileRepository, authentication)
        {
            Requires.NotNull(institutionRepository, nameof(institutionRepository));
            Requires.NotNull(baseScoreCardRepository, nameof(baseScoreCardRepository));
            Requires.NotNull(valuationScoreCardRepository, nameof(valuationScoreCardRepository));
            
            this.institutionRepository = institutionRepository;
            this.baseScoreCardRepository = baseScoreCardRepository;
            this.valuationScoreCardRepository = valuationScoreCardRepository;
        }

        [GET("")]
        public ViewResult Index(IndexViewModel model)
        {
            model.Institutions = this.institutionRepository.Search(model.ToFilter());
            model.NumberOfBaseScoreCards = this.baseScoreCardRepository.Count(new ScoreCardFilter { State = ScoreCardState.Published });
            model.NumberOfValuationScoreCards = this.valuationScoreCardRepository.Count(new ScoreCardFilter { State = ScoreCardState.Published });

            return this.View(model);
        }

        [GET("{id:int}")]
        public ViewResult Details(DetailsViewModel model)
        {
            model.Institution = this.institutionRepository.Find(model.Id);
            model.UserProfiles = this.UserProfileRepository.Search(model.ToUserProfileFilter());
            model.BaseScoreCardStats = this.baseScoreCardRepository.CalculateStats(model.Institution);
            model.ValuationScoreCardStats = this.valuationScoreCardRepository.CalculateStats(model.Institution);

            return this.View(model);
        }

        [GET("names")]
        [OutputCache(CacheProfile = CacheProfile.OneQuarter)]
        public JsonResult Names(string query)
        {
            return this.Json(this.institutionRepository.Names(query).Select(s => new { value = s }).Take(AutoCompleteItemsCount).ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}