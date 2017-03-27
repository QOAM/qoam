using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NLog;
using QOAM.Core.Helpers;
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

        #region Private Methods

        IList<Journal> ParseJournals(JournalTocsFetchMode action = JournalTocsFetchMode.Update)
        {
            var xml = GetXml(action);

            return _parser.Parse(xml);
        }

        IEnumerable<string> GetXml(JournalTocsFetchMode action = JournalTocsFetchMode.Update)
        {
            return _client.DownloadJournals(action);
        }


        #endregion
    }
}