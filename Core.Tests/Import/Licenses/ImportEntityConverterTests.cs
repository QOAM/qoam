using System.Data;
using System.Linq;
using QOAM.Core.Import.Licences;
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
            Assert.Equal(8, result[2].Licenses.Count);
        }

        [Fact]
        public void LicensesAreParsedIntoInstitutionJournalEntities()
        {
            Initialize();

            var result = _converter.Convert();

            var firstLicense = result.First().Licenses.First();

            Assert.IsType<LicenseInfo>(firstLicense);
            Assert.Equal("Sage", firstLicense.LicenseName);
        }
    }
}