namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BFJTrustedJournals : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrustedJournals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateAdded = c.DateTime(nullable: false),
                        InstitutionId = c.Int(nullable: false),
                        JournalId = c.Int(nullable: false),
                        UserProfileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Institutions", t => t.InstitutionId, cascadeDelete: true)
                .ForeignKey("dbo.Journals", t => t.JournalId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfiles", t => t.UserProfileId)
                .Index(t => t.InstitutionId)
                .Index(t => t.JournalId)
                .Index(t => t.UserProfileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrustedJournals", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.TrustedJournals", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.TrustedJournals", "InstitutionId", "dbo.Institutions");
            DropIndex("dbo.TrustedJournals", new[] { "UserProfileId" });
            DropIndex("dbo.TrustedJournals", new[] { "JournalId" });
            DropIndex("dbo.TrustedJournals", new[] { "InstitutionId" });
            DropTable("dbo.TrustedJournals");
        }
    }
}
