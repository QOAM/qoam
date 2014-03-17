namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using PagedList;

    using QOAM.Core.Repositories.Filters;

    public interface IInstitutionRepository
    {
        IList<Institution> All { get; }

        Institution Find(int id);
        Institution Find(string shortName);

        void Insert(Institution institution);

        IQueryable<string> Names(string query);

        IPagedList<Institution> Search(InstitutionFilter filter);
    }
}