using System.Collections.Generic;
using NLog;
using QOAM.Core.Repositories;

namespace QOAM.Core.Import.JournalTOCs
{
    public class JournalTocsImport : Import
    {
        readonly IJournalTocsClient _client;
        readonly IJournalTocsParser _parser;
        static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public JournalTocsImport(IJournalTocsClient client, IBlockedISSNRepository blockedIssnRepository, IJournalTocsParser parser) : base(blockedIssnRepository)
        {
            _client = client;
            _parser = parser;
        }

        public IList<Journal> DownloadJournals(JournalTocsFetchMode action = JournalTocsFetchMode.Update)
        {
            return ExcludeBlockedIssns(ParseJournals(action));
        }

        public IList<Journal> DownloadJournals(List<string> issns)
        {
            return ExcludeBlockedIssns(ParseJournals(issns));
        }

        #region Private Methods

        IList<Journal> ParseJournals(JournalTocsFetchMode action = JournalTocsFetchMode.Update)
        {
            var xml = _client.DownloadJournals(action);

            return _parser.Parse(xml);
        }

        IList<Journal> ParseJournals(List<string> issns)
        {
            var json = _client.DownloadJournals(issns);

            return _parser.Parse(json);
        }

        #endregion
    }
}