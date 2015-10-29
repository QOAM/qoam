namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;

    public interface IPublisherRepository
    {
        IList<Publisher> All { get; }

        void InsertBulk(IEnumerable<Publisher> publishers);
    }
}