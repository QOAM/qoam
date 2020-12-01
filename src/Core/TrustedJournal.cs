using System;

namespace QOAM.Core
{
    public class TrustedJournal : Entity
    {
        public DateTime DateAdded { get; set; }
        public int InstitutionId { get; set; }
        public int JournalId { get; set; }
        public int UserProfileId { get; set; }
        public virtual Institution Institution { get; set; }
        public virtual Journal Journal { get; set; }
        public virtual UserProfile UserProfile { get; set; }

        #region Equality Members

        protected bool Equals(TrustedJournal other)
        {
            return InstitutionId == other.InstitutionId && JournalId == other.JournalId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TrustedJournal) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (InstitutionId * 397) ^ JournalId;
            }
        }

        #endregion
    }
}