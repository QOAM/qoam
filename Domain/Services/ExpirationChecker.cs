namespace RU.Uci.OAMarket.Domain.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using RU.Uci.OAMarket.Domain.Repositories;

    using Validation;

    public class ExpirationChecker
    {
        private readonly IScoreCardRepository scoreCardRepository;
        private readonly ExpirationCheckerNotification expirationCheckerNotification;
        private readonly ExpirationCheckerSettings expirationCheckerSettings;
        private readonly IMailSender mailSender;

        public ExpirationChecker(IScoreCardRepository scoreCardRepository, ExpirationCheckerNotification expirationCheckerNotification, ExpirationCheckerSettings expirationCheckerSettings, IMailSender mailSender)
        {
            Requires.NotNull(scoreCardRepository, "scoreCardRepository");
            Requires.NotNull(expirationCheckerNotification, "expirationCheckerNotification");
            Requires.NotNull(expirationCheckerSettings, "expirationCheckerSettings");
            Requires.NotNull(mailSender, "mailSender");

            this.scoreCardRepository = scoreCardRepository;
            this.expirationCheckerNotification = expirationCheckerNotification;
            this.expirationCheckerSettings = expirationCheckerSettings;
            this.mailSender = mailSender;
        }

        public ArchivedScoreCardsResult ArchiveScoreCardsThatHaveExpired()
        {
            var scoreCardsToBeArchived = this.scoreCardRepository.FindScoreCardsToBeArchived();

            foreach (var scoreCard in scoreCardsToBeArchived)
            {
                scoreCard.State = ScoreCardState.Expired;

                this.scoreCardRepository.Update(scoreCard);
                this.scoreCardRepository.Save();

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
            var scoreCardsThatWillSoonBeArchived = this.scoreCardRepository.FindScoreCardsThatWillBeArchived(this.expirationCheckerSettings.SoonToBeArchivedWindow);
            var scoreCardsThatWillSoonBeArchivedNotifyTasks = scoreCardsThatWillSoonBeArchived.Select(this.expirationCheckerNotification.CreateSoonToBeExpiredMailMessage).Select(this.mailSender.Send).ToArray();

            Task.WaitAll(scoreCardsThatWillSoonBeArchivedNotifyTasks);

            var scoreCardsThatWillAlmostBeArchived = this.scoreCardRepository.FindScoreCardsThatWillBeArchived(this.expirationCheckerSettings.AlmostArchivedWindow);
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