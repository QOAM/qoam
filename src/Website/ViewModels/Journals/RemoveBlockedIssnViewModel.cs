namespace QOAM.Website.ViewModels.Journals
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core;

    public class RemoveBlockedIssnViewModel
    {
        [Required]
        public IEnumerable<int> SelectedItems { get; set; }

        public IEnumerable<BlockedISSN> BlockedIssns { get; set; }
    }
}