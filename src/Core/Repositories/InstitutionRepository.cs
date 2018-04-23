namespace QOAM.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using System.Web.Helpers;
    using PagedList;

    using Filters;

    public class InstitutionRepository : Repository<Institution>, IInstitutionRepository
    {
        public InstitutionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IList<Institution> All
        {
            get
            {
                return DbContext.Institutions.OrderBy(i => i.Name).ToList();
            }
        }

        public IQueryable<string> Names(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Enumerable.Empty<string>().AsQueryable();

            return DbContext.Institutions.Where(u => u.Name.ToLower().StartsWith(query.ToLower())).Select(j => j.Name);
        }

        public IPagedList<Institution> Search(InstitutionFilter filter)
        {
            var query = DbContext.Institutions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(u => u.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            return ApplyOrdering(query, filter).ToPagedList(filter.PageNumber, filter.PageSize);
        }

        public Institution Find(string shortName)
        {
            return DbContext.Institutions.FirstOrDefault(i => i.ShortName == shortName);
        }

        public Institution Find(int id)
        {
            return DbContext.Institutions.Find(id);
        }
        
        public Institution Find(MailAddress mailAddress)
        {
            return DbContext.Institutions.FirstOrDefault(i => mailAddress.Host.Equals(i.ShortName, StringComparison.OrdinalIgnoreCase));
        }

        public bool Exists(string name)
        {
            return DbContext.Institutions.Any(i => i.Name == name);
        }

        public bool DomainExists(string domain)
        {
            return DbContext.Institutions.Any(i => i.ShortName == domain);
        }

        public bool Exists(int id)
        {
            return DbContext.Institutions.Any(i => i.Id == id);
        }

        //public override void Delete(Institution entity)
        //{
        //    var userProfiles = entity.UserProfiles.Select(u => u.Id).ToList();

        //    if (userProfiles.Any())
        //    { 
        //        DbContext.BaseJournalPrices
        //            .Where(b => userProfiles.Contains(b.UserProfileId))
        //            .Delete();

        //        DbContext.BaseScoreCards
        //            .Where(b => userProfiles.Contains(b.UserProfileId))
        //            .Delete();

        //        DbContext.ValuationJournalPrices
        //            .Where(p => userProfiles.Contains(p.UserProfileId))
        //            .Delete();

        //        DbContext.ValuationScoreCards
        //            .Where(c => userProfiles.Contains(c.UserProfileId))
        //            .Delete();
        //    }

        //    base.Delete(entity);
        //}

        private static IOrderedQueryable<Institution> ApplyOrdering(IQueryable<Institution> query, InstitutionFilter filter)
        {
            switch (filter.SortMode)
            {
                case InstitutionSortMode.Name:
                    return filter.SortDirection == SortDirection.Ascending ? query.OrderBy(u => u.Name) : query.OrderByDescending(u => u.Name);
                case InstitutionSortMode.NumberOfBaseJournalScoreCards:
                    return filter.SortDirection == SortDirection.Ascending ?
                        query.OrderBy(u => u.NumberOfBaseScoreCards).ThenBy(u => u.Name) :
                        query.OrderByDescending(u => u.NumberOfBaseScoreCards).ThenBy(u => u.Name);
                case InstitutionSortMode.NumberOfValuationJournalScoreCards:
                    return filter.SortDirection == SortDirection.Ascending ?
                        query.OrderBy(u => u.NumberOfValuationScoreCards).ThenBy(u => u.Name) :
                        query.OrderByDescending(u => u.NumberOfValuationScoreCards).ThenBy(u => u.Name);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}