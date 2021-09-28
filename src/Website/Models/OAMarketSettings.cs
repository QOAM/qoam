using QOAM.Core.Import.CrossRef;
using QOAM.Core.Import.JournalTOCs;

namespace QOAM.Website.Models
{
    using System;
    using System.Configuration;
    using System.Web.Configuration;

    using QOAM.Core.Import;

    public class OAMarketSettings : ConfigurationSectionGroup
    {
        const string OAMarketSectionGroupName = "oamarket";
        const string ContactSectionName = "contact";
        const string GeneralSectionName = "general";
        const string GeneralImportSectionName = "generalImport";
        const string UlrichsSectionName = "ulrichs";
        const string DoajSectionName = "doaj";
        const string JournalTocsSectionName = "journaltocs";
        const string CrossRefSectionName = "crossRef";

        static readonly Lazy<OAMarketSettings> Instance = new Lazy<OAMarketSettings>(() => WebConfigurationManager.OpenWebConfiguration("~").GetSectionGroup(OAMarketSectionGroupName) as OAMarketSettings);

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

        [ConfigurationProperty(CrossRefSectionName)]
        public CrossRefSettings CrossRef => (CrossRefSettings)Sections[CrossRefSectionName];
    }
}