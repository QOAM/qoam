namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JournalArticlesPerYear : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticlesPerYear",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JournalId = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        NumberOfArticles = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Journals", t => t.JournalId, cascadeDelete: true)
                .Index(t => t.JournalId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArticlesPerYear", "JournalId", "dbo.Journals");
            DropIndex("dbo.ArticlesPerYear", new[] { "JournalId" });
            DropTable("dbo.ArticlesPerYear");
        }
    }
}
