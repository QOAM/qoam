namespace QOAM.Website.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Core.Repositories.Filters;
    using QOAM.Core;
    using QOAM.Core.Repositories;
    using QOAM.Website.Helpers;
    using QOAM.Website.Models;
    using QOAM.Website.ViewModels.Profiles;

    using Validation;

    [RoutePrefix("profiles")]
    public class ProfilesController : ApplicationController
    {
        private readonly IInstitutionRepository institutionRepository;
        private readonly IRoles roles;

        public ProfilesController(IInstitutionRepository institutionRepository, IBaseScoreCardRepository baseScoreCardRepository, IValuationScoreCardRepository valuationScoreCardRepository, IRoles roles, IUserProfileRepository userProfileRepository, IAuthentication authentication)
            : base(baseScoreCardRepository, valuationScoreCardRepository, userProfileRepository, authentication)
        {
            Requires.NotNull(institutionRepository, nameof(institutionRepository));
            Requires.NotNull(roles, nameof(roles));
            
            this.institutionRepository = institutionRepository;
            this.roles = roles;
        }

        [HttpGet, Route("")]
        public ViewResult Index(IndexViewModel model)
        {
            model.Institutions = this.institutionRepository.All.ToSelectListItems("<All institutions>");
            model.Profiles = this._userProfileRepository.Search(model.ToFilter());
            model.NumberOfBaseScoreCards = this.baseScoreCardRepository.Count(new ScoreCardFilter { State = ScoreCardState.Published });
            model.NumberOfValuationScoreCards = this.valuationScoreCardRepository.Count(new ScoreCardFilter { State = ScoreCardState.Published });

            return this.View(model);
        }

        [HttpGet, Route("{id:int}")]
        public ViewResult Details(DetailsViewModel model)
        {
            model.UserProfile = this._userProfileRepository.Find(model.Id);
            model.BaseScoreCards = this.baseScoreCardRepository.FindForUser(model.ToScoreCardFilter(this.GetScoreCardStateFilter(model.Id)));
            model.BaseScoreCardStats = this.baseScoreCardRepository.CalculateStats(model.UserProfile);
            model.ValuationScoreCards = this.valuationScoreCardRepository.FindForUser(model.ToScoreCardFilter(this.GetScoreCardStateFilter(model.Id)));
            model.ValuationScoreCardStats = this.valuationScoreCardRepository.CalculateStats(model.UserProfile);

            return this.View(model);
        }

        [HttpGet, Route("{id:int}/edit")]
        [Authorize(Roles = ApplicationRole.Admin)]
        public ViewResult Edit(int id, string returnUrl)
        {
            var model = new EditViewModel
                            {
                                UserProfile = this._userProfileRepository.Find(id),
                                ReturnUrl = returnUrl
                            };
            
            var currentRoles = this.roles.GetRolesForUser(model.UserProfile.UserName).ToList();

            model.IsAdmin = currentRoles.Contains(ApplicationRole.Admin);
            model.IsInstitutionAdmin = currentRoles.Contains(ApplicationRole.InstitutionAdmin);
            model.IsDataAdmin = currentRoles.Contains(ApplicationRole.DataAdmin);
            model.IsDeveloper = currentRoles.Contains(ApplicationRole.Developer);
            
            return this.View(model);
        }

        [HttpPost, Route("{id:int}/edit")]
        [Authorize(Roles = ApplicationRole.Admin)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditViewModel model)
        {
            model.UserProfile = this._userProfileRepository.Find(id);

            if (this.ModelState.IsValid)
            {
                this.roles.UpdateUserRoles(model.UserProfile.UserName, model.Roles);

                return this.Redirect(model.ReturnUrl);
            }

            return this.View(model);
        }

        [HttpGet, Route("{id:int}/delete")]
        [Authorize(Roles = ApplicationRole.Admin + "," + ApplicationRole.DataAdmin)]
        public ActionResult DeleteProfile(int id, string returnUrl)
        {
            var profile = _userProfileRepository.Find(id);

            if (profile != null)
            {
                _userProfileRepository.Delete(profile);
                _userProfileRepository.Save();
            }

            return Redirect(returnUrl);
        }

        [HttpGet, Route("{id:int}/basescorecards")]
        public PartialViewResult BaseScoreCards(ScoreCardsViewModel model)
        {
            var scoreCards = this.baseScoreCardRepository.FindForUser(model.ToScoreCardFilter(this.GetScoreCardStateFilter(model.Id)));

            return this.PartialView(scoreCards);
        }

        [HttpGet, Route("{id:int}/valuationscorecards")]
        public PartialViewResult ValuationScoreCards(ScoreCardsViewModel model)
        {
            var scoreCards = this.valuationScoreCardRepository.FindForUser(model.ToScoreCardFilter(this.GetScoreCardStateFilter(model.Id)));

            return this.PartialView(scoreCards);
        }

        [HttpGet, Route("names")]
        [OutputCache(CacheProfile = CacheProfile.OneQuarter)]
        public JsonResult Names(string query)
        {
            return this.Json(this._userProfileRepository.Names(query).Select(s => new { value = s }).Take(AutoCompleteItemsCount).ToList(), JsonRequestBehavior.AllowGet);
        }

        private ScoreCardState? GetScoreCardStateFilter(int userProfileId)
        {
            return this.IsIdOfAuthenticatedUser(userProfileId) ? (ScoreCardState?)null : ScoreCardState.Published;
        }

        private bool IsIdOfAuthenticatedUser(int id)
        {
            return id == this.Authentication.CurrentUserId;
        }
    }
}