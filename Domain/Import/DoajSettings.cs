namespace RU.Uci.OAMarket.Domain.Import
{
    using System.Configuration;

    public class DoajSettings : ConfigurationSection
    {
        private const string CsvUrlPropertyName = "csvUrl";

        [ConfigurationProperty(CsvUrlPropertyName, IsRequired = true)]
        public string CsvUrl
        {
            get
            {
                return (string)this[CsvUrlPropertyName];
            }
            set
            {
                this[CsvUrlPropertyName] = value;
            }
        }
    }
}