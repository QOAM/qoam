namespace RU.Uci.OAMarket.Website.Models
{
    using System;
    using System.Configuration;
    using System.Web.Configuration;

    using RU.Uci.OAMarket.Domain.Import;

    public class OAMarketSettings : ConfigurationSectionGroup
    {
        private const string OAMarketSectionGroupName = "oamarket";
        private const string ContactSectionName = "contact";
        private const string GeneralSectionName = "general";
        private const string SurfContextSectionName = "surfContext";
        private const string GeneralImportSectionName = "generalImport";
        private const string UlrichsSectionName = "ulrichs";
        private const string DoajSectionName = "doaj";

        private static readonly Lazy<OAMarketSettings> Instance = new Lazy<OAMarketSettings>(() => WebConfigurationManager.OpenWebConfiguration("~").GetSectionGroup(OAMarketSectionGroupName) as OAMarketSettings);

        public static OAMarketSettings Current
        {
            get
            {
                return Instance.Value;
            }
        }

        [ConfigurationProperty(ContactSectionName)]
        public ContactSettings Contact
        {
            get
            {
                return (ContactSettings)this.Sections[ContactSectionName];
            }
        }

        [ConfigurationProperty(GeneralSectionName)]
        public GeneralSettings General
        {
            get
            {
                return (GeneralSettings)this.Sections[GeneralSectionName];
            }
        }

        [ConfigurationProperty(SurfContextSectionName)]
        public SurfContextSettings SurfContext
        {
            get
            {
                return (SurfContextSettings)this.Sections[SurfContextSectionName];
            }
        }

        [ConfigurationProperty(GeneralImportSectionName)]
        public GeneralImportSettings GeneralImport
        {
            get
            {
                return (GeneralImportSettings)this.Sections[GeneralImportSectionName];
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