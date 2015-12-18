using System;

namespace QOAM.Core
{
    public class UserJournal : Entity
    {
        public int JournalId { get; set; }
        public int UserProfileId { get; set; }
        public virtual Journal Journal { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public DateTime DateAdded { get; set; }
    }
}