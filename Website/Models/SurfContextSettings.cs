namespace RU.Uci.OAMarket.Website.Models
{
    using System.Configuration;

    public class SurfContextSettings : ConfigurationSection
    {
        private const string ClientIdPropertyName = "clientId";
        private const string ClientSecretPropertyName = "clientSecret";
        private const string AccessTokenUrlPropertyName = "accessTokenUrl";
        private const string AuthorizeUrlPropertyName = "authorizeUrl";
        private const string ProfileUrlPropertyName = "profileUrl";

        [ConfigurationProperty(ClientIdPropertyName, IsRequired = true)]
        public string ClientId
        {
            get
            {
                return (string)this[ClientIdPropertyName];
            }
            set
            {
                this[ClientIdPropertyName] = value;
            }
        }

        [ConfigurationProperty(ClientSecretPropertyName, IsRequired = true)]
        public string ClientSecret
        {
            get
            {
                return (string)this[ClientSecretPropertyName];
            }
            set
            {
                this[ClientSecretPropertyName] = value;
            }
        }

        [ConfigurationProperty(AccessTokenUrlPropertyName, IsRequired = true)]
        public string AccessTokenUrl
        {
            get
            {
                return (string)this[AccessTokenUrlPropertyName];
            }
            set
            {
                this[AccessTokenUrlPropertyName] = value;
            }
        }

        [ConfigurationProperty(AuthorizeUrlPropertyName, IsRequired = true)]
        public string AuthorizeUrl
        {
            get
            {
                return (string)this[AuthorizeUrlPropertyName];
            }
            set
            {
                this[AuthorizeUrlPropertyName] = value;
            }
        }

        [ConfigurationProperty(ProfileUrlPropertyName, IsRequired = true)]
        public string ProfileUrl
        {
            get
            {
                return (string)this[ProfileUrlPropertyName];
            }
            set
            {
                this[ProfileUrlPropertyName] = value;
            }
        }
    }
}