using System.Text;
using NLog;

namespace QOAM.Core.Import
{
    public interface IJournalTocsClient
    {
        //int ResumptionToken { get; set; }
        string DownloadJournals(string action = "update");
    }

    public class JournalTocsClient : IJournalTocsClient
    {
        readonly JournalTocsSettings _settings;
        readonly IWebClientFactory _webClientFactory;
        static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        static readonly Encoding _encoding = new UTF8Encoding(false);

        const string EndOfBatchesNotice = "<noticeCode>11</noticeCode>";

        int _resumptionToken;

        public JournalTocsClient(JournalTocsSettings settings, IWebClientFactory webClientFactory)
        {
            _settings = settings;
            _webClientFactory = webClientFactory;
        }

        public string DownloadJournals(string action = "update")
        {
            _logger.Info("Downloading journals...");

            var result = new StringBuilder();
            using (var webClient = _webClientFactory.Create())
            {
                webClient.Encoding = _encoding;
                var fetch = true;

                do
                {
                    _logger.Info($"\t...downloading batch #{_resumptionToken + 1}...");

                    var batch = webClient.DownloadString($"{_settings.RequestUrl}&action={action}&resumptionToken={_resumptionToken}");

                    if (!batch.Contains(EndOfBatchesNotice))
                    {
                        result.Append(batch);
                        _resumptionToken++;
                    }
                    else
                        fetch = false;

                } while (fetch);

                _logger.Info("Finised downloading journals.");

                return result.ToString();
            }
        }
    }
}