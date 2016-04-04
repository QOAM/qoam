namespace QOAM.Core.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using QOAM.Core.Repositories;

    using Validation;

    public class ExpirationChecker
    {
        private readonly IBaseScoreCardRepository baseScoreCardRepository;
        private readonly ExpirationCheckerNotification expirationCheckerNotification;
        private readonly ExpirationCheckerSettings expirationCheckerSettings;
        private readonly IMailSender mailSender;

        public ExpirationChecker(IBaseScoreCardRepository baseScoreCardRepository, ExpirationCheckerNotification expirationCheckerNotification, ExpirationCheckerSettings expirationCheckerSettings, IMailSender mailSender)
        {
            Requires.NotNull(baseScoreCardRepository, nameof(baseScoreCardRepository));
            Requires.NotNull(expirationCheckerNotification, nameof(expirationCheckerNotification));
            Requires.NotNull(expirationCheckerSettings, nameof(expirationCheckerSettings));
            Requires.NotNull(mailSender, nameof(mailSender));

            this.baseScoreCardRepository = baseScoreCardRepository;
            this.expirationCheckerNotification = expirationCheckerNotification;
            this.expirationCheckerSettings = expirationCheckerSettings;
            this.mailSender = mailSender;
        }

        public ArchivedScoreCardsResult ArchiveBaseScoreCardsThatHaveExpired()
        {
            var scoreCardsToBeArchived = this.baseScoreCardRepository.FindScoreCardsToBeArchived();

            foreach (var scoreCard in scoreCardsToBeArchived)
            {
                scoreCard.State = ScoreCardState.Archived;

                this.baseScoreCardRepository.InsertOrUpdate(scoreCard);
                this.baseScoreCardRepository.Save();

                var expiredMailMessage = this.expirationCheckerNotification.CreateArchivedMailMessage(scoreCard);

                this.mailSender.Send(expiredMailMessage).Wait();
            }

            return new ArchivedScoreCardsResult
                       {
                           NumberOfArchivedScoreCards = scoreCardsToBeArchived.Count
                       };
        }

        public AlmostExpiredScoreCardsResult NotifyUsersOfScoreCardsThatAlmostExpire()
        {
            var scoreCardsThatWillSoonBeArchived = this.baseScoreCardRepository.FindScoreCardsThatWillBeArchived(this.expirationCheckerSettings.SoonToBeArchivedWindow);
            var scoreCardsThatWillSoonBeArchivedNotifyTasks = scoreCardsThatWillSoonBeArchived.Select(this.expirationCheckerNotification.CreateSoonToBeExpiredMailMessage).Select(this.mailSender.Send).ToArray();

            Task.WaitAll(scoreCardsThatWillSoonBeArchivedNotifyTasks);

            var scoreCardsThatWillAlmostBeArchived = this.baseScoreCardRepository.FindScoreCardsThatWillBeArchived(this.expirationCheckerSettings.AlmostArchivedWindow);
            var scoreCardsThatWillAlmostBeArchivedNotifyTasks = scoreCardsThatWillAlmostBeArchived.Select(this.expirationCheckerNotification.CreateAlmostExpiredMailMessage).Select(this.mailSender.Send).ToArray();

            Task.WaitAll(scoreCardsThatWillAlmostBeArchivedNotifyTasks);
            
            return new AlmostExpiredScoreCardsResult
                       {
                           NumberOfSoonToBeArchivedNotificationsSent = scoreCardsThatWillSoonBeArchived.Count,
                           NumberOfAlmostArchivedNotificationsSent = scoreCardsThatWillAlmostBeArchived.Count
                       };
        }
    }
}