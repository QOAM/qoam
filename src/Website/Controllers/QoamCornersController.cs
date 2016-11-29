using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using QOAM.Core;
using QOAM.Core.Import;
using QOAM.Core.Import.QOAMcorners;
using QOAM.Core.Repositories;
using QOAM.Website.Helpers;
using QOAM.Website.ViewModels.QoamCorners;

namespace QOAM.Website.Controllers
{
    [RoutePrefix("corners")]
    [Authorize]
    public class QoamCornersController : ApplicationController
    {
        readonly IJournalRepository _journalRepository;
        readonly ICornerRepository _cornerRepository;
        readonly IBulkImporter<CornerToImport> _bulkImporter;

        const string ImportResultSessionKey = "ImportResult";

        public QoamCornersController(IBaseScoreCardRepository baseScoreCardRepository, IValuationScoreCardRepository valuationScoreCardRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication, IJournalRepository journalRepository, ICornerRepository cornerRepository, IBulkImporter<CornerToImport> bulkImporter) : base(baseScoreCardRepository, valuationScoreCardRepository, userProfileRepository, authentication)
        {
            _journalRepository = journalRepository;
            _cornerRepository = cornerRepository;
            _bulkImporter = bulkImporter;
        }

        [HttpGet, Route("")]
        public ActionResult Index(CornersIndexViewModel model)
        {
            var corner = GetCorner(model.Corner);
            IncreaseNumberOfVisitors(corner);

            model.Corners = _cornerRepository.All().OrderByDescending(c => c.NumberOfVisitors).ThenBy(c => c.Name).ToList().ToSelectListItems("<Select a QOAMcorner>");
            model.Journals = _cornerRepository.GetJournalsForCorner(model.Corner.GetValueOrDefault(), model.Page, model.PageSize);
            model.CornerAdmin = corner?.CornerAdmin;

            return View(model);
        }
        
        [HttpGet, Route("create")]
        public ViewResult CreateCorner()
        {
            return View();
        }

        [HttpPost, Route("create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCorner(CornerImportViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var currentUserId = Authentication.CurrentUserId;

                var data = _bulkImporter.Execute(model.File.InputStream);

                var importResult = new CornersImportedViewModel();

                foreach (var cornerToImport in data)
                {
                    var existingCorner = _cornerRepository.Find(cornerToImport.Name);

                    if (existingCorner == null)
                    {
                        var corner = new Corner
                        {
                            UserProfileId = currentUserId,
                            Name = cornerToImport.Name,
                            CornerJournals = ParseJournalsFromIssns(cornerToImport)
                        };

                        _cornerRepository.InsertOrUpdate(corner);
                        importResult.ImportedCorners.Add(corner.Name);
                    }
                    else if (existingCorner.UserProfileId == currentUserId)
                    {
                        var newJournals = ParseJournalsFromIssns(cornerToImport).Where(j => existingCorner.CornerJournals.All(x => x.JournalId != j.JournalId)).ToList();

                        if (!newJournals.Any())
                            continue;

                        foreach (var cornerJournal in newJournals)
                            existingCorner.CornerJournals.Add(cornerJournal);

                       importResult.UpdatedCorners.Add(existingCorner.Name);
                    }
                    else if (existingCorner.UserProfileId != currentUserId)
                    {
                        importResult.ExistingCorners.Add(cornerToImport.Name);
                        continue;
                    }
                    
                    _cornerRepository.Save();
                }

                Session[ImportResultSessionKey] = importResult;

                return RedirectToAction("CornersImported");
            }
            catch (ArgumentException invalidFileException)
            {
                ModelState.AddModelError("generalError", invalidFileException.Message);
                return View(model);
            }
            catch (DbEntityValidationException)
            {
                //foreach (var eve in e.EntityValidationErrors)
                //{
                //    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                //    }
                //}
                //throw;
                return View(model);
            }
        }

        [HttpGet, Route("cornersimported")]
        public ActionResult CornersImported()
        {
            var model = (CornersImportedViewModel) Session[ImportResultSessionKey];
            return View(model);
        }

        #region Private Methods

        List<CornerJournal> ParseJournalsFromIssns(CornerToImport cornerToImport)
        {
            return (from issn in cornerToImport.ISSNs
                    let journal = _journalRepository.FindByIssn(issn)
                    where journal != null
                    select new CornerJournal
                    {
                        JournalId = journal.Id,
                    }).ToList();
        }

        Corner GetCorner(int? cornerId)
        {
            return cornerId.HasValue ? _cornerRepository.Find(cornerId.Value) : null;
        }

        void IncreaseNumberOfVisitors(Corner corner)
        {
            if (corner == null)
                return;

            corner.NumberOfVisitors++;
            corner.LastVisitedOn = DateTime.Now;
            _cornerRepository.Save();
        }

        #endregion
    }
}