namespace RU.Uci.OAMarket.Website.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using RU.Uci.OAMarket.Domain.Helpers;

    using WebRoles = System.Web.Security.Roles;

    public class Roles : IRoles
    {
        public IEnumerable<string> GetRolesForUser(string userName)
        {
            return WebRoles.GetRolesForUser(userName).ToSet();
        }

        public void UpdateUserRoles(string username, IEnumerable<string> newRoles)
        {
            var currentRoles = GetRolesForUser(username);
            var rolesToRemove = currentRoles.Except(newRoles);
            var rolesToAdd = newRoles.Except(currentRoles);

            foreach (var roleToAdd in rolesToAdd)
            {
                WebRoles.AddUserToRole(username, roleToAdd);
            }

            foreach (var roleToRemove in rolesToRemove)
            {
                WebRoles.RemoveUserFromRole(username, roleToRemove);
            }
        }
    }
}