namespace QOAM.Website.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using QOAM.Core.Repositories;
    using QOAM.Core.Services;
    using QOAM.Website.Helpers;
    using QOAM.Website.Models;
    using QOAM.Website.ViewModels.Home;

    using Validation;

    public class HomeController : ApplicationController
    {
        private readonly IMailSender mailSender;
        private readonly ContactSettings contactSettings;
        private readonly IJournalRepository journalRepository;

        public HomeController(
            IBaseScoreCardRepository baseScoreCardRepository, 
            IValuationScoreCardRepository valuationScoreCardRepository, 
            IJournalRepository journalRepository, IMailSender mailSender, 
            ContactSettings contactSettings, 
            IUserProfileRepository userProfileRepository, 
            IAuthentication authentication)
            : base(baseScoreCardRepository, valuationScoreCardRepository, userProfileRepository, authentication)
        {
            Requires.NotNull(journalRepository, nameof(journalRepository));
            Requires.NotNull(mailSender, nameof(mailSender));
            Requires.NotNull(contactSettings, nameof(contactSettings));

            this.journalRepository = journalRepository;
            this.mailSender = mailSender;
            this.contactSettings = contactSettings;
        }

        [HttpGet, Route("")]
        public ViewResult Index()
        {
            var model = new IndexViewModel
            {
                NumberOfScoredJournals = this.journalRepository.ScoredJournalsCount()
            };
            return this.View(model);
        }

        [HttpGet, Route("about")]
        public ViewResult About()
        {
            return this.View();
        }

        [HttpGet, Route("organisation")]
        public ViewResult Organisation()
        {
            return this.View();
        }

        [HttpGet, Route("press")]
        public ViewResult Press()
        {
            return this.View();
        }

        [HttpGet, Route("faq")]
        public ViewResult Faq()
        {
            return this.View();
        }

        [HttpGet, Route("journalscorecard")]
        public ViewResult JournalScoreCard()
        {
            return this.View();
        }

        [HttpGet, Route("contact")]
        public ViewResult Contact()
        {
            return this.View();
        }

        [HttpPost, Route("contact")]
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

        [HttpGet, Route("contact/sent")]
        public ViewResult ContactSent()
        {
            return this.View();
        }
    }
}