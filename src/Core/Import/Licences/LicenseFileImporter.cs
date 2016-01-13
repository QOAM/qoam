using System.Collections.Generic;

namespace QOAM.Core.Import.Licences
{
    using System;
    using System.Linq;
    using NPOI.SS.UserModel;

    public class LicenseFileImporter : FileImporterBase
    {
        protected override string MainSheet => "Universities";

        protected override void ProcessSheets(IWorkbook workbook, int mainSheetIndex)
        {
            ExtractSheet(MainSheet, workbook, "Domain", "Tabs");

            for (var i = 0; i < workbook.NumberOfSheets; i++)
            {
                if (i == mainSheetIndex)
                    continue;

                ExtractSheet(workbook.GetSheetName(i), workbook, "ISSN", "Text");
            }
        }

        protected override void ValidateFile(int mainSheetIndex)
        {
            if (mainSheetIndex == -1)
                throw new ArgumentException("Invalid License import file!");
        }

        protected override void ThrowOnInvalidColumns(List<string> missingColumns)
        {
            if (missingColumns.Any())
                throw new ArgumentException($"Invalid QOAM import file: Column(s) \"{missingColumns.Aggregate((a, b) => $"{a}, {b}")}\" not found.");
        }
    }
}