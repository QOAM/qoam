using System.Collections.Generic;
using System.Linq;
using Moq;
using QOAM.Core.Import;
using QOAM.Core.Repositories;
using QOAM.Core.Tests.Import.Resources;
using Xunit;

namespace QOAM.Core.Tests.Import
{
    public class JournalTocsClientTests
    {
        Mock<IWebClientFactory> _factory;
        Mock<IWebClient> _webClient;
        
        [Fact]
        public void DownloadJournals_fetches_a_batch_of_journals()
        {
            var sut = CreateClient();

            _webClient.Setup(x => x.DownloadString(It.IsAny<string>())).Returns(GetJournalTocsFirst500Xml());
        }

        #region Private Methods

        JournalTocsClient CreateClient()
        {
            _factory = new Mock<IWebClientFactory>();
            _webClient = new Mock<IWebClient>();

            _factory.Setup(x => x.Create()).Returns(_webClient.Object);

            return new JournalTocsClient(new JournalTocsSettings(), _factory.Object);
        }

        static string GetJournalTocsFirst500Xml()
        {
            return ResourceReader.GetContentsOfResource("JournalTocs-setup-first-500.xml");
        }

        static string GetJournalTocsNextXml()
        {
            return ResourceReader.GetContentsOfResource("JournalTocs-setup-next-500.xml");
        }

        static string GetJournalTocsNoMoreItemsNotice()
        {
            return ResourceReader.GetContentsOfResource("JournalTocs-no-more-items-notice.xml");
        }

        #endregion
    }
}