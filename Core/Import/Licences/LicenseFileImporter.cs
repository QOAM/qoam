using System;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace QOAM.Core.Import.Licences
{
    public class LicenseFileImporter : ILicenseFileImporter
    {
        readonly IWorkbook _workbook;
        const string UniversitiesSheet = "Universities";

        public LicenseFileImporter(IWorkbook workbook)
        {
            _workbook = workbook;
        }

        DataSet _dataSet { get; } = new DataSet();

        public DataSet Extract()
        {
            var universitiesSheetIndex = _workbook.GetSheetIndex(UniversitiesSheet);

            if(universitiesSheetIndex == -1)
                throw new ArgumentException("Invalid QOAM import file!");

            ExtractSheet(UniversitiesSheet);
            
            for (var i = 0; i < _workbook.NumberOfSheets; i++)
            {
                if(i == universitiesSheetIndex)
                    continue;
                
                ExtractSheet(_workbook.GetSheetName(i));
            }

            return _dataSet;
        }

        void ExtractSheet(string sheetName)
        {
            var dt = new DataTable(sheetName);

            var sheet = _workbook.GetSheet(sheetName);
            var headerRow = sheet.GetRow(0);
            var rows = sheet.GetRowEnumerator();

            int colCount = headerRow.LastCellNum;
            //var rowCount = sheet.LastRowNum;

            for (var c = 0; c < colCount; c++)
                dt.Columns.Add(headerRow.GetCell(c).ToString());

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

            _dataSet.Tables.Add(dt);
        }
    }
}