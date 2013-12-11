namespace RU.Uci.OAMarket.Website.Models
{
    using System;
    using System.ComponentModel;
    using System.Configuration;

    public class GeneralSettings : ConfigurationSection
    {
        private const string ScoreCardLifeTimePropertyName = "scoreCardLifeTime";

        [ConfigurationProperty(ScoreCardLifeTimePropertyName, IsRequired = true)]
        [TypeConverter(typeof(TimeSpanConverter))]
        public TimeSpan ScoreCardLifeTime
        {
            get
            {
                return (TimeSpan)this[ScoreCardLifeTimePropertyName];
            }
            set
            {
                this[ScoreCardLifeTimePropertyName] = value;
            }
        }
    }
}