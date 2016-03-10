namespace QOAM.Core.Cleanup
{
    using System;
    using System.Configuration;

    public class CleanupSettings : ConfigurationSection
    {
        private const string SectionName = "cleanup";
        private const string NumberOfDaysBeforeArchivingInactiveProfilesPropertyName = "numberOfInactiveDaysBeforeArchivingInactiveProfiles";
        private const string NumberOfDaysBeforeArchivingUnpublishedScoreCardsPropertyName = "numberOfDaysBeforeArchivingUnpublishedScoreCards";

        private static readonly Lazy<CleanupSettings> Instance = new Lazy<CleanupSettings>(() => ConfigurationManager.GetSection(SectionName) as CleanupSettings);

        public static CleanupSettings Current => Instance.Value;
        
        [ConfigurationProperty(NumberOfDaysBeforeArchivingInactiveProfilesPropertyName, IsRequired = true)]
        public int NumberOfInactiveDaysBeforeArchivingInactiveProfiles
        {
            get
            {
                return (int)this[NumberOfDaysBeforeArchivingInactiveProfilesPropertyName];
            }
            set
            {
                this[NumberOfDaysBeforeArchivingInactiveProfilesPropertyName] = value;
            }
        }
        
        [ConfigurationProperty(NumberOfDaysBeforeArchivingUnpublishedScoreCardsPropertyName, IsRequired = true)]
        public int NumberOfDaysBeforeArchivingUnpublishedScoreCards
        {
            get
            {
                return (int)this[NumberOfDaysBeforeArchivingUnpublishedScoreCardsPropertyName];
            }
            set
            {
                this[NumberOfDaysBeforeArchivingUnpublishedScoreCardsPropertyName] = value;
            }
        }
    }
}