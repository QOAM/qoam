namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;

    public interface ISubjectRepository
    {
        ApplicationDbContext DbContext { get; }
        IList<Subject> All { get; }
        IList<Subject> Active { get; }
        
        void InsertBulk(IEnumerable<Subject> subjects);
    }
}