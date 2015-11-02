namespace QOAM.Core.Cleanup
{
    using System.Configuration;

    public class CleanupSettings : ConfigurationSectionGroup
    {
        private const string UnpublishedScoreCardsSectionName = "unpublishedScoreCards";
        private const string InactiveProfilesSectionName = "inactiveProfiles";

        public static CleanupSettings Current { get; } = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSectionGroup("cleanup") as CleanupSettings;

        [ConfigurationProperty(UnpublishedScoreCardsSectionName)]
        public UnpublishedScoreCardsCleanupSettings UnpublishedScoreCards => (UnpublishedScoreCardsCleanupSettings)this.Sections[UnpublishedScoreCardsSectionName];

        [ConfigurationProperty(InactiveProfilesSectionName)]
        public InactiveProfilesCleanupSettings InactiveProfiles => (InactiveProfilesCleanupSettings)this.Sections[InactiveProfilesSectionName];
    }
}