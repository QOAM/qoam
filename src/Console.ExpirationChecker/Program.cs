namespace QOAM.Console.ExpirationChecker
{
    using System;

    using Autofac;

    using NLog;

    using QOAM.Core.Services;

    internal static class Program
    {
        private static IContainer Container { get; } = DependencyInjectionConfig.RegisterComponents();

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static void Main()
        {
            try
            {
                ArchiveScoreCardsThatHaveExpired();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            try
            {
                NotifyUsersOfScoreCardsThatAlmostExpire();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        
        private static void ArchiveScoreCardsThatHaveExpired()
        {
            Logger.Info("Sending notifications to owners of expired JSC's...");

            var expirationChecker = Container.Resolve<ExpirationChecker>();
            var result = expirationChecker.ArchiveBaseScoreCardsThatHaveExpired();

            Logger.Info("Number of expired JSC's: {0}", result.NumberOfArchivedScoreCards);
        }

        private static void NotifyUsersOfScoreCardsThatAlmostExpire()
        {
            Logger.Info("Sending notifications to owners of soon to expire JSC's...");

            var expirationChecker = Container.Resolve<ExpirationChecker>();
            var result = expirationChecker.NotifyUsersOfScoreCardsThatAlmostExpire();

            Logger.Info("Number of soon to expire JSC's: {0}", result.NumberOfSoonToBeArchivedNotificationsSent);
            Logger.Info("Number of almost expired JSC's: {0}", result.NumberOfAlmostArchivedNotificationsSent);
        }
    }
}
