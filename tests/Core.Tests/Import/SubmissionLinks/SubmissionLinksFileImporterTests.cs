using System;
using System.Data;
using System.IO;
using NPOI.SS.UserModel;
using QOAM.Core.Import.SubmissionLinks;
using Xunit;

namespace QOAM.Core.Tests.Import.SubmissionLinks
{
    public class SubmissionLinksFileImporterTests
    {
        const string MainSheet = "Links";
        SubmissionLinksFileImporter _importer;
        IWorkbook _workbook;

        void Initialize(string fileName = "Submission-links")
        {
            using (var file = new FileStream($"Import\\SubmissionLinks\\Stubs\\{fileName}.xlsx", FileMode.Open, FileAccess.Read))
            {
                _workbook = WorkbookFactory.Create(file);
            }

            _importer = new SubmissionLinksFileImporter();
        }

        [Fact]
        public void FirstSheetIsExtractedAsMainSheet()
        {
            Initialize();

            var table = GetTable(MainSheet);

            Assert.Equal("eissn", table.Columns[0].ColumnName);
            Assert.Equal("url", table.Columns[1].ColumnName);
        }

        [Fact]
        public void AnExceptionIsThrownIfColumnNamesAreNotValid()
        {
            Initialize("Invalid column names -Submission-links");

            Assert.Throws<ArgumentException>(() => _importer.Execute(_workbook));
        }

        #region Private Methods

        DataTable GetTable(string name)
        {
            var result = _importer.Execute(_workbook);
            return result.Tables[name];
        }

        #endregion

    }
}