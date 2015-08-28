namespace QOAM.Website.ViewModels.Journals
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using Core;
    using Models;

    public class BlockIssnViewModel
    {
        [Required]
        [Issns]
        [DisplayName(@"ISSN")]
        public string ISSN { get; set; }

        public IEnumerable<BlockedISSN> BlockedIssns { get; set; }
    }
}