namespace QOAM.Core.Services
{
    using System;
    using System.Net.Mail;
    using System.Threading.Tasks;

    using Validation;

    public class MailSender : IMailSender, IDisposable
    {
        private readonly SmtpClient smtpClient;

        public MailSender(string smtpHost)
        {
            Requires.NotNullOrEmpty(smtpHost, "smtpHost");

            this.smtpClient = new SmtpClient(smtpHost);
        }

        public Task Send(MailMessage message)
        {
            Requires.NotNull(message, "message");

            return this.smtpClient.SendMailAsync(message);
        }

        public void Dispose()
        {
            this.smtpClient.Dispose();
        }
    }
}