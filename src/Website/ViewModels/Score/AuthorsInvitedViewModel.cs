using System.Collections.Generic;
using QOAM.Core.Import.Invitations;

namespace QOAM.Website.ViewModels.Score
{
    public class AuthorsInvitedViewModel
    {
        public int AmountInvited { get; set; }
        public int AmountNotInvited => AuthorsNotInvited.Count;
        public List<NotInvitedViewModel> AuthorsNotInvited { get; set; }
    }

    public class NotInvitedViewModel
    {
        public string ISSN { get; set; }
        public string JournalTitle { get; set; }
        public string AuthorEmail { get; set; }
        public string AuthorName { get; set; }
    }
}