namespace RU.Uci.OAMarket.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class InstitutionJournal : Entity
    {
        public DateTime DateAdded { get; set; }

        [Required]
        public string Link { get; set; }

        public int InstitutionId { get; set; }
        public int JournalId { get; set; }
        public int UserProfileId { get; set; }
        public virtual Institution Institution { get; set; }
        public virtual Journal Journal { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}