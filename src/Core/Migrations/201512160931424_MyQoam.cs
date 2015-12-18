namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MyQoam : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserJournals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JournalId = c.Int(nullable: false),
                        UserProfileId = c.Int(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Journals", t => t.JournalId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfiles", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.JournalId)
                .Index(t => t.UserProfileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserJournals", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.UserJournals", "JournalId", "dbo.Journals");
            DropIndex("dbo.UserJournals", new[] { "UserProfileId" });
            DropIndex("dbo.UserJournals", new[] { "JournalId" });
            DropTable("dbo.UserJournals");
        }
    }
}
