namespace QOAM.Core.Import
{
    using System.Collections.Generic;

    using Validation;

    public class JournalIssnEqualityComparer : IEqualityComparer<Journal>
    {
        public bool Equals(Journal x, Journal y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return string.Equals(x.ISSN, y.ISSN);
        }

        public int GetHashCode(Journal obj)
        {
            Requires.NotNull(obj, nameof(obj));

            return obj.ISSN.GetHashCode();
        }
    }
}