using System.Data;
using System.Linq;
using QOAM.Core.Import.SubmissionLinks;
using QOAM.Core.Tests.Import.SubmissionLinks.Stubs;
using Xunit;

namespace QOAM.Core.Tests.Import.SubmissionLinks
{
    public class SubmissionPageLinkEntityConverterTests
    {
        SubmissionPageLinkEntityConverter _converter;
        DataSet _validDataSet;

        public void Initialize()
        {
            _validDataSet = SubmissionLinkImportStubs.CompleteDataSet();
            _converter = new SubmissionPageLinkEntityConverter();
        }

        [Fact]
        public void LinksTableIsParsedIntoAUsableEntity()
        {
            Initialize();
            var result = _converter.Execute(_validDataSet);

            Assert.Equal(4, result.Count);

            Assert.Equal("1687-8140", result.First().ISSN);
            Assert.Equal("http://ade.sagepub.com/submission", result.First().Url);
        }
    }
}