namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using Scripts;

    public partial class JournalScoreSimplification : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JournalScores", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.Journals", "JournalScoreId", "dbo.JournalScores");
            DropIndex("dbo.Journals", new[] { "JournalScoreId" });
            DropIndex("dbo.JournalScores", new[] { "JournalId" });
            AddColumn("dbo.Journals", "OverallScore_AverageScore", c => c.Single(nullable: false));
            AddColumn("dbo.Journals", "OverallScore_TotalScore", c => c.Int(nullable: false));
            AddColumn("dbo.Journals", "EditorialInformationScore_AverageScore", c => c.Single(nullable: false));
            AddColumn("dbo.Journals", "EditorialInformationScore_TotalScore", c => c.Int(nullable: false));
            AddColumn("dbo.Journals", "PeerReviewScore_AverageScore", c => c.Single(nullable: false));
            AddColumn("dbo.Journals", "PeerReviewScore_TotalScore", c => c.Int(nullable: false));
            AddColumn("dbo.Journals", "GovernanceScore_AverageScore", c => c.Single(nullable: false));
            AddColumn("dbo.Journals", "GovernanceScore_TotalScore", c => c.Int(nullable: false));
            AddColumn("dbo.Journals", "ProcessScore_AverageScore", c => c.Single(nullable: false));
            AddColumn("dbo.Journals", "ProcessScore_TotalScore", c => c.Int(nullable: false));
            AddColumn("dbo.Journals", "ValuationScore_AverageScore", c => c.Single(nullable: false));
            AddColumn("dbo.Journals", "ValuationScore_TotalScore", c => c.Int(nullable: false));
            AddColumn("dbo.Journals", "NumberOfBaseReviewers", c => c.Int(nullable: false));
            AddColumn("dbo.Journals", "NumberOfValuationReviewers", c => c.Int(nullable: false));
            AlterColumn("dbo.Journals", "JournalScoreId", c => c.Int(nullable: false));
            
            Sql("UPDATE j SET " +
                  "j.[NumberOfBaseReviewers] = s.[NumberOfBaseReviewers], " +
                  "j.[OverallScore_AverageScore] = s.[OverallScore_AverageScore], " +
                  "j.[OverallScore_TotalScore] = s.[OverallScore_TotalScore], " +
                  "j.[EditorialInformationScore_AverageScore] = s.[EditorialInformationScore_AverageScore], " +
                  "j.[EditorialInformationScore_TotalScore] = s.[EditorialInformationScore_TotalScore], " +
                  "j.[PeerReviewScore_AverageScore] = s.[PeerReviewScore_AverageScore], " +
                  "j.[PeerReviewScore_TotalScore] = s.[PeerReviewScore_TotalScore], " +
                  "j.[GovernanceScore_AverageScore] = s.[GovernanceScore_AverageScore], " +
                  "j.[GovernanceScore_TotalScore] = s.[GovernanceScore_TotalScore], " +
                  "j.[ProcessScore_AverageScore] = s.[ProcessScore_AverageScore], " +
                  "j.[ProcessScore_TotalScore] = s.[ProcessScore_TotalScore], " +
                  "j.[ValuationScore_AverageScore] = s.[ValuationScore_AverageScore], " +
                  "j.[ValuationScore_TotalScore] = s.[ValuationScore_TotalScore], " +
                  "j.[NumberOfValuationReviewers] = s.[NumberOfValuationReviewers] " +
                  "FROM [dbo].[Journals] j " +
                  "INNER JOIN [dbo].[JournalScores] s ON s.[JournalId] = j.[Id] ");

            Sql("DROP TRIGGER [ScoreCards.Published]");
            Sql("DROP TRIGGER [ScoreCards.Deleted]");
            Sql("DROP TRIGGER [ValuationScoreCards.Published]");
            Sql("DROP TRIGGER [ValuationScoreCards.Deleted]");

            Sql(ResourceReader.GetContentsOfResource("20160329_Trigger_BaseScorecards.Modified.sql"));
            Sql(ResourceReader.GetContentsOfResource("20160329_Trigger_ValuationScorecards.Modified.sql"));

            DropTable("dbo.JournalScores");
        }
        
        public override void Down()
        {
            Sql("DROP TRIGGER [BaseScoreCards.Modified]");
            Sql("DROP TRIGGER [ValuationScoreCards.Modified]");

            CreateTable(
                "dbo.JournalScores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OverallScore_AverageScore = c.Single(nullable: false),
                        OverallScore_TotalScore = c.Int(nullable: false),
                        EditorialInformationScore_AverageScore = c.Single(nullable: false),
                        EditorialInformationScore_TotalScore = c.Int(nullable: false),
                        PeerReviewScore_AverageScore = c.Single(nullable: false),
                        PeerReviewScore_TotalScore = c.Int(nullable: false),
                        GovernanceScore_AverageScore = c.Single(nullable: false),
                        GovernanceScore_TotalScore = c.Int(nullable: false),
                        ProcessScore_AverageScore = c.Single(nullable: false),
                        ProcessScore_TotalScore = c.Int(nullable: false),
                        ValuationScore_AverageScore = c.Single(nullable: false),
                        ValuationScore_TotalScore = c.Int(nullable: false),
                        NumberOfBaseReviewers = c.Int(nullable: false),
                        NumberOfValuationReviewers = c.Int(nullable: false),
                        JournalId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            AlterColumn("dbo.Journals", "JournalScoreId", c => c.Int());

            Sql("INSERT INTO [dbo].[JournalScores] (" +
                    "[NumberOfBaseReviewers], " +
                    "[OverallScore_AverageScore], " +
                    "[OverallScore_TotalScore], " +
                    "[EditorialInformationScore_AverageScore], " +
                    "[EditorialInformationScore_TotalScore], " +
                    "[PeerReviewScore_AverageScore], " +
                    "[PeerReviewScore_TotalScore], " +
                    "[GovernanceScore_AverageScore], " +
                    "[GovernanceScore_TotalScore], " +
                    "[ProcessScore_AverageScore], " +
                    "[ProcessScore_TotalScore], " +
                    "[ValuationScore_AverageScore], " +
                    "[ValuationScore_TotalScore], " +
                    "[NumberOfValuationReviewers], " +
                    "[JournalId]) " +
                "SELECT " +
                    "j.[NumberOfBaseReviewers], " +
                    "j.[OverallScore_AverageScore], " +
                    "j.[OverallScore_TotalScore], " +
                    "j.[EditorialInformationScore_AverageScore], " +
                    "j.[EditorialInformationScore_TotalScore], " +
                    "j.[PeerReviewScore_AverageScore], " +
                    "j.[PeerReviewScore_TotalScore], " +
                    "j.[GovernanceScore_AverageScore], " +
                    "j.[GovernanceScore_TotalScore], " +
                    "j.[ProcessScore_AverageScore], " +
                    "j.[ProcessScore_TotalScore], " +
                    "j.[ValuationScore_AverageScore], " +
                    "j.[ValuationScore_TotalScore], " +
                    "j.[NumberOfValuationReviewers], " +
                    "j.[Id] " +
                "FROM [dbo].[Journals] j");

            Sql("UPDATE [dbo].[Journals] SET [JournalScoreId] = (SELECT TOP 1 [Id] FROM [dbo].[JournalScores] WHERE [JournalId] = [dbo].[Journals].[Id])");
            
            DropColumn("dbo.Journals", "NumberOfValuationReviewers");
            DropColumn("dbo.Journals", "NumberOfBaseReviewers");
            DropColumn("dbo.Journals", "ValuationScore_TotalScore");
            DropColumn("dbo.Journals", "ValuationScore_AverageScore");
            DropColumn("dbo.Journals", "ProcessScore_TotalScore");
            DropColumn("dbo.Journals", "ProcessScore_AverageScore");
            DropColumn("dbo.Journals", "GovernanceScore_TotalScore");
            DropColumn("dbo.Journals", "GovernanceScore_AverageScore");
            DropColumn("dbo.Journals", "PeerReviewScore_TotalScore");
            DropColumn("dbo.Journals", "PeerReviewScore_AverageScore");
            DropColumn("dbo.Journals", "EditorialInformationScore_TotalScore");
            DropColumn("dbo.Journals", "EditorialInformationScore_AverageScore");
            DropColumn("dbo.Journals", "OverallScore_TotalScore");
            DropColumn("dbo.Journals", "OverallScore_AverageScore");
            CreateIndex("dbo.JournalScores", "JournalId");
            CreateIndex("dbo.Journals", "JournalScoreId");
            
            AddForeignKey("dbo.Journals", "JournalScoreId", "dbo.JournalScores", "Id");
            AddForeignKey("dbo.JournalScores", "JournalId", "dbo.Journals", "Id", cascadeDelete: true);

            Sql(ResourceReader.GetContentsOfResource("20151026_Trigger_BaseScorecards.Published.sql"));
            Sql(ResourceReader.GetContentsOfResource("20151027_Trigger_BaseScorecards.Deleted.sql"));

            Sql(ResourceReader.GetContentsOfResource("20151026_Trigger_ValuationScorecards.Published.sql"));
            Sql(ResourceReader.GetContentsOfResource("20151027_Trigger_ValuationScorecards.Deleted.sql"));
        }
    }
}
