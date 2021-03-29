namespace QOAM.Core.Import
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using CsvHelper;
    using CsvHelper.Configuration;

    using NLog;
    using Helpers;
    using Repositories;
    using Validation;

    public class DoajImport : Import
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly Encoding Encoding = new UTF8Encoding(false);

        private readonly DoajSettings doajSettings;

        private static readonly CsvConfiguration CsvConfiguration = new CsvConfiguration(new CultureInfo("en-US"))
                                                                        {
                                                                            HasHeaderRecord = true,
                                                                            Delimiter = ",",
                                                                            TrimOptions = TrimOptions.Trim,
                                                                            Encoding = Encoding
        };

        public DoajImport(DoajSettings doajSettings, IBlockedISSNRepository blockedIssnRepository) : base(blockedIssnRepository)
        {
            Requires.NotNull(doajSettings, nameof(doajSettings));
            
            this.doajSettings = doajSettings;
        }

        public IList<Journal> GetJournals()
        {
            return this.ExcludeBlockedIssns(this.ParseJournals(this.DownloadJournals()));
        }

        private string DownloadJournals()
        {
            Logger.Info("Downloading journals...");

            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding;

                return webClient.DownloadString(this.doajSettings.CsvUrl);
            }
        }

        public IList<Journal> ParseJournals(string csv)
        {
            Logger.Info("Parsing journals...");
            
            using (var streamReader = new StringReader(csv))
                using (var csvParser = new CsvParser(streamReader, CsvConfiguration))
                    using (var csvReader = new CsvReader(csvParser))
                    {
                        csvReader.Context.RegisterClassMap<DoajImportRecordMap>();
                        var importRecords = csvReader.GetRecords<DoajImportRecord>().ToList();

                        return importRecords.Select(i => i.ToJournal()).Where(j => j.IsValid()).ToList();
                    }
        }
    }
}