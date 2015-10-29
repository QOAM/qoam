namespace QOAM.Core.Import
{
    using System.Configuration;

    public class UlrichsSettings : ConfigurationSection
    {
        private const string UsernamePropertyName = "username";
        private const string PasswordPropertyName = "password";
        private const string CacheDirectoryPropertyName = "cacheDirectory";
        private const string CacheLifetimeInDaysPropertyName = "cacheLifetimeInDays";
        private const string LogonAttemptsPropertyName = "logonAttempts";
        private const string PageRequestAttemptsPropertyName = "pageRequestAttempts";
        private const string SecondsToWaitForCallLimitPropertyName = "secondsToWaitForCallLimit";
        private const string FilterActivePropertyName = "filterActive";
        private const string FilterAcademicScholarlyPropertyName = "filterAcademicScholarly";
        private const string FilterRefereedPropertyName = "filterRefereed";
        private const string FilterElectronicEditionPropertyName = "filterElectronicEdition";
        
        [ConfigurationProperty(UsernamePropertyName, IsRequired = true)]
        public string Username
        {
            get
            {
                return (string)this[UsernamePropertyName];
            }
            set
            {
                this[UsernamePropertyName] = value;
            }
        }

        [ConfigurationProperty(PasswordPropertyName, IsRequired = true)]
        public string Password
        {
            get
            {
                return (string)this[PasswordPropertyName];
            }
            set
            {
                this[PasswordPropertyName] = value;
            }
        }

        [ConfigurationProperty(CacheDirectoryPropertyName, IsRequired = true)]
        public string CacheDirectory
        {
            get
            {
                return (string)this[CacheDirectoryPropertyName];
            }
            set
            {
                this[CacheDirectoryPropertyName] = value;
            }
        }

        [ConfigurationProperty(CacheLifetimeInDaysPropertyName, IsRequired = false, DefaultValue = 7)]
        public int CacheLifetimeInDays
        {
            get
            {
                return (int)this[CacheLifetimeInDaysPropertyName];
            }
            set
            {
                this[CacheLifetimeInDaysPropertyName] = value;
            }
        }

        [ConfigurationProperty(LogonAttemptsPropertyName, IsRequired = false, DefaultValue = 3)]
        public int LogonAttempts
        {
            get
            {
                return (int)this[LogonAttemptsPropertyName];
            }
            set
            {
                this[LogonAttemptsPropertyName] = value;
            }
        }

        [ConfigurationProperty(PageRequestAttemptsPropertyName, IsRequired = false, DefaultValue = 5)]
        public int PageRequestAttempts
        {
            get
            {
                return (int)this[PageRequestAttemptsPropertyName];
            }
            set
            {
                this[PageRequestAttemptsPropertyName] = value;
            }
        }

        [ConfigurationProperty(SecondsToWaitForCallLimitPropertyName, IsRequired = false, DefaultValue = 4)]
        public int SecondsToWaitForCallLimit
        {
            get
            {
                return (int)this[SecondsToWaitForCallLimitPropertyName];
            }
            set
            {
                this[SecondsToWaitForCallLimitPropertyName] = value;
            }
        }

        [ConfigurationProperty(FilterActivePropertyName, IsRequired = false, DefaultValue = false)]
        public bool FilterActive
        {
            get
            {
                return (bool)this[FilterActivePropertyName];
            }
            set
            {
                this[FilterActivePropertyName] = value;
            }
        }

        [ConfigurationProperty(FilterAcademicScholarlyPropertyName, IsRequired = false, DefaultValue = false)]
        public bool FilterAcademicScholarly
        {
            get
            {
                return (bool)this[FilterAcademicScholarlyPropertyName];
            }
            set
            {
                this[FilterAcademicScholarlyPropertyName] = value;
            }
        }

        [ConfigurationProperty(FilterRefereedPropertyName, IsRequired = false, DefaultValue = false)]
        public bool FilterRefereed
        {
            get
            {
                return (bool)this[FilterRefereedPropertyName];
            }
            set
            {
                this[FilterRefereedPropertyName] = value;
            }
        }

        [ConfigurationProperty(FilterElectronicEditionPropertyName, IsRequired = false, DefaultValue = false)]
        public bool FilterElectronicEdition
        {
            get
            {
                return (bool)this[FilterElectronicEditionPropertyName];
            }
            set
            {
                this[FilterElectronicEditionPropertyName] = value;
            }
        }
    }
}