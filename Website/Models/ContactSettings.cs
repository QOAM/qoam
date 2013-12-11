namespace RU.Uci.OAMarket.Website.Models
{
    using System.Configuration;

    public class ContactSettings : ConfigurationSection
    {
        private const string SmtpHostPropertyName = "smtpHost";
        private const string ContactFormToPropertyName = "contactFormTo";
        private const string ContactFormSubjectPropertyName = "contactFormSubject";

        [ConfigurationProperty(SmtpHostPropertyName, IsRequired = true)]
        public string SmtpHost
        {
            get
            {
                return (string)this[SmtpHostPropertyName];
            }
            set
            {
                this[SmtpHostPropertyName] = value;
            }
        }

        [ConfigurationProperty(ContactFormToPropertyName, IsRequired = true)]
        public string ContactFormTo
        {
            get
            {
                return (string)this[ContactFormToPropertyName];
            }
            set
            {
                this[ContactFormToPropertyName] = value;
            }
        }

        [ConfigurationProperty(ContactFormSubjectPropertyName, IsRequired = true)]
        public string ContactFormSubject
        {
            get
            {
                return (string)this[ContactFormSubjectPropertyName];
            }
            set
            {
                this[ContactFormSubjectPropertyName] = value;
            }
        }
    }
}