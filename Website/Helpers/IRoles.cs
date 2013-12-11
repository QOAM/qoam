namespace RU.Uci.OAMarket.Website.Helpers
{
    using System.Collections.Generic;

    public interface IRoles
    {
        IEnumerable<string> GetRolesForUser(string userName);
        void UpdateUserRoles(string userName, IEnumerable<string> newRoles);
    }
}