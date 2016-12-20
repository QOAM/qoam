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
        readonly ISubjectRepository _subjectRepository;

        const string ImportResultSessionKey = "ImportResult";

        public QoamCornersController(IBaseScoreCardRepository baseScoreCardRepository, IValuationScoreCardRepository valuationScoreCardRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication, IJournalRepository journalRepository, ICornerRepository cornerRepository, IBulkImporter<CornerToImport> bulkImporter, ISubjectRepository subjectRepository) 
            : base(baseScoreCardRepository, valuationScoreCardRepository, userProfileRepository, authentication)
        {
            _journalRepository = journalRepository;
            _cornerRepository = cornerRepository;
            _bulkImporter = bulkImporter;
            _subjectRepository = subjectRepository;
        }

        [HttpGet, Route("")]
        [AllowAnonymous]
        public ActionResult Index(CornersIndexViewModel model)
        {
            var corner = GetCorner(model.Corner);
            IncreaseNumberOfVisitors(corner);

            model.Disciplines = _subjectRepository.Active.Where(s => !string.IsNullOrWhiteSpace(s.Name)).ToList().ToSelectListItems("Search by discipline"); //NormalizeSearchStrings(model.Disciplines);
            model.Corners = _cornerRepository.All().OrderByDescending(c => c.NumberOfVisitors).ThenBy(c => c.Name).ToList();
            model.Journals = _cornerRepository.GetJournalsForCorner(model.ToFilter());
            model.CornerAdmin = corner?.CornerAdmin;
            model.IsCornerAdmin = VisitorIsCornerAdmin(corner);
            model.CornerName = corner?.Name;

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
                var data = _bulkImporter.Execute(model.File.InputStream);

                var importResult = new CornersImportedViewModel();

                foreach (var cornerToImport in data)
                {
                    var existingCorner = _cornerRepository.Find(cornerToImport.Name);

                    if (existingCorner == null)
                    {
                        CreateNewCorner(cornerToImport, importResult);
                    }
                    else if (VisitorIsCornerAdmin(existingCorner))
                    {
                        var journals = ParseJournalsFromIssns(cornerToImport);
                        var newJournals = journals.Where(j => existingCorner.CornerJournals.All(x => x.JournalId != j.JournalId)).ToList();
                        var toRemove = existingCorner.CornerJournals.Where(cj => journals.All(j => j.JournalId != cj.JournalId)).ToList();

                        if (!newJournals.Any() && !toRemove.Any())
                            continue;

                        UpdateCorner(newJournals, toRemove, existingCorner, importResult);
                    }
                    else if (!VisitorIsCornerAdmin(existingCorner))
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

        [HttpGet, Route("{id:int}/delete")]
        public ActionResult DeleteCorner(int id)
        {
            var corner = GetCorner(id);

            if (corner != null)
            {
                _cornerRepository.Delete(corner);
                _cornerRepository.Save();
            }

            return RedirectToAction("Index");
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

            corner.LastVisitedOn = DateTime.Now;

            // A Corner Admin should not increase the visitor count, but should probably be counted for the LastVisitedOn, so that he can keep the corner alive?
            // Sergi Papaseit 2016-12-19
            if (!VisitorIsCornerAdmin(corner))
            {
                var isUnique = corner.CornerVisitors.All(v => v.IpAddress != Request.UserHostAddress || (v.IpAddress == Request.UserHostAddress && v.VisitedOn.AddMinutes(60) <= DateTime.Now));
                
                if (isUnique)
                {
                    corner.NumberOfVisitors++;

                    corner.CornerVisitors.Add(new CornerVisitor
                    {
                        IpAddress = Request.UserHostAddress,
                        VisitedOn = corner.LastVisitedOn
                    });
                }
            }

            _cornerRepository.Save();
        }

        bool VisitorIsCornerAdmin(Corner corner)
        {
            return corner != null && corner.UserProfileId == Authentication.CurrentUserId;
        }

        void CreateNewCorner(CornerToImport cornerToImport, CornersImportedViewModel importResult)
        {
            var corner = new Corner
            {
                UserProfileId = Authentication.CurrentUserId,
                Name = cornerToImport.Name,
                CornerJournals = ParseJournalsFromIssns(cornerToImport),
                LastVisitedOn = DateTime.Now
            };

            _cornerRepository.InsertOrUpdate(corner);
            importResult.ImportedCorners.Add(corner.Name);
        }

        void UpdateCorner(IEnumerable<CornerJournal> newJournals, IEnumerable<CornerJournal> toRemove, Corner existingCorner, CornersImportedViewModel importResult)
        {
            foreach (var cornerJournal in newJournals)
                existingCorner.CornerJournals.Add(cornerJournal);

            foreach (var cornerJournal in toRemove)
                _cornerRepository.RemoveJournal(cornerJournal);

            importResult.UpdatedCorners.Add(existingCorner.Name);
        }

        #endregion
    }
}