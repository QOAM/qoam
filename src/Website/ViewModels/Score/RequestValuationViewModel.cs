namespace QOAM.Website.ViewModels.Score
{
    using System.ComponentModel.DataAnnotations;
    using QOAM.Website.Models;

    public class RequestValuationViewModel
    {
        public int JournalId { get; set; }

        public string JournalTitle { get; set; }

        public string JournalISSN { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email is not valid")]
        public string EmailFrom { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email is not valid")]
        public string EmailTo { get; set; }

        public string RecipientName { get; set; }

        public string EmailSubject { get; set; }

        [Required(ErrorMessage = "Please enter a message")]
        public string EmailBody { get; set; }

        public bool IsKnownEmailAddress { get; set; }

        public bool HasKnownEmailDomain { get; set; }

        public RequestValuationEmail ToRequestValuationEmail()
        {
            return new RequestValuationEmail
            {
                From = this.EmailFrom,
                To = this.EmailTo,
                Subject = this.EmailSubject,
                Message = this.EmailBody,
                IsKnownEmailAddress = this.IsKnownEmailAddress,
            };
        }
    }
}