namespace QOAM.Console.ExpirationChecker
{
    using System;

    using Autofac;

    using QOAM.Core.Services;

    internal static class Program
    {
        private static IContainer container;

        private static void Main()
        {
            container = DependencyInjectionConfig.RegisterComponents();

            try
            {
                ArchiveScoreCardsThatHaveExpired();
            }
            catch (Exception ex)
            {
                Console.WriteLine(Strings.Error, ex);
            }

            try
            {
                NotifyUsersOfScoreCardsThatAlmostExpire();
            }
            catch (Exception ex)
            {
                Console.WriteLine(Strings.Error, ex);
            }
        }
        
        private static void ArchiveScoreCardsThatHaveExpired()
        {
            Console.WriteLine(Strings.CheckingExpiredJournals);

            var expirationChecker = container.Resolve<ExpirationChecker>();
            var result = expirationChecker.ArchiveBaseScoreCardsThatHaveExpired();

            Console.WriteLine(Strings.CheckedExpiredJournals, result.NumberOfArchivedScoreCards);
        }

        private static void NotifyUsersOfScoreCardsThatAlmostExpire()
        {
            Console.WriteLine(Strings.CheckingAlmostExpiredJournals);

            var expirationChecker = container.Resolve<ExpirationChecker>();
            var result = expirationChecker.NotifyUsersOfScoreCardsThatAlmostExpire();

            Console.WriteLine(Strings.CheckedSoonToBeExpiredJournals, result.NumberOfSoonToBeArchivedNotificationsSent);
            Console.WriteLine(Strings.CheckedAlmostExpiredJournals, result.NumberOfAlmostArchivedNotificationsSent);
        }
    }
}
