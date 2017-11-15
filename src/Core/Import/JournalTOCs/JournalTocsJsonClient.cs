using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NLog;

namespace QOAM.Core.Import.JournalTOCs
{
    public class JournalTocsJsonClient : IJournalTocsClient
    {
        readonly JournalTocsSettings _settings;
        readonly IWebClientFactory _webClientFactory;
        static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        static readonly Encoding _encoding = new UTF8Encoding(false);


        public JournalTocsJsonClient(JournalTocsSettings settings, IWebClientFactory webClientFactory)
        {
            _settings = settings;
            _webClientFactory = webClientFactory;
        }

        public List<string> DownloadJournals(JournalTocsFetchMode action = JournalTocsFetchMode.Update)
        {
            return new List<string>();
        }

        public List<string> DownloadJournals(List<string> issns)
        {
            _logger.Info("Downloading journals...");

            using (var webClient = _webClientFactory.Create())
            {
                webClient.Encoding = _encoding;

                //_logger.Info($"\t...downloading batch #{_resumptionToken + 1}...");
                
                //var resultString = webClient.DownloadString("http://www.journaltocs.ac.uk/API/RSS/GetJournalByIssn.php?sui=z7CsvQxb1udh849067j6&test=true&issns[]=1697-5200&issns[]=1502-4873");

                var issnParams = issns.Aggregate(new StringBuilder(), (sb, a) => sb.Append($"&issns[]={a}"), sb => sb.ToString());

                var resultString = webClient.DownloadString($"{_settings.ByIssnRequestUrl}{issnParams}");

                resultString = HttpUtility.HtmlDecode(resultString);

                return new List<string> { resultString };
            }
        }
    }
}