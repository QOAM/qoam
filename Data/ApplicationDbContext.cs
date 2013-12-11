namespace RU.Uci.OAMarket.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;

    using RU.Uci.OAMarket.Domain;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Journal> Journals { get; set; }
        public DbSet<JournalPrice> JournalsPrices { get; set; }
        public DbSet<ScoreCard> ScoreCards { get; set; }
        public DbSet<ScoreCardVersion> ScoreCardVersions { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<InstitutionJournal> InstitutionJournals { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        public ObjectContext ObjectContext
        {
            get
            {
                return ((IObjectContextAdapter)this).ObjectContext;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ComplexType<ScoreCardScore>();

            modelBuilder.Entity<Journal>()
                .HasOptional(t => t.JournalPrice)
                .WithMany(t => t.Journals)
                .HasForeignKey(d => d.JournalPriceId);
            modelBuilder.Entity<Journal>()
                .HasOptional(t => t.JournalScore)
                .WithMany(t => t.Journals)
                .HasForeignKey(d => d.JournalScoreId);
        }
    }
}