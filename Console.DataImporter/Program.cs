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
        private static IContainer container;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            try
            {
                container = DependencyInjectionConfig.RegisterComponents();

                ImportJournals(GetImportType(args), GetImportMode(args), GetJournalUpdateProperties(args));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private static void ImportJournals(JournalsImportSource importSource, JournalsImportMode importMode, ISet<JournalUpdateProperty> journalUpdateProperties)
        {
            Logger.Info("Import source: {0}", importSource);
            Logger.Info("Import mode: {0}", importMode);
            Logger.Info("Importing journals...");

            var importResult = container.Resolve<JournalsImport>().ImportJournals(GetJournalsToImport(importSource), importMode, journalUpdateProperties);

            Logger.Info("Imported {0} journals total", importResult.NumberOfImportedJournals);
            Logger.Info("mported {0} new journals", importResult.NumberOfNewJournals);
        }

        private static IList<Journal> GetJournalsToImport(JournalsImportSource importSource)
        {
            switch (importSource)
            {
                case JournalsImportSource.DOAJ:
                    return container.Resolve<DoajImport>().GetJournals();    
                case JournalsImportSource.Ulrichs:
                    return container.Resolve<UlrichsImport>().GetJournals(UlrichsImport.UlrichsJournalType.OpenAccess);    
                default:
                    throw new ArgumentOutOfRangeException("importSource");
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

        public static JournalsImportMode GetImportMode(IList<string> args)
        {
            if (args.Count < 2)
            {
                return JournalsImportMode.InsertOnly;
            }

            return (JournalsImportMode)Enum.Parse(typeof(JournalsImportMode), args[1], true);
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
