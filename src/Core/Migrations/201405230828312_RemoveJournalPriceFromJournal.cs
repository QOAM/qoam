namespace QOAM.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class RemoveJournalPriceFromJournal : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Journals", "BaseJournalPriceId", "dbo.BaseJournalPrices");
            DropForeignKey("dbo.Journals", "ValuationJournalPriceId", "dbo.ValuationJournalPrices");
            DropIndex("dbo.Journals", "IX_JournalPriceId");
            DropIndex("dbo.Journals", new[] { "BaseJournalPriceId" });
            DropIndex("dbo.Journals", new[] { "ValuationJournalPriceId" });
            DropColumn("dbo.Journals", "BaseJournalPriceId");
            DropColumn("dbo.Journals", "ValuationJournalPriceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Journals", "ValuationJournalPriceId", c => c.Int());
            AddColumn("dbo.Journals", "BaseJournalPriceId", c => c.Int());
            CreateIndex("dbo.Journals", "ValuationJournalPriceId");
            CreateIndex("dbo.Journals", "BaseJournalPriceId");
            AddForeignKey("dbo.Journals", "ValuationJournalPriceId", "dbo.ValuationJournalPrices", "Id");
            AddForeignKey("dbo.Journals", "BaseJournalPriceId", "dbo.BaseJournalPrices", "Id");
        }
    }
}
