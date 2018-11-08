using System.Threading.Tasks;
using System.Web.Mvc;
using QOAM.Core.Repositories;
using QOAM.Core.Services;
using QOAM.Website.Helpers;
using QOAM.Website.Models;
using QOAM.Website.ViewModels.Home;
using Validation;

namespace QOAM.Website.Controllers
{
    public class HomeController : ApplicationController
    {
        readonly IMailSender _mailSender;
        readonly ContactSettings _contactSettings;
        readonly IJournalRepository _journalRepository;

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

            _journalRepository = journalRepository;
            _mailSender = mailSender;
            _contactSettings = contactSettings;
        }

        [HttpGet, Route("")]
        public ViewResult Index()
        {
            var model = new IndexViewModel
            {
                NumberOfScoredJournals = _journalRepository.BaseScoredJournalsCount()
            };
            return View(model);
        }

        [HttpGet, Route("about")]
        public ViewResult About()
        {
            return View();
        }

        [HttpGet, Route("organisation")]
        public ViewResult Organisation()
        {
            return View();
        }

        [HttpGet, Route("references")]
        public ViewResult References()
        {
            return View();
        }

        [HttpGet, Route("faq")]
        public ViewResult Faq()
        {
            return View();
        }


        [HttpGet, Route("tools")]
        public ViewResult Tools()
        {
            return View();
        }

        [HttpGet, Route("journalscorecard")]
        public ViewResult JournalScoreCard()
        {
            return View();
        }

        [HttpGet, Route("contact")]
        public ViewResult Contact()
        {
            return View();
        }

        [HttpPost, Route("contact")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _mailSender.Send(model.ToMailMessage(_contactSettings));

                    return RedirectToAction("ContactSent");
                }
                catch
                {
                    ModelState.AddModelError("mailsender", "An error occured while trying to process the contact form. Please try again.");
                }
            }

            return View();
        }

        [HttpGet, Route("contact/sent")]
        public ViewResult ContactSent()
        {
            return View();
        }
    }
}