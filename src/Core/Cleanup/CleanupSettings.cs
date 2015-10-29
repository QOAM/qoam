namespace QOAM.Core.Cleanup
{
    using System.Configuration;

    public class CleanupSettings : ConfigurationSectionGroup
    {
        private const string UnpublishedScoreCardsSectionName = "unpublishedScoreCards";

        public static CleanupSettings Current { get; } = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSectionGroup("cleanup") as CleanupSettings;

        [ConfigurationProperty(UnpublishedScoreCardsSectionName)]
        public UnpublishedScoreCardsCleanupSettings UnpublishedScoreCardsCleanup => (UnpublishedScoreCardsCleanupSettings)this.Sections[UnpublishedScoreCardsSectionName];
    }
}