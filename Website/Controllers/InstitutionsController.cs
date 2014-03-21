namespace QOAM.Website.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AttributeRouting;
    using AttributeRouting.Web.Mvc;

    using QOAM.Core.Repositories;
    using QOAM.Website.Helpers;
    using QOAM.Website.Models;
    using QOAM.Website.ViewModels.Institutions;

    using Validation;

    [RoutePrefix("institutions")]
    public class InstitutionsController : ApplicationController
    {
        private readonly IInstitutionRepository institutionRepository;
        private readonly IScoreCardRepository scoreCardRepository;

        public InstitutionsController(IInstitutionRepository institutionRepository, IScoreCardRepository scoreCardRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication)
            : base(userProfileRepository, authentication)
        {
            Requires.NotNull(institutionRepository, "institutionRepository");
            Requires.NotNull(scoreCardRepository, "scoreCardRepository");
            
            this.institutionRepository = institutionRepository;
            this.scoreCardRepository = scoreCardRepository;
        }

        [GET("")]
        public ViewResult Index(IndexViewModel model)
        {
            model.Institutions = this.institutionRepository.Search(model.ToFilter());
            
            return this.View(model);
        }

        [GET("{id:int}")]
        public ViewResult Details(DetailsViewModel model)
        {
            model.Institution = this.institutionRepository.Find(model.Id);
            model.UserProfiles = this.UserProfileRepository.Search(model.ToUserProfileFilter());
            model.ScoreCardStats = this.scoreCardRepository.CalculateStats(model.Institution);

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