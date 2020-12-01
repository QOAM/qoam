using System;
using QOAM.Core;

namespace QOAM.Website.ViewModels.BonaFideJournals
{
    public class SubmitTrustViewModel
    {
        public int JournalId { get; set; }
        public int InstitutionId { get; set; }
        public int UserId { get; set; }

        public TrustedJournal ToTrustedJournal()
        {
            return new TrustedJournal
            {
                DateAdded = DateTime.Now,
                JournalId = JournalId,
                UserProfileId = UserId,
                InstitutionId = InstitutionId
            };
        }
    }
}