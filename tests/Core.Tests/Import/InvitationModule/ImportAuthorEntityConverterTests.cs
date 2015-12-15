using System.Data;
using System.Linq;
using QOAM.Core.Import.Invitations;
using QOAM.Core.Tests.Import.InvitationModule.Stubs;
using Xunit;

namespace QOAM.Core.Tests.Import.InvitationModule
{
    public class ImportAuthorEntityConverterTests
    {
        ImportAuthorEntityConverter _converter;
        DataSet _validDataSet;

        public void Initialize()
        {
            _validDataSet = InvitationStubs.CompleteDataSet();
            _converter = new ImportAuthorEntityConverter();
        }

        [Fact]
        public void AuthorsTableIsParsedIntoAUsableEntity()
        {
            Initialize();
            var result = _converter.Execute(_validDataSet);

            Assert.Equal(4, result.Count);

            Assert.Equal("1687-8140", result.First().ISSN);
            Assert.Equal("annette.caenen@ugent.be", result.First().AuthorEmail);
            Assert.Equal("A. Caenen", result.First().AuthorName);
        }
    }
}