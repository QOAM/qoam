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
    { }

    
    public class SystemWebClientFactory : IWebClientFactory
    {
        public IWebClient Create()
        {
            return new SystemWebClient();
        }
    }
}