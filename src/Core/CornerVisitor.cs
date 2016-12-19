using System;

namespace QOAM.Core
{
    public class CornerVisitor : Entity
    {
        public int CornerId { get; set; }
        public string IpAddress { get; set; }
        public DateTime VisitedOn { get; set; }

        public virtual Corner Corner { get; set; }
    }
}