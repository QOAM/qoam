namespace QOAM.Core
{
    public class CornerJournal : Entity
    {
        public int JournalId { get; set; }
        public int CornerId { get; set; }

        public virtual Journal Journal { get; set; }
        public virtual Corner Corner { get; set; }

        #region Equality Members

        protected bool Equals(CornerJournal other)
        {
            return JournalId == other.JournalId && CornerId == other.CornerId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((CornerJournal) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (JournalId * 397) ^ CornerId;
            }
        }

        #endregion
    }
}