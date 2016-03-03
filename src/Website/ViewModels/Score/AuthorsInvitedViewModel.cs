using System.Collections.Generic;

namespace QOAM.Website.ViewModels.Score
{
    public class AuthorsInvitedViewModel
    {
        public int AmountInvited { get; set; }
        public int AmountNotInvited => AuthorsNotInvited.Count;
        public int AmountInvitedWithError => AuthorsInvitedWithError.Count;
        public List<NotInvitedViewModel> AuthorsNotInvited { get; set; }
        public List<ErrorInvitedViewModel> AuthorsInvitedWithError { get; set; }
    }

    public class NotInvitedViewModel
    {
        public string ISSN { get; set; }
        public string JournalTitle { get; set; }
        public string AuthorEmail { get; set; }
        public string AuthorName { get; set; }
    }

    public class ErrorInvitedViewModel
    {
        public string ISSN { get; set; }
        public string JournalTitle { get; set; }
        public string AuthorEmail { get; set; }
        public string AuthorName { get; set; }
    }
}