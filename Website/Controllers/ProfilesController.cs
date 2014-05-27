namespace QOAM.Website.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AttributeRouting;
    using AttributeRouting.Web.Mvc;

    using PagedList;

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
        private readonly IScoreCardRepository scoreCardRepository;
        private readonly IRoles roles;

        public ProfilesController(IInstitutionRepository institutionRepository, IScoreCardRepository scoreCardRepository, IRoles roles, IUserProfileRepository userProfileRepository, IAuthentication authentication)
            : base(userProfileRepository, authentication)
        {
            Requires.NotNull(institutionRepository, "institutionRepository");
            Requires.NotNull(scoreCardRepository, "scoreCardRepository");
            Requires.NotNull(roles, "roles");
            
            this.institutionRepository = institutionRepository;
            this.scoreCardRepository = scoreCardRepository;
            this.roles = roles;
        }

        [GET("")]
        public ViewResult Index(IndexViewModel model)
        {
            model.Institutions = this.institutionRepository.All.ToSelectListItems("<All institutions>");
            model.Profiles = this.UserProfileRepository.Search(model.ToFilter());
            
            return this.View(model);
        }

        [GET("{id:int}")]
        public ViewResult Details(DetailsViewModel model)
        {
            model.UserProfile = this.UserProfileRepository.Find(model.Id);
            model.ScoreCards = new StaticPagedList<ScoreCard>(new ScoreCard[0], 1, 1, 0);
            model.ScoreCards = this.scoreCardRepository.FindForUser(model.ToScoreCardFilter(this.GetScoreCardStateFilter(model.Id)));
            model.ScoreCardStats = this.scoreCardRepository.CalculateStats(model.UserProfile);

            return this.View(model);
        }

        [GET("{id:int}/edit")]
        [Authorize(Roles = ApplicationRole.Admin)]
        public ViewResult Edit(int id, string returnUrl)
        {
            var model = new EditViewModel
                            {
                                UserProfile = this.UserProfileRepository.Find(id),
                                ReturnUrl = returnUrl
                            };
            
            var currentRoles = this.roles.GetRolesForUser(model.UserProfile.UserName).ToList();

            model.IsAdmin = currentRoles.Contains(ApplicationRole.Admin);
            model.IsInstitutionAdmin = currentRoles.Contains(ApplicationRole.InstitutionAdmin);
            model.IsDataAdmin = currentRoles.Contains(ApplicationRole.DataAdmin);
            
            return this.View(model);
        }

        [POST("{id:int}/edit")]
        [Authorize(Roles = ApplicationRole.Admin)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditViewModel model)
        {
            model.UserProfile = this.UserProfileRepository.Find(id);

            if (this.ModelState.IsValid)
            {
                this.roles.UpdateUserRoles(model.UserProfile.UserName, model.Roles);

                return this.Redirect(model.ReturnUrl);
            }

            return this.View(model);
        }

        [GET("{id:int}/scorecards")]
        public PartialViewResult ScoreCards(ScoreCardsViewModel model)
        {
            var scoreCards = this.scoreCardRepository.FindForUser(model.ToScoreCardFilter(this.GetScoreCardStateFilter(model.Id)));

            return this.PartialView(scoreCards);
        }

        [GET("names")]
        [OutputCache(CacheProfile = CacheProfile.OneQuarter)]
        public JsonResult Names(string query)
        {
            return this.Json(this.UserProfileRepository.Names(query).Select(s => new { value = s }).Take(AutoCompleteItemsCount).ToList(), JsonRequestBehavior.AllowGet);
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