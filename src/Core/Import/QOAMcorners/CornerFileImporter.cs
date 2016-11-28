using System.Collections.Generic;
using NPOI.SS.UserModel;

namespace QOAM.Core.Import.QOAMcorners
{
    public class CornerFileImporter : FileImporterBase
    {
        protected override string MainSheet => "QOAMcorners";

        protected override void ProcessSheets(IWorkbook workbook, int mainSheetIndex)
        {
            ExtractSheet(MainSheet, workbook, "");
        }

        protected override ISheet GetSheet(IWorkbook workbook, string sheetName)
        {
            return workbook.GetSheetAt(0);
        }

        protected override void ThrowOnInvalidColumns(List<string> missingColumns)
        {
            //if (missingColumns.Any())
            //    throw new ArgumentException($"Invalid Institutions import file: Column(s) \"{missingColumns.Aggregate((a, b) => $"{a}, {b}")}\" not found.");
        }

        protected override void ValidateFile(int mainSheetIndex)
        { }
    }
}