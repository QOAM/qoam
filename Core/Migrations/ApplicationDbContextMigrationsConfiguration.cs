namespace QOAM.Core.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    using QOAM.Core.Repositories;

    internal sealed class ApplicationDbContextMigrationsConfiguration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public ApplicationDbContextMigrationsConfiguration()
        {
            this.AutomaticMigrationsEnabled = false;
            this.AutomaticMigrationDataLossAllowed = true;
            this.MigrationsNamespace = typeof(ApplicationDbContextMigrationsConfiguration).Namespace;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            // Ensure that there always is a scorecard version one instance
            var firstScoreCardVersion = context.ScoreCardVersions.FirstOrDefault(s => s.Number == 1) ?? GetScoreCardVersionOne();

            context.ScoreCardVersions.AddOrUpdate(firstScoreCardVersion);
        }

        private static ScoreCardVersion GetScoreCardVersionOne()
        {
            return new ScoreCardVersion
                   {
                       Number = 1, 
                       OverallNumberOfQuestions = 19,
                       EditorialInformationNumberOfQuestions = 4,
                       PeerReviewNumberOfQuestions = 4,
                       GovernanceNumberOfQuestions = 4,
                       ProcessNumberOfQuestions = 4,
                       ValuationNumberOfQuestions = 3
                   };
        }
    }
}