using System.IO;
using System.Xml.Serialization;

namespace QOAM.Core.Export.BonaFide
{
    public class XmlDataMiner : DataMinerBase
    {
        protected override string ContentType => "application/xml";
        protected override string FileNameExtension => "xml";

        protected override byte[] Serialize(DataMiningRecord record)
        {
            var serializer = new XmlSerializer(typeof(DataMiningRecord));

            using (var memoryStream = new MemoryStream())
            {
                serializer.Serialize(memoryStream, record);
                memoryStream.Position = 0;

                return memoryStream.ToArray();
            }
        }
    }
}