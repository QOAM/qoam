namespace QOAM.Core.Tests.Import.Licenses
{
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;
    using Core.Import.Licences;
    using NPOI.SS.UserModel;
    using Xunit;

    public class LicenseFileImporterTests
    {
        private LicenseFileImporter _importer;
        private IWorkbook _workbook;

        private const string UnivertitiesTab = "Universities";

        private void Initialize(string fileName = "QOAMupload")
        {
            using (var file = new FileStream($"Import\\Licenses\\Stubs\\{fileName}.xlsx", FileMode.Open, FileAccess.Read))
            {
                _workbook = WorkbookFactory.Create(file);
            }

            _importer = new LicenseFileImporter();
        }

        [Fact]
        public void TableHeadersAreExtractedFromSheet()
        {
            Initialize();

            var table = GetTable(UnivertitiesTab);

            Assert.Equal("Domein", table.Columns[0].ColumnName);
            Assert.Equal("Tabbladen", table.Columns[1].ColumnName);
        }

        [Fact]
        public void UniversitiesTableIsPresentInDataSet()
        {
            Initialize();

            var table = GetTable(UnivertitiesTab);

            Assert.NotNull(table);

            Assert.Equal(4, table.Rows.Count);
        }

        [Fact]
        public void SheetsOtherThanUniversitiesAreExtractedAsDataTables()
        {
            Initialize();

            var result = _importer.Execute(_workbook);

            Assert.Equal(2, result.Tables.Cast<DataTable>().Count(dt => dt.TableName != UnivertitiesTab));
        }

        [Fact]
        public void AnExceptionIsThrownIfUniversitiesTableIsNotPresentInFile()
        {
            Initialize("Missing main Sheet - QOAMupload");

            Assert.Throws<ArgumentException>(() => _importer.Execute(_workbook));
        }

        [Fact]
        public void AnExceptionIsThrownIfColumnNamesAreNotValid()
        {
            Initialize("Invalid Column names - QOAMupload");

            Assert.Throws<ArgumentException>(() => _importer.Execute(_workbook));
        }

        #region Private Methods

        private DataTable GetTable(string name)
        {
            var result = _importer.Execute(_workbook);
            return result.Tables[name];
        }

        #endregion
    }
}