using System.IO;
using Newtonsoft.Json;

namespace QOAM.Core.Export.BonaFide
{
    public class JsonDataMiner : DataMinerBase
    {
        protected override string ContentType => "application/json";
        protected override string FileNameExtension => "json";

        protected override byte[] Serialize(DataMiningRecord records)
        {
            var serializer = new JsonSerializer();

            using (var memoryStream = new MemoryStream())
                using (var sw = new StreamWriter(memoryStream))
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, records);
                        writer.Flush();
                        memoryStream.Position = 0;

                        return memoryStream.ToArray();
                    }
        }
    }
}