namespace QOAM.Core.Import
{
    using System.Configuration;

    public class GeneralImportSettings : ConfigurationSection
    {
        private const string BatchSizePropertyName = "batchSize";

        [ConfigurationProperty(BatchSizePropertyName, IsRequired = true)]
        public int BatchSize
        {
            get => (int)this[BatchSizePropertyName];
            set => this[BatchSizePropertyName] = value;
        }
    }
}