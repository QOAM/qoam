using System.ComponentModel.DataAnnotations;

namespace QOAM.Website.ViewModels.Score
{
    using PagedList;
    using QOAM.Core.Services;    
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using QOAM.Core;

    public class ReqValuationViewModel
    {
        // Journal
        public int JournalId { get; set; }
        public string JournalTitle { get; set; }
        public string JournalISSN { get; set; }

        // Mail
        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public string EMailFrom { get; set; }
        [Required(ErrorMessage = "Institutional email is Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public string EMailTo { get; set; }
        public string EMailSubject { get; set; }
        [Required(ErrorMessage = "Please enter a message")]
        public string EMailBody { get; set; }

        // model
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
        public bool IsKnowTo { get; set; }
        public bool IsKnowDomain { get; set; }
    }
}