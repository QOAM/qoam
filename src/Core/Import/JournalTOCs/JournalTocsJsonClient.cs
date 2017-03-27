using System.Collections.Generic;
using System.Text;
using System.Web.Helpers;
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
            _logger.Info("Downloading journals...");

            var result = new List<string>();

            using (var webClient = _webClientFactory.Create())
            {
                webClient.Encoding = _encoding;

                //_logger.Info($"\t...downloading batch #{_resumptionToken + 1}...");

                //var batch = webClient.DownloadString($"{_settings.RequestUrl}&action={action.ToString().ToLowerInvariant()}&resumptionToken={_resumptionToken}");

                var resultString = webClient.DownloadString("http://www.journaltocs.ac.uk/API/RSS/GetJournalByIssn.php?sui=z7CsvQxb1udh849067j6&test=true&issns[]=1697-5200&issns[]=1502-4873");

                var json = Json.Decode(resultString.Replace("prism:", "").Replace("dc:", ""));
                    
                _logger.Info("Finised downloading journals.");

                return result;
            }
        }
    }
}