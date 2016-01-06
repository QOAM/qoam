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

            Assert.Equal(7, result.Count);

            Assert.Equal("1687-8140", result.First().ISSN);
            Assert.Equal("annette.caenen@ugent.be", result.First().AuthorEmail);
            Assert.Equal("A. Caenen", result.First().AuthorName);
        }

        [Fact]
        public void CommasAndSemicolonsAreSplitAndParsed()
        {
            Initialize();
            var result = _converter.Execute(_validDataSet);

            Assert.Equal(7, result.Count);

            var splitResults = result.Where(r => r.ISSN == "1234-7675").ToList();

            Assert.Equal(3, splitResults.Count);
            Assert.True(splitResults.All(r => r.AuthorName == "K. Test"));
            Assert.Equal("kees.test@ugent.be", splitResults.First().AuthorEmail);
            Assert.Equal("kees.t@outlook.com", splitResults.Last().AuthorEmail);
        }
    }
}