using System;
using System.Linq.Expressions;

namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using PagedList;

    using QOAM.Core.Repositories.Filters;

    public interface IInstitutionRepository
    {
        IList<Institution> All { get; }

        Institution Find(int id);
        Institution Find(string shortName);

        void InsertOrUpdate(Institution institution);

        void Save();

        IQueryable<string> Names(string query);

        IPagedList<Institution> Search(InstitutionFilter filter);
        Institution Find(MailAddress mailAddress);
        void Delete(Institution entity);
        bool Exists(string name);
        bool Exists(int id);
        bool DomainExists(string domain);
        Institution FindByExactHost(MailAddress mailAddress);
        IList<Institution> WithLicenses();
        List<Institution> FindWhere(Expression<Func<Institution, bool>> whereClause);
    }
}