using System.Collections.Generic;
using System.Data;
using System.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace QOAM.Core.Import
{
    public abstract class FileImporterBase : IFileImporter
    {
        DataSet _dataSet { get; } = new DataSet();
        protected abstract string MainSheet { get; }
        protected virtual string OptionalColumnPrefixIndicator => "?";

        public DataSet Execute(IWorkbook workbook)
        {
            var mainSheetIndex = GetMainSheetIndex(workbook);

            ValidateFile(mainSheetIndex);

            ProcessSheets(workbook, mainSheetIndex);

            return _dataSet;
        }

        protected virtual int GetMainSheetIndex(IWorkbook workbook)
        {
            return workbook.GetSheetIndex(MainSheet);
        }

        protected void ExtractSheet(string sheetName, IWorkbook workbook, params string[] expectedColumns)
        {
            var dt = new DataTable(sheetName);

            var sheet = GetSheet(workbook, sheetName);
            var headerRow = sheet.GetRow(0);

            if (headerRow == null)
                return;

            var rows = sheet.GetRowEnumerator();

            int colCount = headerRow.LastCellNum;

            for (var c = 0; c < colCount; c++)
                dt.Columns.Add(headerRow.GetCell(c)?.ToString().Trim());

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
                        dr[i] = cell.ToString().Trim();
                }

                if (!IsEmpty(dr))
                    dt.Rows.Add(dr);
            }

            _dataSet.Tables.Add(dt);
        }

        protected virtual ISheet GetSheet(IWorkbook workbook, string sheetName)
        {
            return workbook.GetSheet(sheetName);
        }

        protected abstract void ProcessSheets(IWorkbook workbook, int mainSheetIndex);
        protected abstract void ValidateFile(int mainSheetIndex);
        protected void ValidateColumnNames(string[] expectedColumns, DataTable dt)
        {
            var missingColumns = expectedColumns.Where(column => !column.StartsWith(OptionalColumnPrefixIndicator) && !dt.Columns.Contains(column)).ToList();

            ThrowOnInvalidColumns(missingColumns);
        }

        protected abstract void ThrowOnInvalidColumns(List<string> missingColumns);

        static bool IsEmpty(DataRow dr)
        {
            return dr == null || dr.ItemArray.All(value => string.IsNullOrWhiteSpace(value?.ToString()));
        }
    }
}