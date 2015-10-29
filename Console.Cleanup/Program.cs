namespace QOAM.Console.Cleanup
{
    using System;
    using System.Collections.Generic;
    using Autofac;
    using Core.Cleanup;
    using NLog;

    public static class Program
    {
        private static IContainer Container { get; } = DependencyInjectionConfig.RegisterComponents();

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            try
            {
                Cleanup(GetCleanupMode(args));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private static void Cleanup(CleanupMode cleanupMode)
        {
            Logger.Info("Cleanup mode: {0}", cleanupMode);
            Logger.Info("Cleaning up...");

            GetCleanup(cleanupMode).Cleanup();

            Logger.Info("Finished cleaning up.");
        }

        public static ICleanup GetCleanup(CleanupMode cleanupMode)
        {
            switch (cleanupMode)
            {
                case CleanupMode.UnpublishedScoreCards:
                    return Container.Resolve<UnpublishedScoreCardsCleanup>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(cleanupMode));
            }
        }

        public static CleanupMode GetCleanupMode(IList<string> args)
        {
            if (args.Count < 1)
            {
                return CleanupMode.UnpublishedScoreCards;
            }

            return (CleanupMode)Enum.Parse(typeof(CleanupMode), args[0], true);
        }
    }
}
