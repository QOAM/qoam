using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NPOI.OpenXmlFormats.Dml;
using QOAM.Core.Tests.Import.Licenses.Stubs;
using Xunit;

namespace QOAM.Core.Tests.Import.Licenses
{
    public class ImportEntityConverterTests
    {
        ImportEntityConverter _converter;
        DataSet _validDataSet;

        public void Initialize()
        {
            _validDataSet = ImportFileStubs.CompleteDataSet();
            _converter = new ImportEntityConverter(_validDataSet);
        }

        [Fact]
        public void UniversitiesTableIsParsedIntoAUsableEntity()
        {
            Initialize();
            var result = _converter.Convert();

            Assert.Equal(4, result.Count);
            Assert.Equal(2, result[2].Licenses.Count);
        }
    }

    public class ImportEntityConverter
    {
        readonly DataSet _data;

        public ImportEntityConverter(DataSet data)
        {
            _data = data;
        }

        public List<UniversityLicense> Convert()
        {
            var licenses = (from row in _data.Tables["Universities"].Rows.Cast<DataRow>()
                            select new UniversityLicense
                            {
                                Domain = row["Domein"].ToString(),
                                Licenses = row["Tabbladen"].ToString().Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList()
                            }).ToList();

            return licenses;
        }
    }


    public class UniversityLicense
    {
        public string Domain { get; set; }
        public List<string> Licenses { get; set; }
    }
}