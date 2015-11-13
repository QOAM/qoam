using System;
using System.Data;
using System.IO;
using System.Linq;
using NPOI.SS.UserModel;
using QOAM.Core.Import.Licences;
using Xunit;

namespace QOAM.Core.Tests.Import.Licenses
{
    public class LicenseFileImporterTests
    {
        LicenseFileImporter _importer;
        IWorkbook _workbook;

        const string UnivertitiesTab = "Universities";

        void Initialize(string fileName = "QOAMupload")
        {
            // from row in dt.AsEnumerable()


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
            Initialize("Invalid QOAMupload");

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