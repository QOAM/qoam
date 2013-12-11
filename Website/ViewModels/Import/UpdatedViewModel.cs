namespace RU.Uci.OAMarket.Website.ViewModels.Import
{
    using System.Collections.Generic;

    public class UpdatedViewModel
    {
        public IEnumerable<string> NotFoundISSNs { get; set; }
        public IEnumerable<string> FoundISSNs { get; set; }
    }
}