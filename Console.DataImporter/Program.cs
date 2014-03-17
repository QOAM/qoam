namespace QOAM.Console.DataImporter
{
    using System;
    using System.Collections.Generic;

    using Autofac;

    using QOAM.Core;
    using QOAM.Core.Import;

    internal static class Program
    {
        private static IContainer container;

        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine(Strings.InvalidArguments);
                return;
            }

            try
            {
                container = DependencyInjectionConfig.RegisterComponents();

                ImportJournals(GetImportType(args), GetImportMode(args));
            }
            catch (Exception ex)
            {
                Console.WriteLine(Strings.Error, ex);
            }
        }

        private static void ImportJournals(JournalsImportSource importSource, JournalsImportMode importMode)
        {
            Console.WriteLine(Strings.ExecutingType, importSource);
            Console.WriteLine(Strings.ExecutingMode, importMode);
            Console.WriteLine(Strings.ImportingJournals);

            var importResult = container.Resolve<JournalsImport>().ImportJournals(GetJournalsToImport(importSource), importMode);

            Console.WriteLine(Strings.ImportedJournals, importResult.NumberOfImportedJournals, importResult.NumberOfNewJournals);
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

        private static JournalsImportSource GetImportType(IList<string> args)
        {
            return (JournalsImportSource)Enum.Parse(typeof(JournalsImportSource), args[0], true);
        }

        private static JournalsImportMode GetImportMode(IList<string> args)
        {
            if (args.Count < 2)
            {
                return JournalsImportMode.InsertOnly;
            }

            return (JournalsImportMode)Enum.Parse(typeof(JournalsImportMode), args[1], true);
        }
    }
}
