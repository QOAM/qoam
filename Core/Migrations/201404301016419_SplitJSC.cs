namespace QOAM.Core.Migrations
{
    using System.Data.Entity.Migrations;

    using QOAM.Core.Scripts;

    public partial class SplitJSC : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JournalPrices", "ScoreCardId", "dbo.ScoreCards");
            DropForeignKey("dbo.JournalPrices", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.JournalPrices", "Id", "dbo.Journals");
            DropForeignKey("dbo.QuestionScores", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.QuestionScores", "ScoreCardId", "dbo.ScoreCards");
            DropForeignKey("dbo.ScoreCards", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.ScoreCards", "VersionId", "dbo.ScoreCardVersions");
            DropForeignKey("dbo.ScoreCards", "UserProfileId", "dbo.UserProfiles");

            RenameTable(name: "dbo.ScoreCards", newName: "BaseScoreCards");
            RenameTable(name: "dbo.JournalPrices", newName: "BaseJournalPrices");
            RenameTable(name: "dbo.QuestionScores", newName: "BaseQuestionScores");
            RenameColumn(table: "dbo.BaseQuestionScores", name: "ScoreCardId", newName: "BaseScoreCardId");
            RenameColumn(table: "dbo.BaseJournalPrices", name: "ScoreCardId", newName: "BaseScoreCardId");
            RenameColumn(table: "dbo.Journals", name: "JournalPriceId", newName: "BaseJournalPriceId");
            //RenameIndex(table: "dbo.Journals", name: "IX_JournalPriceId", newName: "IX_BaseJournalPriceId");
            //RenameIndex(table: "dbo.BaseJournalPrices", name: "IX_ScoreCardId", newName: "IX_BaseScoreCardId");
            //RenameIndex(table: "dbo.BaseQuestionScores", name: "IX_ScoreCardId", newName: "IX_BaseScoreCardId");
            CreateTable(
                "dbo.ValuationScoreCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateStarted = c.DateTime(nullable: false),
                        DateExpiration = c.DateTime(),
                        DatePublished = c.DateTime(),
                        Remarks = c.String(),
                        PriceRemarks = c.String(),
                        UserProfileId = c.Int(nullable: false),
                        JournalId = c.Int(nullable: false),
                        VersionId = c.Int(nullable: false),
                        Score_ValuationScore_AverageScore = c.Single(),
                        Score_ValuationScore_TotalScore = c.Int(),
                        State = c.Int(nullable: false),
                        Submitted = c.Boolean(nullable: false),
                        Editor = c.Boolean(nullable: false),
                        BaseScoreCardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserProfileId)
                .Index(t => t.JournalId)
                .Index(t => t.VersionId);
            
            CreateTable(
                "dbo.ValuationQuestionScores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Score = c.Int(nullable: false),
                        ValuationScoreCardId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.ValuationScoreCardId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.ValuationJournalPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateAdded = c.DateTime(nullable: false),
                        Price_Amount = c.Decimal(precision: 18, scale: 2),
                        Price_Currency = c.Int(),
                        Price_FeeType = c.Int(),
                        JournalId = c.Int(nullable: false),
                        ValuationScoreCardId = c.Int(nullable: false),
                        UserProfileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.JournalId)
                .Index(t => t.ValuationScoreCardId)
                .Index(t => t.UserProfileId);
            
            AddColumn("dbo.Journals", "ValuationJournalPriceId", c => c.Int());
            RenameColumn(table: "dbo.JournalScores", name: "NumberOfReviewers", newName: "NumberOfBaseReviewers");
            AddColumn("dbo.JournalScores", "NumberOfValuationReviewers", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.UserProfiles", name: "NumberOfScoreCards", newName: "NumberOfBaseScoreCards");
            AddColumn("dbo.UserProfiles", "NumberOfValuationScoreCards", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Institutions", name: "NumberOfScoreCards", newName: "NumberOfBaseScoreCards");
            AddColumn("dbo.Institutions", "NumberOfValuationScoreCards", c => c.Int(nullable: false));
            CreateIndex("dbo.Journals", "ValuationJournalPriceId");

            AlterColumn("dbo.Institutions", "NumberOfBaseScoreCards", c => c.Int(nullable: false, defaultValue: 0));
            AlterColumn("dbo.Institutions", "NumberOfValuationScoreCards", c => c.Int(nullable: false, defaultValue: 0));
            AlterColumn("dbo.UserProfiles", "NumberOfBaseScoreCards", c => c.Int(nullable: false, defaultValue: 0));
            AlterColumn("dbo.UserProfiles", "NumberOfValuationScoreCards", c => c.Int(nullable: false, defaultValue: 0));
            AlterColumn("dbo.JournalScores", "NumberOfBaseReviewers", c => c.Int(nullable: false, defaultValue: 0));
            AlterColumn("dbo.JournalScores", "NumberOfValuationReviewers", c => c.Int(nullable: false, defaultValue: 0));
            AlterColumn("dbo.JournalScores", "NumberOfBaseReviewers", c => c.Int(nullable: false, defaultValue: 0));
            AlterColumn("dbo.JournalScores", "NumberOfValuationReviewers", c => c.Int(nullable: false, defaultValue: 0));

            AlterColumn("dbo.JournalScores", "OverallScore_AverageScore", c => c.Single(nullable: false, defaultValue: 0));
            AlterColumn("dbo.JournalScores", "EditorialInformationScore_AverageScore", c => c.Single(nullable: false, defaultValue: 0));
            AlterColumn("dbo.JournalScores", "PeerReviewScore_AverageScore", c => c.Single(nullable: false, defaultValue: 0));
            AlterColumn("dbo.JournalScores", "GovernanceScore_AverageScore", c => c.Single(nullable: false, defaultValue: 0));
            AlterColumn("dbo.JournalScores", "ProcessScore_AverageScore", c => c.Single(nullable: false, defaultValue: 0));
            AlterColumn("dbo.JournalScores", "ValuationScore_AverageScore", c => c.Single(nullable: false, defaultValue: 0));

            AlterColumn("dbo.JournalScores", "OverallScore_TotalScore", c => c.Int(nullable: false, defaultValue: 0));
            AlterColumn("dbo.JournalScores", "EditorialInformationScore_TotalScore", c => c.Int(nullable: false, defaultValue: 0));
            AlterColumn("dbo.JournalScores", "PeerReviewScore_TotalScore", c => c.Int(nullable: false, defaultValue: 0));
            AlterColumn("dbo.JournalScores", "GovernanceScore_TotalScore", c => c.Int(nullable: false, defaultValue: 0));
            AlterColumn("dbo.JournalScores", "ProcessScore_TotalScore", c => c.Int(nullable: false, defaultValue: 0));
            AlterColumn("dbo.JournalScores", "ValuationScore_TotalScore", c => c.Int(nullable: false, defaultValue: 0));
            
            AlterColumn("dbo.BaseJournalPrices", "Price_FeeType", c => c.Int(nullable: true));

            Sql(ResourceReader.GetContentsOfResource("20140409_Trigger_BaseScorecards.Published.sql"));
            Sql(ResourceReader.GetContentsOfResource("20140409_Trigger_ValuationScorecards.Published.sql"));
        }
        
        public override void Down()
        {
            AddColumn("dbo.JournalScores", "NumberOfReviewers", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfiles", "NumberOfScoreCards", c => c.Int(nullable: false));
            AddColumn("dbo.Institutions", "NumberOfScoreCards", c => c.Int(nullable: false));
            DropForeignKey("dbo.Journals", "ValuationJournalPriceId", "dbo.ValuationJournalPrices");
            DropForeignKey("dbo.ValuationJournalPrices", "ValuationScoreCardId", "dbo.ValuationScoreCards");
            DropForeignKey("dbo.ValuationJournalPrices", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.ValuationJournalPrices", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.ValuationQuestionScores", "QuestionId", "dbo.Questions");
            DropIndex("dbo.ValuationJournalPrices", new[] { "UserProfileId" });
            DropIndex("dbo.ValuationJournalPrices", new[] { "ValuationScoreCardId" });
            DropIndex("dbo.ValuationJournalPrices", new[] { "JournalId" });
            DropIndex("dbo.ValuationQuestionScores", new[] { "QuestionId" });
            DropIndex("dbo.ValuationQuestionScores", new[] { "ValuationScoreCardId" });
            DropIndex("dbo.ValuationScoreCards", new[] { "VersionId" });
            DropIndex("dbo.ValuationScoreCards", new[] { "JournalId" });
            DropIndex("dbo.ValuationScoreCards", new[] { "UserProfileId" });
            DropIndex("dbo.Journals", new[] { "ValuationJournalPriceId" });
            DropColumn("dbo.JournalScores", "NumberOfValuationReviewers");
            DropColumn("dbo.JournalScores", "NumberOfBaseReviewers");
            DropColumn("dbo.UserProfiles", "NumberOfValuationScoreCards");
            DropColumn("dbo.UserProfiles", "NumberOfBaseScoreCards");
            DropColumn("dbo.Institutions", "NumberOfValuationScoreCards");
            DropColumn("dbo.Institutions", "NumberOfBaseScoreCards");
            DropColumn("dbo.Journals", "ValuationJournalPriceId");
            DropTable("dbo.ValuationJournalPrices");
            DropTable("dbo.ValuationQuestionScores");
            DropTable("dbo.ValuationScoreCards");
            RenameIndex(table: "dbo.BaseQuestionScores", name: "IX_BaseScoreCardId", newName: "IX_ScoreCardId");
            RenameIndex(table: "dbo.BaseJournalPrices", name: "IX_BaseScoreCardId", newName: "IX_ScoreCardId");
            RenameIndex(table: "dbo.Journals", name: "IX_BaseJournalPriceId", newName: "IX_JournalPriceId");
            RenameColumn(table: "dbo.Journals", name: "BaseJournalPriceId", newName: "JournalPriceId");
            RenameColumn(table: "dbo.BaseJournalPrices", name: "BaseScoreCardId", newName: "ScoreCardId");
            RenameColumn(table: "dbo.ValuationQuestionScores", name: "ValuationScoreCardId", newName: "ScoreCardId");
            RenameColumn(table: "dbo.BaseQuestionScores", name: "BaseScoreCardId", newName: "ScoreCardId");
            RenameTable(name: "dbo.BaseQuestionScores", newName: "QuestionScores");
            RenameTable(name: "dbo.BaseJournalPrices", newName: "JournalPrices");
            RenameTable(name: "dbo.BaseScoreCards", newName: "ScoreCards");
        }
    }
}
