using System;
using System.Net;
using System.Text;

namespace QOAM.Core.Import
{
    public interface IWebClient : IDisposable
    {
        Encoding Encoding { get; set; }
        string DownloadString(string address);
    }

    public interface IWebClientFactory
    {
        IWebClient Create();
    }

    public class SystemWebClient : WebClient, IWebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            var webRequest = base.GetWebRequest(address);

            if (webRequest == null)
                return null;

            webRequest.Timeout = 5 * 60 * 1000;
            
            return webRequest;
        }
    }

    
    public class SystemWebClientFactory : IWebClientFactory
    {
        public IWebClient Create()
        {
            return new SystemWebClient();
        }
    }
}