﻿using System.Linq;
using Moq;
using QOAM.Core.Import;
using QOAM.Core.Import.JournalTOCs;
using QOAM.Core.Tests.Import.Resources;
using Xunit;

namespace QOAM.Core.Tests.Import
{
    public class JournalTocsClientTests
    {
        Mock<IWebClientFactory> _factory;
        Mock<IWebClient> _webClient;
        JournalTocsSettings _settings;

        [Theory]
        [InlineData(JournalTocsFetchMode.Setup)]
        [InlineData(JournalTocsFetchMode.Update)]
        public void DownloadJournals_uses_the_given_Action_to_fetch_journals(JournalTocsFetchMode action)
        {
            var sut = CreateClient();

            _webClient.Setup(x => x.DownloadString(It.IsAny<string>())).Returns(GetJournalTocsNoMoreItemsNotice());

            sut.DownloadJournals(action);

            _webClient.Verify(x => x.DownloadString($"{_settings.AllJournalsRequestUrl}&action={action.ToString().ToLowerInvariant()}&resumptionToken=0"), Times.Once);
        }

        [Fact]
        public void DownloadJournals_fetches_a_batch_of_journals()
        {
            var sut = CreateClient();

            var journalsXml = GetJournalTocsFirst500Xml();

            _webClient.Setup(x => x.DownloadString($"{_settings.AllJournalsRequestUrl}&action=update&resumptionToken=0")).Returns(journalsXml);
            _webClient.Setup(x => x.DownloadString($"{_settings.AllJournalsRequestUrl}&action=update&resumptionToken=1")).Returns(GetJournalTocsNoMoreItemsNotice());

            var result = sut.DownloadJournals();

            Assert.Equal(journalsXml.Replace("&", "&amp;"), result.First());
        }

        [Fact]
        public void DownloadJournals_fetches_journals_until_there_are_no_more_batches()
        {
            var sut = CreateClient();

            var firstBatch = GetJournalTocsFirst500Xml();
            var secondBatch = GetJournalTocsNext500Xml();

            _webClient.Setup(x => x.DownloadString($"{_settings.AllJournalsRequestUrl}&action=update&resumptionToken=0")).Returns(firstBatch);
            _webClient.Setup(x => x.DownloadString($"{_settings.AllJournalsRequestUrl}&action=update&resumptionToken=1")).Returns(secondBatch);
            _webClient.Setup(x => x.DownloadString($"{_settings.AllJournalsRequestUrl}&action=update&resumptionToken=2")).Returns(GetJournalTocsNoMoreItemsNotice());

            var result = sut.DownloadJournals();

            _webClient.Verify(x => x.DownloadString($"{_settings.AllJournalsRequestUrl}&action=update&resumptionToken=0"), Times.Once());
            _webClient.Verify(x => x.DownloadString(It.IsAny<string>()), Times.Exactly(3));

            Assert.Equal(firstBatch.Replace("&", "&amp;"), result[0]);
            Assert.Equal(secondBatch.Replace("&", "&amp;"), result[1]);
        }

        #region Private Methods

        JournalTocsClient CreateClient()
        {
            _factory = new Mock<IWebClientFactory>();
            _webClient = new Mock<IWebClient>();

            _factory.Setup(x => x.Create()).Returns(_webClient.Object);

            _settings = new JournalTocsSettings();
            return new JournalTocsClient(_settings, _factory.Object);
        }

        static string GetJournalTocsFirst500Xml()
        {
            return ResourceReader.GetContentsOfResource("JournalTocs-setup-first-500.xml");
        }

        static string GetJournalTocsNext500Xml()
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