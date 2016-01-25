using System;
using System.Collections.Generic;
using System.Linq;
using NPOI.SS.UserModel;

namespace QOAM.Core.Import.SubmissionLinks
{
    public class SubmissionLinksFileImporter : FileImporterBase
    {
        protected override string MainSheet => "Links";

        protected override void ProcessSheets(IWorkbook workbook, int mainSheetIndex)
        {
            ExtractSheet(MainSheet, workbook, "eissn", "url");
        }

        protected override int GetMainSheetIndex(IWorkbook workbook)
        {
            return 0;
        }

        protected override ISheet GetSheet(IWorkbook workbook, string sheetName)
        {
            return workbook.GetSheetAt(0);
        }

        protected override void ThrowOnInvalidColumns(List<string> missingColumns)
        {
            if (missingColumns.Any())
                throw new ArgumentException($"Invalid Submission Links import file: Column(s) \"{missingColumns.Aggregate((a, b) => $"{a}, {b}")}\" not found.");
        }

        protected override void ValidateFile(int mainSheetIndex)
        { }
    }
}