namespace QOAM.Core.Services
{
    using System.Net.Mail;
    using System.Threading.Tasks;

    public interface IMailSender
    {
        Task Send(MailMessage message);
    }
}