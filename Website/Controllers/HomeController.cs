namespace QOAM.Website.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using AttributeRouting.Web.Mvc;

    using QOAM.Core.Repositories;
    using QOAM.Core.Services;
    using QOAM.Website.Helpers;
    using QOAM.Website.Models;
    using QOAM.Website.Resources;
    using QOAM.Website.ViewModels.Home;

    using Validation;

    public class HomeController : ApplicationController
    {
        private readonly IMailSender mailSender;
        private readonly ContactSettings contactSettings;
        private readonly IJournalRepository journalRepository;

        public HomeController(IJournalRepository journalRepository, IMailSender mailSender, ContactSettings contactSettings, IUserProfileRepository userProfileRepository, IAuthentication authentication)
            : base(userProfileRepository, authentication)
        {
            Requires.NotNull(mailSender, "mailSender");
            Requires.NotNull(contactSettings, "contactSettings");

            this.journalRepository = journalRepository;
            this.mailSender = mailSender;
            this.contactSettings = contactSettings;
        }

        [GET("/")]
        public ViewResult Index()
        {
            return this.View(new IndexViewModel { NumberOfScoredJournals = this.journalRepository.ScoredJournalsCount() });
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
                    this.ModelState.AddModelError("mailsender", "An error occured while trying to process the contact form. Please try again.");
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