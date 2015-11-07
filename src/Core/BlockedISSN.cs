namespace QOAM.Core
{
    using System.ComponentModel.DataAnnotations;

    public class BlockedISSN : Entity
    {
        [Required]
        [StringLength(32)]
        public string ISSN { get; set; }
    }
}