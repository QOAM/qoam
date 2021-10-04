using System.Net;
using QOAM.Core.Import.CrossRef;
using QOAM.Core.Import.JournalTOCs;

namespace QOAM.Console.DataImporter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autofac;

    using NLog;

    using Core;
    using Core.Import;

    public static class Program
    {
        static IContainer Container { get; } = DependencyInjectionConfig.RegisterComponents();

        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                var source = GetImportType(args);

                ImportJournals(source, GetImportMode(source, args), GetFetchMode(source, args), GetJournalUpdateProperties(args, source));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private static void ImportJournals(JournalsImportSource importSource, JournalsImportMode importMode, JournalTocsFetchMode? action, ISet<JournalUpdateProperty> journalUpdateProperties)
        {
            Logger.Info("Import source: {0}", importSource);
            Logger.Info("Import mode: {0}", importMode);
            Logger.Info("Importing journals...");

            var journalsImport = Container.Resolve<JournalsImport>();
            var importResult = journalsImport.ImportJournals(GetJournalsToImport(importSource, action), importMode, journalUpdateProperties);

            Logger.Info("Imported {0} journals total", importResult.NumberOfImportedJournals);
            Logger.Info("Imported {0} new journals", importResult.NumberOfNewJournals);
        }

        private static IList<Journal> GetJournalsToImport(JournalsImportSource importSource, JournalTocsFetchMode? action = JournalTocsFetchMode.Update)
        {
            switch (importSource)
            {
                case JournalsImportSource.DOAJ:
                    return Container.Resolve<DoajImport>().GetJournals();    
                case JournalsImportSource.Ulrichs:
                    return Container.Resolve<UlrichsImport>().GetJournals(UlrichsImport.UlrichsJournalType.OpenAccess);
                case JournalsImportSource.JournalTOCs:
                    return Container.Resolve<JournalTocsImport>().DownloadJournals(action.GetValueOrDefault(JournalTocsFetchMode.Update));
                case JournalsImportSource.CrossRef:
                    return Container.Resolve<CrossRefImport>().DownloadJournals();
                default:
                    throw new ArgumentOutOfRangeException(nameof(importSource));
            }
        }

        public static JournalsImportSource GetImportType(IList<string> args)
        {
            if (args.Count < 1)
            {
                return JournalsImportSource.Ulrichs;
            }

            return (JournalsImportSource)Enum.Parse(typeof(JournalsImportSource), args[0], true);
        }

        public static JournalsImportMode GetImportMode(JournalsImportSource source, IList<string> args)
        {
            switch (source)
            {
                case JournalsImportSource.JournalTOCs:
                    return JournalsImportMode.InsertAndUpdate;
                case JournalsImportSource.CrossRef:
                    return JournalsImportMode.UpdateOnly;
            }

            if (args.Count < 2)
            {
                return JournalsImportMode.InsertOnly;
            }

            return (JournalsImportMode)Enum.Parse(typeof(JournalsImportMode), args[1], true);
        }

        public static JournalTocsFetchMode? GetFetchMode(JournalsImportSource source, IList<string> args)
        {
            if(source != JournalsImportSource.JournalTOCs || args.Count < 2)
                return null;

            return (JournalTocsFetchMode) Enum.Parse(typeof(JournalTocsFetchMode), args[1], true);
        }

        public static ISet<JournalUpdateProperty> GetJournalUpdateProperties(IList<string> args, JournalsImportSource journalsImportSource)
        {
            if (journalsImportSource == JournalsImportSource.CrossRef)
                return new HashSet<JournalUpdateProperty> { JournalUpdateProperty.NumberOfArticles };

            HashSet<JournalUpdateProperty> journalUpdateProperties;
            if (args.Count < 3)
            {
                journalUpdateProperties = new HashSet<JournalUpdateProperty>((JournalUpdateProperty[]) Enum.GetValues(typeof (JournalUpdateProperty)));
                journalUpdateProperties.Remove(JournalUpdateProperty.DoajSeal);
            }
            else 
                journalUpdateProperties = new HashSet<JournalUpdateProperty>(args[2].Split(',', ';').Select(s => (JournalUpdateProperty)Enum.Parse(typeof(JournalUpdateProperty), s, true)));

            if (journalsImportSource == JournalsImportSource.DOAJ)
                journalUpdateProperties.Remove(JournalUpdateProperty.DataSource);

            return journalUpdateProperties;
        }
    }
}
