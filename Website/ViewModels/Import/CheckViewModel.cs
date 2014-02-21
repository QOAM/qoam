namespace RU.Uci.OAMarket.Website.ViewModels.Import
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CheckViewModel : PagedViewModel
    {
        [Required]
        public string ISSNs { get; set; }

        public IEnumerable<string> NotFoundISSNs { get; set; }

        public IEnumerable<string> FoundISSNs { get; set; }
    }
}