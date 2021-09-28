using System.Configuration;
using QOAM.Core.Import.CrossRef;
using QOAM.Core.Import.JournalTOCs;

namespace QOAM.Core.Import
{
    public class ImportSettings : ConfigurationSectionGroup
    {
        const string GeneralSectionName = "general";
        const string UlrichsSectionName = "ulrichs";
        const string DoajSectionName = "doaj";
        const string JournalTocsSectionName = "journaltocs";
        const string CrossRefSectionName = "crossRef";

        public static ImportSettings Current { get; } = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSectionGroup("import") as ImportSettings;

        [ConfigurationProperty(GeneralSectionName)]
        public GeneralImportSettings General => (GeneralImportSettings) Sections[GeneralSectionName];

        [ConfigurationProperty(UlrichsSectionName)]
        public UlrichsSettings Ulrichs => (UlrichsSettings) Sections[UlrichsSectionName];

        [ConfigurationProperty(DoajSectionName)]
        public DoajSettings Doaj => (DoajSettings) Sections[DoajSectionName];

        [ConfigurationProperty(JournalTocsSectionName)]
        public JournalTocsSettings JournalTocs => (JournalTocsSettings) Sections[JournalTocsSectionName];

        [ConfigurationProperty(CrossRefSectionName)]
        public CrossRefSettings CrossRef => (CrossRefSettings) Sections[CrossRefSectionName];
    }
}