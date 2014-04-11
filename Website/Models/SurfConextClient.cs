namespace QOAM.Website.Models
{
    using System;
    using System.Collections.Generic;

    using DotNetOpenAuth.AspNet.Clients;

    public class SurfConextClient : OAuth2Client
    {
        public SurfConextClient()
            : base("SurfConext")
        {
        }

        protected override Uri GetServiceLoginUrl(Uri returnUrl)
        {
            throw new NotImplementedException();
        }

        protected override IDictionary<string, string> GetUserData(string accessToken)
        {
            throw new NotImplementedException();
        }

        protected override string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
            throw new NotImplementedException();
        }
    }
}