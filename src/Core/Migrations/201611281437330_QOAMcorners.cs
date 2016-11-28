namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QOAMcorners : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CornerJournals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JournalId = c.Int(nullable: false),
                        CornerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Corners", t => t.CornerId, cascadeDelete: true)
                .ForeignKey("dbo.Journals", t => t.JournalId, cascadeDelete: true)
                .Index(t => t.JournalId)
                .Index(t => t.CornerId);
            
            CreateTable(
                "dbo.Corners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NumberOfVisitors = c.Int(nullable: false),
                        UserProfileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CornerJournals", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.CornerJournals", "CornerId", "dbo.Corners");
            DropForeignKey("dbo.Corners", "UserProfileId", "dbo.UserProfiles");
            DropIndex("dbo.Corners", new[] { "UserProfileId" });
            DropIndex("dbo.CornerJournals", new[] { "CornerId" });
            DropIndex("dbo.CornerJournals", new[] { "JournalId" });
            DropTable("dbo.Corners");
            DropTable("dbo.CornerJournals");
        }
    }
}
