namespace RU.Uci.OAMarket.Domain.Repositories
{
    using System.Collections.Generic;

    public interface ICountryRepository
    {
        IList<Country> All { get; }
        
        void InsertBulk(IEnumerable<Country> newCountries);
    }
}