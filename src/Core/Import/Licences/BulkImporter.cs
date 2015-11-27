namespace QOAM.Core.Import.Licences
{
    using System.Collections.Generic;
    using System.IO;
    using NPOI.SS.UserModel;

    public class BulkImporter : IBulkImporter
    {
        private readonly ILicenseFileImporter _fileImporter;
        private readonly IImportEntityConverter _entityConverter;
        private IWorkbook _workbook;

        public BulkImporter(ILicenseFileImporter fileImporter, IImportEntityConverter entityConverter)
        {
            _fileImporter = fileImporter;
            _entityConverter = entityConverter;
        }

        public IList<UniversityLicense> Execute(Stream importFile)
        {
            _workbook = WorkbookFactory.Create(importFile);

            var data = _fileImporter.Execute(_workbook);
            var convertedData = _entityConverter.Execute(data);

            return convertedData;
        }
    }
}