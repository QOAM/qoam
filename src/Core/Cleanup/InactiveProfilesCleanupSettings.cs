namespace QOAM.Core.Cleanup
{
    using System.Configuration;

    public class InactiveProfilesCleanupSettings : ConfigurationSection
    {
        private const string NumberOfDaysBeforeArchivingPropertyName = "numberOfInactiveDaysBeforeRemoval";
        
        [ConfigurationProperty(NumberOfDaysBeforeArchivingPropertyName, IsRequired = true)]
        public int NumberOfInactiveDaysBeforeRemoval
        {
            get
            {
                return (int)this[NumberOfDaysBeforeArchivingPropertyName];
            }
            set
            {
                this[NumberOfDaysBeforeArchivingPropertyName] = value;
            }
        }
    }
}