using System;
using System.Data;
using System.IO;
using NPOI.SS.UserModel;
using QOAM.Core.Import.Invitations;
using Xunit;

namespace QOAM.Core.Tests.Import.InvitationModule
{
    public class InvitationFileImporterTests
    {
        const string MainSheet = "Authors";
        InvitationFileImporter _importer;
        IWorkbook _workbook;

        void Initialize(string fileName = "Invitation-Module - Authors")
        {
            using (var file = new FileStream($"Import\\InvitationModule\\Stubs\\{fileName}.xlsx", FileMode.Open, FileAccess.Read))
            {
                _workbook = WorkbookFactory.Create(file);
            }

            _importer = new InvitationFileImporter();
        }

        [Fact]
        public void FirstSheetIsExtractedAsMainSheet()
        {
            Initialize();

            var table = GetTable(MainSheet);

            Assert.Equal("eissn", table.Columns[0].ColumnName);
            Assert.Equal("Author email address", table.Columns[1].ColumnName);
            Assert.Equal("Author name", table.Columns[4].ColumnName);
        }
        
        [Fact]
        public void AnExceptionIsThrownIfColumnNamesAreNotValid()
        {
            Initialize("Invalid Column names - Authors");

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