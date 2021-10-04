namespace QOAM.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Journal : Entity
    {
        public Journal()
        {
            InstitutionJournalPrices = new List<InstitutionJournal>();
            TrustingInstitutions = new List<TrustedJournal>();
            BaseJournalPrices = new List<BaseJournalPrice>();
            ValuationJournalPrices = new List<ValuationJournalPrice>();
            Languages = new List<Language>();
            BaseScoreCards = new List<BaseScoreCard>();
            ValuationScoreCards = new List<ValuationScoreCard>();
            Subjects = new List<Subject>();
            OverallScore = new JournalCategoryScore();
            EditorialInformationScore = new JournalCategoryScore();
            PeerReviewScore = new JournalCategoryScore();
            GovernanceScore = new JournalCategoryScore();
            ProcessScore = new JournalCategoryScore();
            ValuationScore = new JournalCategoryScore();
            CornerJournals = new List<CornerJournal>();
            ArticlesPerYear = new List<ArticlesPerYear>();
        }

        [Required]
        [StringLength(1000)]
        public string Title { get; set; }

        [Required]
        [StringLength(32)]
        public string ISSN { get; set; }

        [StringLength(32)]
        public string PISSN { get; set; }

        [Required]
        public string Link { get; set; }
        public string SubmissionPageLink { get; set; }
        
        public DateTime DateAdded { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public int CountryId { get; set; }
        public int PublisherId { get; set; }
        public int JournalScoreId { get; set; }
        public bool DoajSeal { get; set; }
        public JournalCategoryScore OverallScore { get; set; }

        public JournalCategoryScore EditorialInformationScore { get; set; }

        public JournalCategoryScore PeerReviewScore { get; set; }

        public JournalCategoryScore GovernanceScore { get; set; }

        public JournalCategoryScore ProcessScore { get; set; }

        public JournalCategoryScore ValuationScore { get; set; }

        public int NumberOfBaseReviewers { get; set; }

        public int NumberOfValuationReviewers { get; set; }

        [StringLength(50)]
        public string DataSource { get; set; }
        public bool OpenAccess { get; set; }
        public bool NoFee { get; set; }
        public bool InDoaj { get; set; }

        public int NumberOfArticles { get; set; }

        public string Swot
        {
            get
            {
                if (this.OverallScore == null || this.ValuationScore == null ||
                    this.NumberOfBaseReviewers == 0 || this.NumberOfValuationReviewers == 0)
                {
                    return string.Empty;
                }

                if (this.OverallScore.AverageScore >= 3 && this.ValuationScore.AverageScore >= 3)
                {
                    return SwotVerdict.StrongJournal;
                }
                if (this.OverallScore.AverageScore < 3 && this.ValuationScore.AverageScore < 3)
                {
                    return SwotVerdict.WeakerJournal;
                }
                if (this.OverallScore.AverageScore >= 3 && this.ValuationScore.AverageScore < 3)
                {
                    return SwotVerdict.ThreatToAuthor;
                }
                if (this.OverallScore.AverageScore < 3 && this.ValuationScore.AverageScore >= 3)
                {
                    return SwotVerdict.OpportunityToPublisher;
                }

                return string.Empty;
            }
        }

        public virtual Country Country { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual ListPrice ListPrice { get; set; }

        public virtual ICollection<InstitutionJournal> InstitutionJournalPrices { get; set; }
        public virtual ICollection<TrustedJournal> TrustingInstitutions { get; set; }
        public virtual ICollection<BaseJournalPrice> BaseJournalPrices { get; set; }
        public virtual ICollection<ValuationJournalPrice> ValuationJournalPrices { get; set; }
        public virtual ICollection<Language> Languages { get; set; }
        public virtual ICollection<BaseScoreCard> BaseScoreCards { get; set; }
        public virtual ICollection<ValuationScoreCard> ValuationScoreCards { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<CornerJournal> CornerJournals { get; set; }
        public virtual ICollection<ArticlesPerYear> ArticlesPerYear { get; set; }
    }
}