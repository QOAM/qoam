using System.Collections.Generic;
using System.Text;
using NLog;

namespace QOAM.Core.Import.JournalTOCs
{
    public class JournalTocsClient : IJournalTocsClient
    {
        readonly JournalTocsSettings _settings;
        readonly IWebClientFactory _webClientFactory;
        static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        static readonly Encoding _encoding = new UTF8Encoding(false);

        const string EndOfBatchesNotice = "<noticeCode>11</noticeCode>";
        const string Notice = "<noticeCode>";

        int _resumptionToken;

        public JournalTocsClient(JournalTocsSettings settings, IWebClientFactory webClientFactory)
        {
            _settings = settings;
            _webClientFactory = webClientFactory;
        }

        public List<string> DownloadJournals(JournalTocsFetchMode action = JournalTocsFetchMode.Update)
        {
            _logger.Info("Downloading journals...");

            var result = new List<string>();

            using (var webClient = _webClientFactory.Create())
            {
                webClient.Encoding = _encoding;
                var fetch = true;

                do
                {
                    _logger.Info($"\t...downloading batch #{_resumptionToken + 1}...");

                    var batch = webClient.DownloadString($"{_settings.AllJournalsRequestUrl}&action={action.ToString().ToLowerInvariant()}&resumptionToken={_resumptionToken}");

                    if (!batch.Contains(EndOfBatchesNotice) || !batch.Contains(Notice))
                    {
                        result.Add(batch.Replace("&", "&amp;"));
                        _resumptionToken++;
                    }
                    else
                        fetch = false;

                } while (fetch);

                _logger.Info("Finised downloading journals.");

                return result;
            }
        }

        public List<string> DownloadJournals(List<string> issns)
        {
            return new List<string>();
        }
    }
}