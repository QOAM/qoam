namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JournalListPrice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ListPrices",
                c => new
                    {
                        JournalId = c.Int(nullable: false),
                        Link = c.String(),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.JournalId)
                .ForeignKey("dbo.Journals", t => t.JournalId, cascadeDelete: true)
                .Index(t => t.JournalId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ListPrices", "JournalId", "dbo.Journals");
            DropIndex("dbo.ListPrices", new[] { "JournalId" });
            DropTable("dbo.ListPrices");
        }
    }
}
