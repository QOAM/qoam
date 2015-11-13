//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using Moq;
//using NPOI.SS.UserModel;
//using QOAM.Core.Import.Licences;
//using QOAM.Core.Tests.Import.Licenses.Stubs;
//using Xunit;

//namespace QOAM.Core.Tests.Import.Licenses
//{
//    public class BulkImporterTest
//    {
//        #region Fields

//        BulkImporter _bulkImporter;

//        Mock<ILicenseFileImporter> _fileImporter;
//        Mock<IImportEntityConverter> _entityConverter;

//        #endregion

//        public void Initialize()
//        {
//            _fileImporter = new Mock<ILicenseFileImporter>();
//            _entityConverter = new Mock<IImportEntityConverter>();
//            _bulkImporter = new BulkImporter(_fileImporter.Object, _entityConverter.Object);
//        }

//        [Fact]
//        public void UniversityLicensesAreConvertedToInstitutionJournals()
//        {
//            Initialize();

//            var dataSet = ImportFileStubs.CompleteDataSet();
//            var licenses = ConvertedEntitiyStubs.Licenses();

//            _fileImporter.Setup(x => x.Execute(It.IsAny<IWorkbook>())).Returns(dataSet);
//            _entityConverter.Setup(x => x.Execute(dataSet)).Returns(licenses);

//            var result = _bulkImporter.Execute(new FileStream("Import\\Licenses\\Stubs\\QOAMupload.xlsx", FileMode.Open, FileAccess.Read));

//            Assert.IsType<List<InstitutionJournal>>(result);

//            var firstJournal = result.First();
//            var firstLicense = licenses.First();
//        }
//    }
//}