using System.Configuration;

namespace QOAM.Core.Import.CrossRef
{
    public class CrossRefSettings : ConfigurationSection
    {
        const string UrlPropertyName = "baseUrl";
        const string AllJournalsEndpointPropertyName = "journalsEndpoint";
        const string UserAgentHeaderPropertyName = "userAgent";

        [ConfigurationProperty(UrlPropertyName, IsRequired = true)]
        public string BaseUrl
        {
            get => (string)this[UrlPropertyName];
            set => this[UrlPropertyName] = value;
        }

        [ConfigurationProperty(AllJournalsEndpointPropertyName, IsRequired = true)]
        public string JournalsEndpoint
        {
            get => (string)this[AllJournalsEndpointPropertyName];
            set => this[AllJournalsEndpointPropertyName] = value;
        }

        [ConfigurationProperty(UserAgentHeaderPropertyName, IsRequired = true)]
        public string UserAgentHeader
        {
            get => (string)this[UserAgentHeaderPropertyName];
            set => this[UserAgentHeaderPropertyName] = value;
        }

        public string Cursor { get; set; } = "*";

        public string AllJournalsRequestUrl => $"{BaseUrl}/{JournalsEndpoint}?cursor={Cursor}&rows=1000";
        public string ByIssnRequestUrl => $"{BaseUrl}/{JournalsEndpoint}";
    }
}