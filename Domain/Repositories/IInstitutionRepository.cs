namespace RU.Uci.OAMarket.Domain.Repositories
{
    using System.Collections.Generic;

    public interface IInstitutionRepository
    {
        IList<Institution> All { get; }

        Institution Find(int id);
        Institution Find(string shortName);

        void Insert(Institution institution);
    }
}