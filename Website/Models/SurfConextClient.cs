namespace RU.Uci.OAMarket.Website.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net;
    using System.Text;

    using DotNetOpenAuth.AspNet.Clients;
    using DotNetOpenAuth.Messaging;

    using Newtonsoft.Json;

    using Validation;

    public class SurfConextClient : OAuth2Client
    {
        private const string UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; MDDC)";

        private readonly SurfContextSettings settings;

        public SurfConextClient(SurfContextSettings settings)
            : base("SurfConext")
        {
            Requires.NotNull(settings, "settings");
            
            this.settings = settings;
        }

        /// <summary>
        /// Gets the full url pointing to the login page for this client. The url should include the specified return url so that when the login completes, user is redirected back to that url.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>
        /// An absolute URL.
        /// </returns>
        protected override Uri GetServiceLoginUrl(Uri returnUrl)
        {
            var uriBuilder = new UriBuilder(this.settings.AuthorizeUrl);
            uriBuilder.AppendQueryArgument("client_id", this.settings.ClientId);
            uriBuilder.AppendQueryArgument("redirect_uri", returnUrl.ToString());
            uriBuilder.AppendQueryArgument("response_type", "code");
            uriBuilder.AppendQueryArgument("scope", "read");

            return uriBuilder.Uri;
        }

        /// <summary>
        /// Given the access token, gets the logged-in user's data. The returned dictionary must include two keys 'id', and 'username'.
        /// </summary>
        /// <param name="accessToken">The access token of the current user.</param>
        /// <returns>
        /// A dictionary contains key-value pairs of user data
        /// </returns>
        protected override IDictionary<string, string> GetUserData(string accessToken)
        {
            using (var webClient = CreateWebClient())
            {
                webClient.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;

                var uriBuilder = new UriBuilder(this.settings.ProfileUrl);
                uriBuilder.AppendQueryArgument("access_token", accessToken);

                var profileResponse = webClient.DownloadString(uriBuilder.Uri);
                var profile = JsonConvert.DeserializeObject<SurfConextProfile>(profileResponse);
                
                return new Dictionary<string, string>
                       {
                           { "id", profile.entry.id }, 
                           { "displayName", profile.entry.displayName },
                           { "account_username", profile.entry.accounts.Select(a => a.username).FirstOrDefault() }, 
                           { "emails_value", string.Join(", ", profile.entry.emails.Select(o => o.value)) }, 
                           { "organisations_name", string.Join(", ", profile.entry.organizations.Select(o => o.name)) }
                       };
            }
        }

        /// <summary>
        /// Queries the access token from the specified authorization code.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="authorizationCode">The authorization code.</param>
        /// <returns>
        /// The access token
        /// </returns>
        protected override string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
            using (var webClient = CreateWebClient())
            {
                var parameters = new NameValueCollection
                                          {
                                              { "client_id", this.settings.ClientId },
                                              { "client_secret", this.settings.ClientSecret },
                                              { "redirect_uri", returnUrl.ToString() },
                                              { "grant_type", "authorization_code" },
                                              { "scope", "read" },
                                              { "code", authorizationCode },
                                          };

                var accessTokenResponse = Encoding.UTF8.GetString(webClient.UploadValues(this.settings.AccessTokenUrl, parameters));
                var accessToken = JsonConvert.DeserializeObject<SurfConextAccessToken>(accessTokenResponse);
                return accessToken.access_token;
            }
        }

        private static WebClient CreateWebClient()
        {
            var webClient = new WebClient();
            webClient.Headers["User-Agent"] = UserAgent;
            return webClient;
        }
    }
}