namespace QOAM.Core.Migrations
{
    using System.Data.Entity.Migrations;

    using QOAM.Core.Scripts;

    public partial class ConvertDataAfterSplit : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Journals", "JournalPriceId", "dbo.JournalPrices");
            DropForeignKey("dbo.Journals", "JournalScoreId", "dbo.JournalScores");
            DropForeignKey("dbo.BaseJournalPrices", "ScoreCardId", "dbo.ScoreCards");
            DropForeignKey("dbo.BaseJournalPrices", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.BaseJournalPrices", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.ValuationQuestionScores", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.BaseQuestionScores", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.BaseQuestionScores", "BaseScoreCardId", "dbo.BaseScoreCards");
            DropForeignKey("dbo.BaseScoreCards", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.BaseScoreCards", "ScoreCardVersionId", "dbo.ScoreCardVersions");
            DropForeignKey("dbo.BaseScoreCards", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.InstitutionJournals", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.JournalScores", "JournalId", "dbo.Journals");
            
            Sql(ResourceReader.GetContentsOfResource("Copy after split JSC.sql"));

            AddForeignKey("dbo.ValuationScoreCards", "UserProfileId", "dbo.UserProfiles", "Id");
            AddForeignKey("dbo.ValuationScoreCards", "JournalId", "dbo.Journals", "Id");
            AddForeignKey("dbo.ValuationScoreCards", "VersionId", "dbo.ScoreCardVersions", "Id");
            AddForeignKey("dbo.ValuationQuestionScores", "QuestionId", "dbo.Questions", "Id");
            AddForeignKey("dbo.ValuationQuestionScores", "ValuationScoreCardId", "dbo.ValuationScoreCards", "Id");
            AddForeignKey("dbo.ValuationJournalPrices", "JournalId", "dbo.Journals", "Id");
            AddForeignKey("dbo.ValuationJournalPrices", "UserProfileId", "dbo.UserProfiles", "Id");
            AddForeignKey("dbo.ValuationJournalPrices", "ValuationScoreCardId", "dbo.ValuationScoreCards", "Id");
            AddForeignKey("dbo.BaseJournalPrices", "JournalId", "dbo.Journals", "Id");
            AddForeignKey("dbo.BaseJournalPrices", "UserProfileId", "dbo.UserProfiles", "Id");
            AddForeignKey("dbo.BaseJournalPrices", "BaseScoreCardId", "dbo.BaseScoreCards", "Id");
            AddForeignKey("dbo.Journals", "JournalScoreId", "dbo.JournalScores", "Id");
            AddForeignKey("dbo.BaseQuestionScores", "QuestionId", "dbo.Questions", "Id");
            AddForeignKey("dbo.BaseQuestionScores", "BaseScoreCardId", "dbo.BaseScoreCards", "Id");
            AddForeignKey("dbo.BaseScoreCards", "JournalId", "dbo.Journals", "Id");
            AddForeignKey("dbo.BaseScoreCards", "VersionId", "dbo.ScoreCardVersions", "Id");
            AddForeignKey("dbo.BaseScoreCards", "UserProfileId", "dbo.UserProfiles", "Id");
            AddForeignKey("dbo.InstitutionJournals", "UserProfileId", "dbo.UserProfiles", "Id");
            AddForeignKey("dbo.JournalScores", "JournalId", "dbo.Journals", "Id");
        }
        
        public override void Down()
        {
        }
    }
}
