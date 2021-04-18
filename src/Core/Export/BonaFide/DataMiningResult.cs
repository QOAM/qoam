using System.IO;

namespace QOAM.Core.Export.BonaFide
{
    public class DataMiningResult
    {
        public byte[] Bytes { get; set; }
        public string ContentType { get; set; }
        public string FileNameExtension { get; set; }
    }
}