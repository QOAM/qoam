using System;
using System.Collections.Generic;

namespace QOAM.Core
{
    public class Corner : Entity
    {
        public Corner()
        {
            CornerJournals = new List<CornerJournal>();
        }
        public string Name { get; set; }
        public int NumberOfVisitors { get; set; }
        public int UserProfileId { get; set; }
        public DateTime LastVisitedOn { get; set; }

        public virtual ICollection<CornerJournal> CornerJournals { get; set; }
        public virtual UserProfile CornerAdmin { get; set; }
    }
}