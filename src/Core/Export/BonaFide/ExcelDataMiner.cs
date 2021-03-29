using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace QOAM.Core.Export.BonaFide
{
    public class ExcelDataMiner : DataMinerBase
    {
        protected override string ContentType => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        protected override string FileNameExtension => "xlsx";
        
        protected override byte[] Serialize(DataMiningRecord record)
        {
            using(var memoryStream = new MemoryStream()) 
            {
                IWorkbook workbook = new XSSFWorkbook();
                var excelSheet = workbook.CreateSheet("Sheet1");

                var headerRow = excelSheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("ISSNs");

                for (var i = 0; i < record.ISSNs.Count; i++)
                {
                    var row = excelSheet.CreateRow(i + 1);
                    row.CreateCell(0).SetCellValue(record.ISSNs[i]);
                }

                workbook.Write(memoryStream);

                return memoryStream.ToArray();
            }
        }
    }
}