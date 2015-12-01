namespace QOAM.Core.Import.Licences
{
    using System.Collections.Generic;
    using System.IO;
    using NPOI.SS.UserModel;

    public class BulkImporter<T> : IBulkImporter<T>
    {
        readonly IFileImporter _fileImporter;
        readonly IImportEntityConverter<T> _entityConverter;
        IWorkbook _workbook;

        public BulkImporter(IFileImporter fileImporter, IImportEntityConverter<T> entityConverter)
        {
            _fileImporter = fileImporter;
            _entityConverter = entityConverter;
        }

        public IList<T> Execute(Stream importFile)
        {
            _workbook = WorkbookFactory.Create(importFile);

            var data = _fileImporter.Execute(_workbook);
            var convertedData = _entityConverter.Execute(data);

            return convertedData;
        }
    }
}