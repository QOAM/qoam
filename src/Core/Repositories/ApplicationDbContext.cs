namespace QOAM.Core.Repositories
{
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;

    using QOAM.Core.Migrations;

    public class ApplicationDbContext : DbContext
    {
        static ApplicationDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, ApplicationDbContextMigrationsConfiguration>());
        }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Journal> Journals { get; set; }
        public DbSet<BaseJournalPrice> BaseJournalPrices { get; set; }
        public DbSet<ListPrice> ListPrices { get; set; }
        public DbSet<BaseScoreCard> BaseScoreCards { get; set; }
        public DbSet<ValuationScoreCard> ValuationScoreCards { get; set; }
        public DbSet<ValuationJournalPrice> ValuationJournalPrices { get; set; }
        public DbSet<ScoreCardVersion> ScoreCardVersions { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<InstitutionJournal> InstitutionJournals { get; set; }
        public DbSet<UserJournal> UserJournals { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<BlockedISSN> BlockedISSNs { get; set; }
        public DbSet<Corner> Corners { get; set; }
        public DbSet<CornerJournal> CornerJournals { get; set; }
        public DbSet<CornerVisitor> CornerVisitors { get; set; }

        public ObjectContext ObjectContext => ((IObjectContextAdapter)this).ObjectContext;

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ComplexType<BaseScoreCardScore>();
            modelBuilder.ComplexType<ValuationScoreCardScore>();

            modelBuilder.Entity<BaseScoreCard>().HasRequired(s => s.UserProfile).WithMany(u => u.BaseScoreCards).WillCascadeOnDelete(true);
            modelBuilder.Entity<BaseScoreCard>().HasRequired(s => s.Journal).WithMany(u => u.BaseScoreCards).WillCascadeOnDelete(true);
            modelBuilder.Entity<BaseJournalPrice>().HasRequired(s => s.UserProfile).WithMany(u => u.JournalPrices).WillCascadeOnDelete(true);
            modelBuilder.Entity<BaseJournalPrice>().HasRequired(s => s.Journal).WithMany(u => u.BaseJournalPrices).WillCascadeOnDelete(true);
            modelBuilder.Entity<ValuationScoreCard>().HasRequired(s => s.UserProfile).WithMany(u => u.ValuationScoreCards).WillCascadeOnDelete(true);
            modelBuilder.Entity<ValuationScoreCard>().HasRequired(s => s.Journal).WithMany(u => u.ValuationScoreCards).WillCascadeOnDelete(true);
            modelBuilder.Entity<InstitutionJournal>().HasRequired(s => s.UserProfile).WithMany(u => u.InstitutionJournalPrices).WillCascadeOnDelete(true);
            modelBuilder.Entity<InstitutionJournal>().HasRequired(s => s.Journal).WithMany(u => u.InstitutionJournalPrices).WillCascadeOnDelete(true);
            modelBuilder.Entity<CornerJournal>().HasRequired(s => s.Journal).WithMany(u => u.CornerJournals).WillCascadeOnDelete(true);
            modelBuilder.Entity<CornerJournal>().HasRequired(s => s.Corner).WithMany(u => u.CornerJournals).WillCascadeOnDelete(true);
            modelBuilder.Entity<CornerVisitor>().HasRequired(s => s.Corner).WithMany(u => u.CornerVisitors).WillCascadeOnDelete(true);
            modelBuilder.Entity<Corner>().HasRequired(s => s.CornerAdmin).WithMany(u => u.Corners).WillCascadeOnDelete(true);
            modelBuilder.Entity<ListPrice>().HasKey(lp => lp.JournalId).HasRequired(lp => lp.Journal).WithOptional(j => j.ListPrice).WillCascadeOnDelete(true);
        }
    }
}