namespace QOAM.Website.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using QOAM.Core.Helpers;

    public class Roles : IRoles
    {
        public IEnumerable<string> GetRolesForUser(string userName)
        {
            return System.Web.Security.Roles.GetRolesForUser(userName).ToSet();
        }

        public void UpdateUserRoles(string username, IEnumerable<string> newRoles)
        {
            var currentRoles = this.GetRolesForUser(username);
            var rolesToRemove = currentRoles.Except(newRoles);
            var rolesToAdd = newRoles.Except(currentRoles);

            foreach (var roleToAdd in rolesToAdd)
            {
                System.Web.Security.Roles.AddUserToRole(username, roleToAdd);
            }

            foreach (var roleToRemove in rolesToRemove)
            {
                System.Web.Security.Roles.RemoveUserFromRole(username, roleToRemove);
            }
        }
    }
}