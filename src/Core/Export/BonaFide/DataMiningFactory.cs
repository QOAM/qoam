using System;

namespace QOAM.Core.Export.BonaFide
{
    public class DataMiningFactory
    {
        public static IDataMiner Create(DataMiningFormats fileFormat)
        {
            switch (fileFormat)
            {
                case DataMiningFormats.Xml:
                    return new XmlDataMiner();
                case DataMiningFormats.Json:
                    return new JsonDataMiner();
                case DataMiningFormats.Excel:
                    return new ExcelDataMiner();
                case DataMiningFormats.Csv:
                    return new CsvDataMiner();
                default:
                    throw new ArgumentException("Unknown file format");
            }
        }
    }
}