using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using DCommon.LinqToNPOI;
using NPOI.HSSF.Extractor;
using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using QOAM.Core.Import.Licences;
using Xunit;

namespace QOAM.Core.Tests.Import.Licenses
{
    public class ExcelParserTests
    {
        LicenseImporter _importer;
        IWorkbook _workbook;

        void Initialize()
        {
            using (var file = new FileStream(@"Import\Licenses\Stubs\QOAMupload.xlsx", FileMode.Open, FileAccess.Read))
            {
                _workbook = WorkbookFactory.Create(file);
            }

            _importer = new LicenseImporter();
        }

        [Fact]
        public void ExcelContentsAreRead()
        {
            Initialize();
            var result = _importer.Extract(_workbook);
            Assert.NotNull(result);

            Assert.True(result.Rows.Count == 4);
        }
    }

    public class UniversityLicense
    {
        public string Domain { get; set; }
    }

    public class LicenseImporter
    {
        public DataTable Extract(IWorkbook workbook)
        {
            //var exporter = ExcelBuilder.Create();

            //exporter.
            // from row in dt.AsEnumerable()
            return xlsxToDT(workbook);
        }

        static DataTable xlsxToDT(IWorkbook workbook)
        {
            var dt = new DataTable();

            var sheet = workbook.GetSheet("Univ");
            var headerRow = sheet.GetRow(0);
            var rows = sheet.GetRowEnumerator();

            int colCount = headerRow.LastCellNum;
            int rowCount = sheet.LastRowNum;

            for (int c = 0; c < colCount; c++)
            {

                dt.Columns.Add(headerRow.GetCell(c).ToString());
            }

            //skip header row
            rows.MoveNext();
            
            while (rows.MoveNext())
            {
                IRow row = (XSSFRow)rows.Current;
                var dr = dt.NewRow();

                for (var i = 0; i < colCount; i++)
                {
                    var cell = row.GetCell(i);

                    if (cell != null)
                        dr[i] = cell.ToString();
                }
                dt.Rows.Add(dr);
            }

            workbook = null;
            sheet = null;
            return dt;
        }
    }
}