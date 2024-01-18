using System.Collections.Generic;
using System.Xml.Serialization;
using CsvHelper.Configuration.Attributes;

namespace QOAM.Core.Export.BonaFide
{
    [XmlRoot("Journals")]
    public class DataMiningRecord
    {
        [XmlArrayItem("ISSN"), Name("ISSNs")]
        public List<string> ISSNs { get; set; }
    }
}