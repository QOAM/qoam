using System.Configuration;

namespace QOAM.Core.Import.JournalTOCs
{
    public class JournalTocsSettings : ConfigurationSection
    {
        const string UrlPropertyName = "baseUrl";
        const string AllJournalsEndpointPropertyName = "allJournalsEndpoint";
        const string ByIssnEndpointPropertyName = "byIssnEndpoint";
        const string SuiPropertyName = "sui";
        const string IpPropertyName = "ip";
        const string TestPropertyName = "test";

        [ConfigurationProperty(UrlPropertyName, IsRequired = true)]
        public string BaseUrl
        {
            get { return (string)this[UrlPropertyName]; }
            set { this[UrlPropertyName] = value; }
        }

        [ConfigurationProperty(AllJournalsEndpointPropertyName, IsRequired = true)]
        public string AllJournalsEndpoint
        {
            get { return (string)this[AllJournalsEndpointPropertyName]; }
            set { this[AllJournalsEndpointPropertyName] = value; }
        }

        [ConfigurationProperty(ByIssnEndpointPropertyName, IsRequired = true)]
        public string ByIssnEndpoint
        {
            get { return (string)this[ByIssnEndpointPropertyName]; }
            set { this[ByIssnEndpointPropertyName] = value; }
        }

        [ConfigurationProperty(SuiPropertyName, IsRequired = true)]
        public string Sui
        {
            get { return (string)this[SuiPropertyName]; }
            set { this[SuiPropertyName] = value; }
        }

        [ConfigurationProperty(IpPropertyName, IsRequired = true)]
        public string Ip
        {
            get { return (string)this[IpPropertyName]; }
            set { this[IpPropertyName] = value; }
        }

        [ConfigurationProperty(TestPropertyName, IsRequired = true)]
        public bool Test
        {
            get { return (bool)this[TestPropertyName]; }
            set { this[TestPropertyName] = value; }
        }

        public string AllJournalsRequestUrl => $"{BaseUrl}{AllJournalsEndpoint}?sui={Sui}&ip={Ip}&test={Test}";
        public string ByIssnRequestUrl => $"{BaseUrl}{ByIssnEndpoint}?sui={Sui}&ip={Ip}&test={Test}&checkArchive=doaj";
    }
}