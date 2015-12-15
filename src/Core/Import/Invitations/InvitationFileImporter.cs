using System;
using System.Collections.Generic;
using System.Linq;
using NPOI.SS.UserModel;

namespace QOAM.Core.Import.Invitations
{
    public class InvitationFileImporter : FileImporterBase
    {
        protected override string MainSheet => "Authors";

        protected override void ProcessSheets(IWorkbook workbook, int mainSheetIndex)
        {
            ExtractSheet(MainSheet, workbook, "eissn", "Author email address", "Author name");
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
                throw new ArgumentException($"Invalid Authors import file: Column(s) \"{missingColumns.Aggregate((a, b) => $"{a}, {b}")}\" not found.");
        }

        protected override void ValidateFile(int mainSheetIndex)
        { }
    }
}