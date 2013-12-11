namespace RU.Uci.OAMarket.Domain.Services
{
    using System;
    using System.ComponentModel;
    using System.Configuration;

    public class ExpirationCheckerSettings : ConfigurationSection
    {
        private const string ExpirationCheckerSettingsSectionName = "expirationChecker";
        private const string SoonToBeArchivedWindowPropertyName = "soonToBeArchivedWindow";
        private const string SoonToBeArchivedMailSubjectPropertyName = "soonToBeArchivedMailSubject";
        private const string SoonToBeArchivedMailMessagePropertyName = "soonToBeArchivedMailMessage";
        private const string AlmostArchivedWindowPropertyName = "almostArchivedWindow";
        private const string AlmostArchivedMailSubjectPropertyName = "almostArchivedMailSubject";
        private const string AlmostArchivedMailMessagePropertyName = "almostArchivedMailMessage";
        private const string ArchivedMailSubjectPropertyName = "archivedMailSubject";
        private const string ArchivedMailMessagePropertyName = "archivedMailMessage";
        private const string JournalScoreUrlPropertyName = "journalScoreUrl";
        private const string MailSenderPropertyName = "mailSender";
        private const string MailSenderNamePropertyName = "mailSenderName";
        private const string SmtpHostPropertyName = "smtpHost";

        private static readonly Lazy<ExpirationCheckerSettings> Instance = new Lazy<ExpirationCheckerSettings>(() => ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSection(ExpirationCheckerSettingsSectionName) as ExpirationCheckerSettings);
        
        public static ExpirationCheckerSettings Current
        {
            get
            {
                return Instance.Value;
            }
        }

        [ConfigurationProperty(SoonToBeArchivedWindowPropertyName, IsRequired = true)]
        [TypeConverter(typeof(TimeSpanConverter))]
        public TimeSpan SoonToBeArchivedWindow
        {
            get
            {
                return (TimeSpan)this[SoonToBeArchivedWindowPropertyName];
            }
            set
            {
                this[SoonToBeArchivedWindowPropertyName] = value;
            }
        }

        [ConfigurationProperty(SoonToBeArchivedMailSubjectPropertyName, IsRequired = true)]
        public string SoonToBeArchivedMailSubject
        {
            get
            {
                return (string)this[SoonToBeArchivedMailSubjectPropertyName];
            }
            set
            {
                this[SoonToBeArchivedMailSubjectPropertyName] = value;
            }
        }

        [ConfigurationProperty(SoonToBeArchivedMailMessagePropertyName, IsRequired = true)]
        public string SoonToBeArchivedMailMessage
        {
            get
            {
                return (string)this[SoonToBeArchivedMailMessagePropertyName];
            }
            set
            {
                this[SoonToBeArchivedMailMessagePropertyName] = value;
            }
        }

        [ConfigurationProperty(AlmostArchivedWindowPropertyName, IsRequired = true)]
        [TypeConverter(typeof(TimeSpanConverter))]
        public TimeSpan AlmostArchivedWindow
        {
            get
            {
                return (TimeSpan)this[AlmostArchivedWindowPropertyName];
            }
            set
            {
                this[AlmostArchivedWindowPropertyName] = value;
            }
        }

        [ConfigurationProperty(AlmostArchivedMailSubjectPropertyName, IsRequired = true)]
        public string AlmostArchivedMailSubject
        {
            get
            {
                return (string)this[AlmostArchivedMailSubjectPropertyName];
            }
            set
            {
                this[AlmostArchivedMailSubjectPropertyName] = value;
            }
        }

        [ConfigurationProperty(AlmostArchivedMailMessagePropertyName, IsRequired = true)]
        public string AlmostArchivedMailMessage
        {
            get
            {
                return (string)this[AlmostArchivedMailMessagePropertyName];
            }
            set
            {
                this[AlmostArchivedMailMessagePropertyName] = value;
            }
        }
        
        [ConfigurationProperty(ArchivedMailSubjectPropertyName, IsRequired = true)]
        public string ArchivedMailSubject
        {
            get
            {
                return (string)this[ArchivedMailSubjectPropertyName];
            }
            set
            {
                this[ArchivedMailSubjectPropertyName] = value;
            }
        }

        [ConfigurationProperty(ArchivedMailMessagePropertyName, IsRequired = true)]
        public string ArchivedMailMessage
        {
            get
            {
                return (string)this[ArchivedMailMessagePropertyName];
            }
            set
            {
                this[ArchivedMailMessagePropertyName] = value;
            }
        }

        [ConfigurationProperty(JournalScoreUrlPropertyName, IsRequired = true)]
        public string JournalScoreUrl
        {
            get
            {
                return (string)this[JournalScoreUrlPropertyName];
            }
            set
            {
                this[JournalScoreUrlPropertyName] = value;
            }
        }

        [ConfigurationProperty(MailSenderPropertyName, IsRequired = true)]
        public string MailSender
        {
            get
            {
                return (string)this[MailSenderPropertyName];
            }
            set
            {
                this[MailSenderPropertyName] = value;
            }
        }

        [ConfigurationProperty(MailSenderNamePropertyName, IsRequired = true)]
        public string MailSenderName
        {
            get
            {
                return (string)this[MailSenderNamePropertyName];
            }
            set
            {
                this[MailSenderNamePropertyName] = value;
            }
        }

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
    }
}