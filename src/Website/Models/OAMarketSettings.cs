namespace QOAM.Website.Models
{
    using System;
    using System.Configuration;
    using System.Web.Configuration;

    using QOAM.Core.Import;

    public class OAMarketSettings : ConfigurationSectionGroup
    {
        private const string OAMarketSectionGroupName = "oamarket";
        private const string ContactSectionName = "contact";
        private const string GeneralSectionName = "general";
        private const string GeneralImportSectionName = "generalImport";
        private const string UlrichsSectionName = "ulrichs";
        private const string DoajSectionName = "doaj";
        private const string JournalTocsSectionName = "journaltocs";

        private static readonly Lazy<OAMarketSettings> Instance = new Lazy<OAMarketSettings>(() => WebConfigurationManager.OpenWebConfiguration("~").GetSectionGroup(OAMarketSectionGroupName) as OAMarketSettings);

        public static OAMarketSettings Current => Instance.Value;

        [ConfigurationProperty(ContactSectionName)]
        public ContactSettings Contact => (ContactSettings)Sections[ContactSectionName];

        [ConfigurationProperty(GeneralSectionName)]
        public GeneralSettings General => (GeneralSettings)Sections[GeneralSectionName];

        [ConfigurationProperty(GeneralImportSectionName)]
        public GeneralImportSettings GeneralImport => (GeneralImportSettings)Sections[GeneralImportSectionName];

        [ConfigurationProperty(UlrichsSectionName)]
        public UlrichsSettings Ulrichs => (UlrichsSettings)Sections[UlrichsSectionName];

        [ConfigurationProperty(DoajSectionName)]
        public DoajSettings Doaj => (DoajSettings)Sections[DoajSectionName];

        [ConfigurationProperty(JournalTocsSectionName)]
        public JournalTocsSettings JournalTocs => (JournalTocsSettings)Sections[JournalTocsSectionName];
    }
}