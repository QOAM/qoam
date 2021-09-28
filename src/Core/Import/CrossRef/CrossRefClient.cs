using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using NLog;

namespace QOAM.Core.Import.CrossRef
{
    public class CrossRefClient : ICrossRefClient
    {
        readonly CrossRefSettings _settings;
        readonly IWebClientFactory _webClientFactory;
        static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        static readonly Encoding _encoding = new UTF8Encoding(false);
        readonly JsonSerializerSettings _deserializerSettings;

        public CrossRefClient(CrossRefSettings settings, IWebClientFactory webClientFactory)
        {
            _settings = settings;
            _webClientFactory = webClientFactory;
            _deserializerSettings = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore };
        }

        public List<Item> DownloadJournals()
        {
            _logger.Info("Downloading journals from CrossRef...");

            var result = new List<Item>();
            var batchCount = 1;

            using (var webClient = _webClientFactory.Create())
            {
                webClient.Encoding = _encoding;

                var fetch = true;

                do
                {
                    // User-Agent Header is removed after each request :/
                    webClient.Headers.Add(HttpRequestHeader.UserAgent, _settings.UserAgentHeader);

                    _logger.Info($"\t...downloading batch #{batchCount}...");

                    var batch = webClient.DownloadString(_settings.AllJournalsRequestUrl);
                    var parsedBatch = JsonConvert.DeserializeObject<CrossRefListResult>(batch, _deserializerSettings );

                    if (parsedBatch != null)
                    {
                        result.AddRange(parsedBatch.Message.Items);

                        if (result.Count == parsedBatch.Message.TotalResults)
                            fetch = false;
                        else
                        {
                            _settings.Cursor = HttpUtility.UrlEncode(parsedBatch.Message.NextCursor);
                            batchCount++;
                        }
                    }
                    else
                    {
                        _logger.Info("Found an empty batch; cannot continue.");
                        fetch = false;
                    }
                } while (fetch);

                _logger.Info("Finished downloading journals from CrossRef.");

                return result;
            }
        }

        public List<Item> DownloadJournal(string issn)
        {
            _logger.Info($"Downloading journal with issn {issn}...");

            using (var webClient = _webClientFactory.Create())
            {
                webClient.Encoding = _encoding;
                webClient.Headers.Add("User-Agent", _settings.UserAgentHeader);

                var resultString = webClient.DownloadString($"{_settings.ByIssnRequestUrl}/{issn}");
                var parsedResult = JsonConvert.DeserializeObject<CrossRefJournalResult>(resultString, _deserializerSettings);
                
                //resultString = HttpUtility.HtmlDecode(resultString);

                return new List<Item> { parsedResult?.Message };
            }
        }
    }
}