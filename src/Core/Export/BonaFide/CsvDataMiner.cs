using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace QOAM.Core.Export.BonaFide
{
    public class CsvDataMiner : DataMinerBase
    {
        protected override string ContentType => "text/csv";
        protected override string FileNameExtension => "csv";
        
        protected override byte[] Serialize(DataMiningRecord record)
        {
            var configuration = new CsvConfiguration(CultureInfo.CurrentCulture) { HasHeaderRecord = true, Delimiter = ";", TrimOptions = TrimOptions.Trim };

            using (var memoryStream = new MemoryStream())
                using (var streamWriter = new StreamWriter(memoryStream))
                    using (var csvWriter = new CsvWriter(streamWriter, configuration))
                    {
                        csvWriter.WriteField("ISSN");
                        csvWriter.NextRecord();

                        foreach (var issn in record.ISSNs)
                        {
                            csvWriter.WriteField(issn);
                            csvWriter.NextRecord();
                        }

                        csvWriter.Flush();
                        streamWriter.Flush();
                        memoryStream.Position = 0;

                        return memoryStream.ToArray();
                    }
        }
    }
}