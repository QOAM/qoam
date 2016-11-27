namespace QOAM.Console.DataImporter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autofac;

    using NLog;

    using QOAM.Core;
    using QOAM.Core.Import;

    public static class Program
    {
        private static IContainer Container { get; } = DependencyInjectionConfig.RegisterComponents();

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            try
            {
                var source = GetImportType(args);
                var fetchMode = GetFetchMode(source, args);

                ImportJournals(source, GetImportMode(source, args, fetchMode), fetchMode, GetJournalUpdateProperties(args));
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

        public static JournalsImportMode GetImportMode(JournalsImportSource source, IList<string> args, JournalTocsFetchMode? fetchMode = null)
        {
            if (source == JournalsImportSource.JournalTOCs && fetchMode != null)
                return fetchMode == JournalTocsFetchMode.Setup ? JournalsImportMode.InsertAndUpdate : JournalsImportMode.UpdateOnly;

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

        public static ISet<JournalUpdateProperty> GetJournalUpdateProperties(IList<string> args)
        {
            if (args.Count < 3)
            {
                var journalUpdateProperties = new HashSet<JournalUpdateProperty>((JournalUpdateProperty[]) Enum.GetValues(typeof (JournalUpdateProperty)));
                journalUpdateProperties.Remove(JournalUpdateProperty.DoajSeal);

                return journalUpdateProperties;
            }

            return new HashSet<JournalUpdateProperty>(args[2].Split(',', ';').Select(s => (JournalUpdateProperty)Enum.Parse(typeof(JournalUpdateProperty), s, true)));
        }
    }
}
