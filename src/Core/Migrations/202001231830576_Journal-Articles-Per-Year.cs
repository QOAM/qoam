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
                        JournalId = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        NumberOfArticles = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JournalId)
                .ForeignKey("dbo.Journals", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArticlesPerYear", "Id", "dbo.Journals");
            DropIndex("dbo.ArticlesPerYear", new[] { "Id" });
            DropTable("dbo.ArticlesPerYear");
        }
    }
}
