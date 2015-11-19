namespace QOAM.Core.Import.Licences
{
    using System;
    using System.Data;
    using System.Linq;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    public class LicenseFileImporter : ILicenseFileImporter
    {
        private DataSet _dataSet { get; } = new DataSet();
        private const string UniversitiesSheet = "Universities";

        public DataSet Execute(IWorkbook workbook)
        {
            var universitiesSheetIndex = workbook.GetSheetIndex(UniversitiesSheet);

            if (universitiesSheetIndex == -1)
                throw new ArgumentException("Invalid QOAM import file!");

            ExtractSheet(UniversitiesSheet, workbook, "Domein", "Tabbladen");

            for (var i = 0; i < workbook.NumberOfSheets; i++)
            {
                if (i == universitiesSheetIndex)
                    continue;

                ExtractSheet(workbook.GetSheetName(i), workbook, "ISSN", "Text");
            }

            return _dataSet;
        }

        private void ExtractSheet(string sheetName, IWorkbook workbook, params string[] expectedColumns)
        {
            var dt = new DataTable(sheetName);

            var sheet = workbook.GetSheet(sheetName);
            var headerRow = sheet.GetRow(0);
            var rows = sheet.GetRowEnumerator();

            int colCount = headerRow.LastCellNum;

            for (var c = 0; c < colCount; c++)
                dt.Columns.Add(headerRow.GetCell(c).ToString());

            dt.CaseSensitive = false;

            ValidateColumnNames(expectedColumns, dt);

            //skip header row
            rows.MoveNext();

            while (rows.MoveNext())
            {
                IRow row = (XSSFRow) rows.Current;
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

        private static void ValidateColumnNames(string[] expectedColumns, DataTable dt)
        {
            var missingColumns = expectedColumns.Where(column => !dt.Columns.Contains(column)).ToList();

            if (missingColumns.Any())
                throw new ArgumentException($"Invalid QOAM import file: Column(s) \"{missingColumns.Aggregate((a, b) => $"{a}, {b}")}\" not found.");
        }
    }
}