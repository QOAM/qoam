using System.IO;
using NPOI.SS.UserModel;

namespace QOAM.Core.Import.Licences
{
    public class BulkImporter
    {
        readonly ILicenseFileImporter _fileImporter;
        readonly IImportEntityConverter _entityConverter;
        IWorkbook _workbook;

        public BulkImporter(ILicenseFileImporter fileImporter, IImportEntityConverter entityConverter)
        {
            _fileImporter = fileImporter;
            _entityConverter = entityConverter;
        }

        public void StartImport(Stream importFile)
        {
            _workbook = WorkbookFactory.Create(importFile);

            var data = _fileImporter.Extract(_workbook);

            var convertedData = _entityConverter.Convert(data);

            // TODO: Should this method return a List<InstitutionJournal>? Is there a nice pattern for this?
            // TODO: Chain of Responsibility maybe?
        }
    }
}