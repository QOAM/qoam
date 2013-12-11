namespace RU.Uci.OAMarket.Website.ViewModels.Profiles
{
    using System.Collections.Generic;

    using RU.Uci.OAMarket.Domain;
    using RU.Uci.OAMarket.Website.Models;

    public class EditViewModel
    {
        public UserProfile UserProfile { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsInstitutionAdmin { get; set; }
        public bool IsDataAdmin { get; set; }
        public string ReturnUrl { get; set; }

        public IEnumerable<string> Roles
        {
            get
            {
                if (this.IsAdmin)
                {
                    yield return ApplicationRole.Admin;
                }

                if (this.IsInstitutionAdmin)
                {
                    yield return ApplicationRole.InstitutionAdmin;
                }

                if (this.IsDataAdmin)
                {
                    yield return ApplicationRole.DataAdmin;
                }
            }
        }
    }
}