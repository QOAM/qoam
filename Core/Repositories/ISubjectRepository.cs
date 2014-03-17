namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;

    public interface ISubjectRepository
    {
        IList<Subject> All { get; }
        
        void InsertBulk(IEnumerable<Subject> subjects);
    }
}