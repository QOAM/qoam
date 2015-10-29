namespace QOAM.Core.Cleanup
{
    using System.Configuration;

    public class UnpublishedScoreCardsCleanupSettings : ConfigurationSection
    {
        private const string NumberOfDaysBeforeArchivingPropertyName = "numberOfDaysBeforeArchiving";
        
        [ConfigurationProperty(NumberOfDaysBeforeArchivingPropertyName, IsRequired = true)]
        public int NumberOfDaysBeforeArchiving
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