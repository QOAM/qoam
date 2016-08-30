using System;
using System.Collections.Generic;
using System.Linq;
using NPOI.SS.UserModel;

namespace QOAM.Core.Import.Institutions
{
    public class InstitutionFileImporter : FileImporterBase
    {
        protected override string MainSheet => "Institutions";

        protected override void ProcessSheets(IWorkbook workbook, int mainSheetIndex)
        {
            ExtractSheet(MainSheet, workbook, "Institution", "Domain");
        }

        protected override ISheet GetSheet(IWorkbook workbook, string sheetName)
        {
            return workbook.GetSheetAt(0);
        }

        protected override void ThrowOnInvalidColumns(List<string> missingColumns)
        {
            if (missingColumns.Any())
                throw new ArgumentException($"Invalid Institutions import file: Column(s) \"{missingColumns.Aggregate((a, b) => $"{a}, {b}")}\" not found.");
        }

        protected override void ValidateFile(int mainSheetIndex)
        { }
    }
}