using System.Data;
using QOAM.Core.Import.Institutions;
using QOAM.Core.Tests.Import.Corners.Stubs;
using Xunit;

namespace QOAM.Core.Tests.Import.Corners
{
    public class CornerEntityConverterTests
    {
        CornerEntityConverter _converter;
        DataSet _validDataSet;

        public void Initialize()
        {
            _validDataSet = CornerStubs.CompleteDataSet();
            _converter = new CornerEntityConverter();
        }

        [Fact]
        public void CornersTableIsParsedIntoAUsableEntity()
        {
            Initialize();
            var result = _converter.Execute(_validDataSet);

            Assert.Equal(2, result.Count);

            Assert.Equal("Test Corner", result[0].Name);
            Assert.Equal(2, result[0].ISSNs.Count);

            Assert.Equal("Another Corner", result[1].Name);
            Assert.Equal(1, result[1].ISSNs.Count);
        }
    }
}