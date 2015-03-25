namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CascadeOnDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JournalScores", "JournalId", "dbo.Journals");
            DropIndex("dbo.JournalScores", new[] { "JournalId" });
            AlterColumn("dbo.JournalScores", "JournalId", c => c.Int(nullable: false));
            CreateIndex("dbo.JournalScores", "JournalId");
            AddForeignKey("dbo.JournalScores", "JournalId", "dbo.Journals", "Id", cascadeDelete: true);

            DropForeignKey("dbo.BaseJournalPrices", "JournalId", "dbo.Journals");
            DropIndex("dbo.BaseJournalPrices", new[] { "JournalId" });
            DropIndex("dbo.BaseJournalPrices", new[] { "JournalId", "UserProfileId" });
            AlterColumn("dbo.BaseJournalPrices", "JournalId", c => c.Int(nullable: false));
            CreateIndex("dbo.BaseJournalPrices", "JournalId");
            CreateIndex("dbo.BaseJournalPrices", new[] { "JournalId", "UserProfileId" });
            AddForeignKey("dbo.BaseJournalPrices", "JournalId", "dbo.Journals", "Id", cascadeDelete: true);

            DropForeignKey("dbo.BaseScoreCards", "JournalId", "dbo.Journals");
            DropIndex("dbo.BaseScoreCards", new[] { "JournalId" });
            DropIndex("dbo.BaseScoreCards", new[] { "JournalId", "UserProfileId" });
            DropIndex("dbo.BaseScoreCards", new[] { "JournalId", "State" });
            DropIndex("dbo.BaseScoreCards", new[] { "UserProfileId", "JournalId", "VersionId" });
            AlterColumn("dbo.BaseScoreCards", "JournalId", c => c.Int(nullable: false));
            CreateIndex("dbo.BaseScoreCards", "JournalId");
            CreateIndex("dbo.BaseScoreCards", new[] { "JournalId", "UserProfileId" });
            CreateIndex("dbo.BaseScoreCards", new[] { "JournalId", "State" });
            CreateIndex("dbo.BaseScoreCards", new[] { "UserProfileId", "JournalId", "VersionId" });
            AddForeignKey("dbo.BaseScoreCards", "JournalId", "dbo.Journals", "Id", cascadeDelete: true);

            DropForeignKey("dbo.BaseQuestionScores", "BaseScoreCardId", "dbo.BaseScoreCards");
            DropIndex("dbo.BaseQuestionScores", new[] { "BaseScoreCardId" });
            DropIndex("dbo.BaseQuestionScores", new[] { "BaseScoreCardId", "QuestionId" });
            AlterColumn("dbo.BaseQuestionScores", "BaseScoreCardId", c => c.Int(nullable: false));
            CreateIndex("dbo.BaseQuestionScores", "BaseScoreCardId");
            CreateIndex("dbo.BaseQuestionScores", new[] { "BaseScoreCardId", "QuestionId" });
            AddForeignKey("dbo.BaseQuestionScores", "BaseScoreCardId", "dbo.BaseScoreCards", "Id", cascadeDelete: true);

            DropForeignKey("dbo.ValuationJournalPrices", "JournalId", "dbo.Journals");
            DropIndex("dbo.ValuationJournalPrices", new[] { "JournalId" });
            AlterColumn("dbo.ValuationJournalPrices", "JournalId", c => c.Int(nullable: false));
            CreateIndex("dbo.ValuationJournalPrices", "JournalId");
            AddForeignKey("dbo.ValuationJournalPrices", "JournalId", "dbo.Journals", "Id", cascadeDelete: true);

            DropForeignKey("dbo.ValuationScoreCards", "JournalId", "dbo.Journals");
            DropIndex("dbo.ValuationScoreCards", new[] { "JournalId" });
            AlterColumn("dbo.ValuationScoreCards", "JournalId", c => c.Int(nullable: false));
            CreateIndex("dbo.ValuationScoreCards", "JournalId");
            AddForeignKey("dbo.ValuationScoreCards", "JournalId", "dbo.Journals", "Id", cascadeDelete: true);

            DropForeignKey("dbo.ValuationQuestionScores", "ValuationScoreCardId", "dbo.ValuationScoreCards");
            DropIndex("dbo.ValuationQuestionScores", new[] { "ValuationScoreCardId" });
            AlterColumn("dbo.ValuationQuestionScores", "ValuationScoreCardId", c => c.Int(nullable: false));
            CreateIndex("dbo.ValuationQuestionScores", "ValuationScoreCardId");
            AddForeignKey("dbo.ValuationQuestionScores", "ValuationScoreCardId", "dbo.ValuationScoreCards", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JournalScores", "JournalId", "dbo.Journals");
            DropIndex("dbo.JournalScores", new[] { "JournalId" });
            AlterColumn("dbo.JournalScores", "JournalId", c => c.Int());
            CreateIndex("dbo.JournalScores", "JournalId");
            AddForeignKey("dbo.JournalScores", "JournalId", "dbo.Journals", "Id");

            DropForeignKey("dbo.BaseJournalPrices", "JournalId", "dbo.Journals");
            DropIndex("dbo.BaseJournalPrices", new[] { "JournalId" });
            DropIndex("dbo.BaseJournalPrices", new[] { "JournalId", "UserProfileId" });
            AlterColumn("dbo.BaseJournalPrices", "JournalId", c => c.Int());
            CreateIndex("dbo.BaseJournalPrices", "JournalId");
            CreateIndex("dbo.BaseJournalPrices", new[] { "JournalId", "UserProfileId" });
            AddForeignKey("dbo.BaseJournalPrices", "JournalId", "dbo.Journals", "Id");

            DropForeignKey("dbo.BaseScoreCards", "JournalId", "dbo.Journals");
            DropIndex("dbo.BaseScoreCards", new[] { "JournalId" });
            DropIndex("dbo.BaseScoreCards", new[] { "JournalId", "UserProfileId" });
            DropIndex("dbo.BaseScoreCards", new[] { "JournalId", "State" });
            DropIndex("dbo.BaseScoreCards", new[] { "UserProfileId", "JournalId", "VersionId" });
            AlterColumn("dbo.BaseScoreCards", "JournalId", c => c.Int());
            CreateIndex("dbo.BaseScoreCards", "JournalId");
            CreateIndex("dbo.BaseScoreCards", new[] { "JournalId", "UserProfileId" });
            CreateIndex("dbo.BaseScoreCards", new[] { "JournalId", "State" });
            CreateIndex("dbo.BaseScoreCards", new[] { "UserProfileId", "JournalId", "VersionId" });
            AddForeignKey("dbo.BaseScoreCards", "JournalId", "dbo.Journals", "Id");

            DropForeignKey("dbo.BaseQuestionScores", "BaseScoreCardId", "dbo.BaseScoreCards");
            DropIndex("dbo.BaseQuestionScores", new[] { "BaseScoreCardId" });
            DropIndex("dbo.BaseQuestionScores", new[] { "BaseScoreCardId", "QuestionId" });
            AlterColumn("dbo.BaseQuestionScores", "BaseScoreCardId", c => c.Int());
            CreateIndex("dbo.BaseQuestionScores", "BaseScoreCardId");
            CreateIndex("dbo.BaseQuestionScores", new[] { "BaseScoreCardId", "QuestionId" });
            AddForeignKey("dbo.BaseQuestionScores", "BaseScoreCardId", "dbo.BaseScoreCards", "Id");

            DropForeignKey("dbo.ValuationJournalPrices", "JournalId", "dbo.Journals");
            DropIndex("dbo.ValuationJournalPrices", new[] { "JournalId" });
            AlterColumn("dbo.ValuationJournalPrices", "JournalId", c => c.Int());
            CreateIndex("dbo.ValuationJournalPrices", "JournalId");
            AddForeignKey("dbo.ValuationJournalPrices", "JournalId", "dbo.Journals", "Id");

            DropForeignKey("dbo.ValuationScoreCards", "JournalId", "dbo.Journals");
            DropIndex("dbo.ValuationScoreCards", new[] { "JournalId" });
            AlterColumn("dbo.ValuationScoreCards", "JournalId", c => c.Int());
            CreateIndex("dbo.ValuationScoreCards", "JournalId");
            AddForeignKey("dbo.ValuationScoreCards", "JournalId", "dbo.Journals", "Id");

            DropForeignKey("dbo.ValuationQuestionScores", "ValuationScoreCardId", "dbo.ValuationScoreCards");
            DropIndex("dbo.ValuationQuestionScores", new[] { "ValuationScoreCardId" });
            AlterColumn("dbo.ValuationQuestionScores", "ValuationScoreCardId", c => c.Int());
            CreateIndex("dbo.ValuationQuestionScores", "ValuationScoreCardId");
            AddForeignKey("dbo.ValuationQuestionScores", "ValuationScoreCardId", "dbo.ValuationScoreCards", "Id");
        }
    }
}
