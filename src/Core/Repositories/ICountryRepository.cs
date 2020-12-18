namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;

    public interface ICountryRepository
    {
        ApplicationDbContext DbContext { get; }
        IList<Country> All { get; }
        
        void InsertBulk(IEnumerable<Country> newCountries);
        Country FindByName(string name);
    }
}