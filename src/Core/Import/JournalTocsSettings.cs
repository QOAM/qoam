using System.Configuration;

namespace QOAM.Core.Import
{
    public class JournalTocsSettings : ConfigurationSection
    {
        const string UrlPropertyName = "url";
        const string SuiPropertyName = "sui";
        const string IpPropertyName = "ip";
        const string TestPropertyName = "test";

        [ConfigurationProperty(UrlPropertyName, IsRequired = true)]
        public string Url
        {
            get { return (string)this[UrlPropertyName]; }
            set { this[UrlPropertyName] = value; }
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

        public string RequestUrl => $"{Url}?sui={Sui}&ip={Ip}&test={Test}";
    }
}