namespace QOAM.Website.Models
{
    using Postal;

    public class RequestValuationEmail : Email
    {
        public string From { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public string Url { get; set; }

        public bool IsKnownEmailAddress { get; set; }
    }
}