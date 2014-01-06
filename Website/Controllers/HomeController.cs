namespace RU.Uci.OAMarket.Website.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using AttributeRouting.Web.Mvc;

    using RU.Uci.OAMarket.Domain.Repositories;
    using RU.Uci.OAMarket.Domain.Services;
    using RU.Uci.OAMarket.Website.Helpers;
    using RU.Uci.OAMarket.Website.Models;
    using RU.Uci.OAMarket.Website.Resources;
    using RU.Uci.OAMarket.Website.ViewModels.Home;

    using Validation;

    public class HomeController : ApplicationController
    {
        private readonly IMailSender mailSender;
        private readonly ContactSettings contactSettings;
        private readonly IScoreCardRepository scoreCardRepository;

        public HomeController(IScoreCardRepository scoreCardRepository, IMailSender mailSender, ContactSettings contactSettings, IUserProfileRepository userProfileRepository, IAuthentication authentication)
            : base(userProfileRepository, authentication)
        {
            Requires.NotNull(mailSender, "mailSender");
            Requires.NotNull(contactSettings, "contactSettings");

            this.scoreCardRepository = scoreCardRepository;
            this.mailSender = mailSender;
            this.contactSettings = contactSettings;
        }

        [GET("/")]
        public ViewResult Index()
        {
            return this.View(new IndexViewModel { NumberOfJournalScoreCards = this.scoreCardRepository.PublishedScoreCardsCount() });
        }

        [GET("about")]
        public ViewResult About()
        {
            return this.View();
        }

        [GET("organisation")]
        public ViewResult Organisation()
        {
            return this.View();
        }

        [GET("press")]
        public ViewResult Press()
        {
            return this.View();
        }

        [GET("faq")]
        public ViewResult Faq()
        {
            return this.View();
        }

        [GET("journalscorecard")]
        public ViewResult JournalScoreCard()
        {
            return this.View();
        }

        [GET("contact")]
        public ViewResult Contact()
        {
            return this.View();
        }

        [POST("contact")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(ContactViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    await this.mailSender.Send(model.ToMailMessage(this.contactSettings));

                    return this.RedirectToAction("ContactSent");
                }
                catch
                {
                    this.ModelState.AddModelError("mailsender", ViewStrings.Home_Contact_Form_Error);
                }
            }

            return this.View();
        }

        [GET("contact/sent")]
        public ViewResult ContactSent()
        {
            return this.View();
        }
    }
}