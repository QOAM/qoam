namespace QOAM.Core.Import
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Web.Services.Protocols;
    using System.Xml;

    using QOAM.Core.Ulrichs;

    using Validation;

    public class UlrichsClient
    {
        private const string VersionNumberApi = "1";
        private const string VersionNumberLogOn = "1";
        private const string VersionNumberRequestReportToken = "1";
        private const string VersionNumberRequestPagedData = "1";

        private readonly UlrichsSettings ulrichsSettings;
        private readonly UlrichsWebService webService;

        private string sessionToken;
        private string reportToken;
        private string lastWebServiceError;

        public UlrichsClient(UlrichsSettings ulrichsSettings)
        {
            Requires.NotNull(ulrichsSettings, "ulrichsSettings");

            this.ulrichsSettings = ulrichsSettings;
            this.webService = new UlrichsWebService { CookieContainer = new CookieContainer() };
        }

        public int NumberOfPages { get; private set; }
        
        public bool TryLogOn()
        {
            for (var i = 0; i < this.ulrichsSettings.LogonAttempts; ++i)
            {
                if (this.LogOn())
                {
                    return true;
                }
            }

            return false;
        }

        private bool LogOn()
        {
            var logonReturnDocument = new XmlDocument();

            try
            {
                logonReturnDocument.LoadXml(this.webService.DataExchange(this.GetLogOnXml()));
            }
            catch (SoapException)
            {
                this.lastWebServiceError = ApiErrors.AccessDeniedOnLogOn;
                return false;
            }
            catch (Exception)
            {
                this.lastWebServiceError = ApiErrors.InvalidXml;
                return false;
            }

            try
            {
                this.sessionToken = logonReturnDocument.SelectSingleNode("/ReturnValueData/Token").InnerXml;
                Guid.Parse(this.sessionToken);
            }
            catch (FormatException)
            {
                this.lastWebServiceError = ApiErrors.InvalidSessionToken;
                return false;
            }
            catch (Exception)
            {
                this.lastWebServiceError = ApiErrors.NoSessionToken;
                return false;
            }

            return true;
        }

        private string GetLogOnXml()
        {
            return string.Format(XmlSnippets.LogOn, VersionNumberApi, this.ulrichsSettings.Username, this.ulrichsSettings.Password, VersionNumberLogOn);
        }

        public bool RequestReportToken()
        {
            var requestReportTokenReturn = new XmlDocument();

            try
            {
                var requestReportTokenReturnValue = this.webService.DataExchange(this.GetRequestReportTokenXml());
                requestReportTokenReturn.LoadXml(requestReportTokenReturnValue);
            }
            catch (SoapException se)
            {
                if (se.Message == SoapExceptions.CallLimitExceeded)
                {
                    this.lastWebServiceError = ApiErrors.CallLimitExceeded;
                }
                else if (se.Message == SoapExceptions.AccessDenied)
                {
                    this.lastWebServiceError = ApiErrors.AccessDenied;
                }
                else
                {
                    this.lastWebServiceError = se.Message;
                }

                return false;
            }
            catch (Exception)
            {
                this.lastWebServiceError = ApiErrors.InvalidXml;
                return false;
            }

            try
            {
                this.reportToken = requestReportTokenReturn.SelectSingleNode("/ReturnValueData/ReportToken").InnerXml;
                Guid.Parse(this.reportToken);
            }
            catch (FormatException)
            {
                this.lastWebServiceError = ApiErrors.InvalidReportToken;
                return false;
            }
            catch (Exception)
            {
                this.lastWebServiceError = ApiErrors.NoReportToken;
                return false;
            }

            this.NumberOfPages = Convert.ToInt32(requestReportTokenReturn.SelectSingleNode("/ReturnValueData/NumPages").InnerXml);

            if (this.NumberOfPages < 1)
            {
                this.lastWebServiceError = ApiErrors.InvalidNumberOfPages;
                return false;
            }

            return true;
        }

        private string GetRequestReportTokenXml()
        {
            return string.Format(XmlSnippets.RequestReportToken,
                VersionNumberApi,
                this.sessionToken,
                this.ulrichsSettings.FilterActive,
                this.ulrichsSettings.FilterAcademicScholarly,
                this.ulrichsSettings.FilterRefereed,
                this.ulrichsSettings.FilterElectronicEdition,
                VersionNumberRequestReportToken);
        }

        public string MakePageRequestAttempts(int pageNumber)
        {
            string singleDataPage = null;

            for (var i = 0; i < this.ulrichsSettings.PageRequestAttempts; i++)
            {
                singleDataPage = this.GetPageOfRecords(pageNumber);

                if (singleDataPage == null)
                {
                    if (this.lastWebServiceError == ApiErrors.AccessDenied)
                    {
                        this.TryLogOn();
                    }
                    else if (this.lastWebServiceError == ApiErrors.CallLimitExceeded)
                    {
                        Thread.Sleep(this.ulrichsSettings.SecondsToWaitForCallLimit * 1000);
                    }
                }
                else
                {
                    return singleDataPage;
                }
            }

            return singleDataPage;
        }

        private string GetPageOfRecords(int pageNumber)
        {
            var requestPagedDataReturn = new XmlDocument();
            try
            {
                var requestPagedDataReturnValue = this.webService.DataExchange(this.GetRequestPagedDataXml(pageNumber));
                requestPagedDataReturn.LoadXml(requestPagedDataReturnValue);
            }
            catch (SoapException se)
            {
                if (se.Message == SoapExceptions.CallLimitExceeded)
                {
                    this.lastWebServiceError = ApiErrors.CallLimitExceeded;
                }
                else if (se.Message == SoapExceptions.AccessDenied)
                {
                    this.lastWebServiceError = ApiErrors.AccessDenied;
                }
                else
                {
                    this.lastWebServiceError = se.Message;
                }

                return null;
            }
            catch (Exception)
            {
                this.lastWebServiceError = ApiErrors.InvalidXml;
                return null;
            }

            // Pull out the record tags and format the data for writing to output
            var ulrichsDataRecordsNode = requestPagedDataReturn.SelectSingleNode("/ReturnValueData/UlrichsDataRecords");

            if (string.IsNullOrEmpty(ulrichsDataRecordsNode.InnerXml))
            {
                this.lastWebServiceError = ApiErrors.EmptyPageData;
                return null;
            }

            return requestPagedDataReturn.OuterXml;
        }

        private string GetRequestPagedDataXml(int pageNumber)
        {
            return string.Format(XmlSnippets.RequestPagedData, VersionNumberApi, this.sessionToken, this.reportToken, pageNumber, VersionNumberRequestPagedData);
        }

        public bool LogOff()
        {
            try
            {
                var logoffReturnDocument = new XmlDocument();
                logoffReturnDocument.LoadXml(this.webService.DataExchange(this.GetLogOffXml()));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private string GetLogOffXml()
        {
            return string.Format(XmlSnippets.LogOff, VersionNumberApi, this.sessionToken);
        }

        private static class ApiErrors
        {
            public const string AccessDeniedOnLogOn = "Access Denied for that username and password.";
            public const string AccessDenied = "Access Denied.";
            public const string InvalidXml = "Did not get a well-formed XML package from that request.";
            public const string InvalidSessionToken = "Invalid session token returned from LogOn call.";
            public const string NoSessionToken = "No session token returned from LogOn call.";
            public const string CallLimitExceeded = "Called API too fast and got throttle message.";
            public const string InvalidReportToken = "Invalid report token returned from RequestReportToken call.";
            public const string NoReportToken = "No session token returned from RequestReportToken call.";
            public const string InvalidNumberOfPages = "Invalid number of pages returned from RequestReportToken call.";
            public const string EmptyPageData = "No valid records returned from RequestPagedData call.";
        }

        private static class SoapExceptions
        {
            public const string AccessDenied = "AccessDenied";
            public const string CallLimitExceeded = "CallLimitExceeded";
        }

        private static class XmlSnippets
        {
            public const string UlrichsDataRecordsOpenTag = "<UlrichsDataRecords>";
            public const string UlrichsDataRecordsCloseTag = "</UlrichsDataRecords>";

            public const string LogOn =
                @"<?xml version=""1.0"" encoding=""utf-16""?> 
                <ApiCallData>        
                       <Version>{0}</Version>          
                       <Api>LogOn</Api>        
                       <AuthenticationData>        
                               <UserName>{1}</UserName>        
                               <Password>{2}</Password>        
                               <Version>{3}</Version>          
                       </AuthenticationData>        
                </ApiCallData>";

            public const string RequestReportToken =
                @"<?xml version=""1.0"" encoding=""utf-16""?> 
		        <ApiCallData> 
			        <Version>{0}</Version>  
			        <Api>RequestReportToken</Api> 
			        <Token>{1}</Token>				
			        <RequestReportToken> 
				        <FilterActive>{2}</FilterActive>
				        <FilterAcademicScholarly>{3}</FilterAcademicScholarly>        
				        <FilterRefereed>{4}</FilterRefereed> 
				        <FilterElectronicEdition>{5}</FilterElectronicEdition>    
				        <Version>{6}</Version>  
			        </RequestReportToken> 
		        </ApiCallData>
            ";

            public const string RequestPagedData =
                @"<?xml version=""1.0"" encoding=""utf-16""?> 
		        <ApiCallData> 
			        <Version>{0}</Version>  
			        <Api>RequestPagedData</Api> 
			        <Token>{1}</Token>				
			        <RequestPagedData> 
				        <ReportToken>{2}</ReportToken>	
				        <PageNumber>{3}</PageNumber> 
				        <Version>{4}</Version>  
			        </RequestPagedData> 
		        </ApiCallData>
            ";

            public const string LogOff =
                @"<?xml version=""1.0"" encoding=""utf-16""?> 
		        <ApiCallData> 
			        <Version>{0}</Version> 
			        <Api>LogOff</Api> 
			        <Token>{1}</Token> 
		        </ApiCallData> 
            ";
        }
    }
}