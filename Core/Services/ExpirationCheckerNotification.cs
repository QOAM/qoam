namespace QOAM.Core.Services
{
    using System;
    using System.Globalization;
    using System.Net.Mail;

    using Validation;

    public class ExpirationCheckerNotification
    {
        private readonly ExpirationCheckerSettings expirationCheckerSettings;

        public ExpirationCheckerNotification(ExpirationCheckerSettings expirationCheckerSettings)
        {
            Requires.NotNull(expirationCheckerSettings, "expirationCheckerSettings");

            this.expirationCheckerSettings = expirationCheckerSettings;
        }
        
        public MailMessage CreateSoonToBeExpiredMailMessage(BaseScoreCard scoreCard)
        {
            return this.CreateMailMessage(scoreCard, this.GetSoonToBeArchivedMailBody, this.expirationCheckerSettings.SoonToBeArchivedMailSubject);
        }

        private string GetSoonToBeArchivedMailBody(BaseScoreCard scoreCard)
        {
            return this.GetToBeArchivedMailBody(scoreCard, this.expirationCheckerSettings.SoonToBeArchivedMailMessage);
        }

        public MailMessage CreateAlmostExpiredMailMessage(BaseScoreCard scoreCard)
        {
            return this.CreateMailMessage(scoreCard, this.GetAlmostArchivedMailBody, this.expirationCheckerSettings.AlmostArchivedMailSubject);
        }

        private string GetAlmostArchivedMailBody(BaseScoreCard scoreCard)
        {
            return this.GetToBeArchivedMailBody(scoreCard, this.expirationCheckerSettings.AlmostArchivedMailMessage);
        }

        private string GetToBeArchivedMailBody(BaseScoreCard scoreCard, string toBeArchivedMailMessage)
        {
            return toBeArchivedMailMessage
                       .Replace("[journalscoreurl]", this.expirationCheckerSettings.JournalScoreUrl.TrimEnd('/'))
                       .Replace("[newline]", "\r\n")
                       .Replace("[username]", scoreCard.UserProfile.DisplayName)
                       .Replace("[journaltitle]", scoreCard.Journal.Title)
                       .Replace("[journalid]", scoreCard.JournalId.ToString(CultureInfo.InvariantCulture))
                       .Replace("[journalexpirationdate]", scoreCard.DateExpiration.Value.ToShortDateString());
        }

        public MailMessage CreateArchivedMailMessage(BaseScoreCard scoreCard)
        {
            return this.CreateMailMessage(scoreCard, this.GetArchivedMailBody, this.expirationCheckerSettings.ArchivedMailSubject);
        }

        private string GetArchivedMailBody(BaseScoreCard scoreCard)
        {
            return this.expirationCheckerSettings.ArchivedMailMessage
                       .Replace("[journalscoreurl]", this.expirationCheckerSettings.JournalScoreUrl.TrimEnd('/'))
                       .Replace("[newline]", "\r\n")
                       .Replace("[username]", scoreCard.UserProfile.DisplayName)
                       .Replace("[journaltitle]", scoreCard.Journal.Title)
                       .Replace("[journalid]", scoreCard.JournalId.ToString(CultureInfo.InvariantCulture));
        }

        private MailMessage CreateMailMessage(BaseScoreCard scoreCard, Func<BaseScoreCard, string> mailBodyFunc, string mailSubject)
        {
            Requires.NotNull(scoreCard, "scoreCard");
            Requires.NotNull(mailBodyFunc, "mailBodyFunc");
            Requires.NotNullOrEmpty(mailSubject, "mailSubject");
            
            return new MailMessage(this.GetMailSender(), new MailAddress(scoreCard.UserProfile.Email))
            {
                Body = mailBodyFunc(scoreCard),
                Subject = mailSubject
            };
        }

        private MailAddress GetMailSender()
        {
            return new MailAddress(this.expirationCheckerSettings.MailSender, this.expirationCheckerSettings.MailSenderName);
        }
    }
}