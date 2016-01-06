namespace QOAM.Core.Tests.Import.Licenses
{
    using System.Data;
    using System.Linq;
    using Core.Import.Licences;
    using Stubs;
    using Xunit;

    public class ImportLicenseEntityConverterTests
    {
        ImportLicenseEntityConverter _converter;
        DataSet _validDataSet;

        public void Initialize()
        {
            _validDataSet = ImportFileStubs.CompleteDataSet();
            _converter = new ImportLicenseEntityConverter();
        }

        [Fact]
        public void UniversitiesTableIsParsedIntoAUsableEntity()
        {
            Initialize();
            var result = _converter.Execute(_validDataSet);

            Assert.Equal(6, result.Count);
            Assert.Equal(8, result[2].Licenses.Count);
        }

        [Fact]
        public void LicensesAreParsedIntoInstitutionJournalEntities()
        {
            Initialize();

            var result = _converter.Execute(_validDataSet);

            var firstLicense = result.First().Licenses.First();

            Assert.IsType<LicenseInfo>(firstLicense);
            Assert.Equal("Sage", firstLicense.LicenseName);
        }
    }
}