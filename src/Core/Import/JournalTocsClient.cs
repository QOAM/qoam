using System.Text;
using NLog;

namespace QOAM.Core.Import
{
    public interface IJournalTocsClient
    {
        int ResumptionToken { get; set; }
        string DownloadJournals();
    }

    public class JournalTocsClient : IJournalTocsClient
    {
        readonly JournalTocsSettings _settings;
        readonly IWebClientFactory _webClientFactory;
        static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        static readonly Encoding _encoding = new UTF8Encoding(false);

        public int ResumptionToken { get; set; }

        public JournalTocsClient(JournalTocsSettings settings, IWebClientFactory webClientFactory)
        {
            _settings = settings;
            _webClientFactory = webClientFactory;
        }

        public string DownloadJournals()
        {
            _logger.Info("Downloading journals...");

            using (var webClient = _webClientFactory.Create())
            {
                webClient.Encoding = _encoding;

                return webClient.DownloadString(_settings.Url);
            }
        }
    }
}