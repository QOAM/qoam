namespace QOAM.Core.Import
{
    using System.Configuration;

    public class ImportSettings : ConfigurationSectionGroup
    {
        private const string GeneralSectionName = "general";
        private const string UlrichsSectionName = "ulrichs";
        private const string DoajSectionName = "doaj";
        
        private static readonly ImportSettings SettingsInstance = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSectionGroup("import") as ImportSettings;

        public static ImportSettings Current
        {
            get
            {
                return SettingsInstance;
            }
        }

        [ConfigurationProperty(GeneralSectionName)]
        public GeneralImportSettings General
        {
            get
            {
                return (GeneralImportSettings)this.Sections[GeneralSectionName];
            }
        }

        [ConfigurationProperty(UlrichsSectionName)]
        public UlrichsSettings Ulrichs
        {
            get
            {
                return (UlrichsSettings)this.Sections[UlrichsSectionName];
            }
        }

        [ConfigurationProperty(DoajSectionName)]
        public DoajSettings Doaj
        {
            get
            {
                return (DoajSettings)this.Sections[DoajSectionName];
            }
        }
    }
}