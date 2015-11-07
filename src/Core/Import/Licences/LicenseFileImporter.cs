using System;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace QOAM.Core.Import.Licences
{
    public class LicenseFileImporter : ILicenseFileImporter
    {
        DataSet _dataSet { get; } = new DataSet();
        const string UniversitiesSheet = "Universities";
        
        public DataSet Extract(IWorkbook workbook)
        {
            var universitiesSheetIndex = workbook.GetSheetIndex(UniversitiesSheet);

            if(universitiesSheetIndex == -1)
                throw new ArgumentException("Invalid QOAM import file!");

            ExtractSheet(UniversitiesSheet, workbook);
            
            for (var i = 0; i < workbook.NumberOfSheets; i++)
            {
                if(i == universitiesSheetIndex)
                    continue;
                
                ExtractSheet(workbook.GetSheetName(i), workbook);
            }

            return _dataSet;
        }

        void ExtractSheet(string sheetName, IWorkbook workbook)
        {
            var dt = new DataTable(sheetName);

            var sheet = workbook.GetSheet(sheetName);
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