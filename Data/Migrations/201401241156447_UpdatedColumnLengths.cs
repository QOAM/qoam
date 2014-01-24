namespace RU.Uci.OAMarket.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedColumnLengths : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Journals", "Title", c => c.String(nullable: false, maxLength: 1024));
            AlterColumn("dbo.Countries", "Name", c => c.String(nullable: false, maxLength: 1024));
            AlterColumn("dbo.Publishers", "Name", c => c.String(nullable: false, maxLength: 1024));
            AlterColumn("dbo.UserProfiles", "UserName", c => c.String(nullable: false, maxLength: 1024));
            AlterColumn("dbo.UserProfiles", "DisplayName", c => c.String(nullable: false, maxLength: 1024));
            AlterColumn("dbo.UserProfiles", "Email", c => c.String(maxLength: 1024));
            AlterColumn("dbo.Institutions", "Name", c => c.String(nullable: false, maxLength: 1024));
            AlterColumn("dbo.Institutions", "ShortName", c => c.String(nullable: false, maxLength: 1024));
            AlterColumn("dbo.Languages", "Name", c => c.String(nullable: false, maxLength: 1024));
            AlterColumn("dbo.Subjects", "Name", c => c.String(nullable: false, maxLength: 1024));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Subjects", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Languages", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Institutions", "ShortName", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Institutions", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.UserProfiles", "Email", c => c.String(maxLength: 255));
            AlterColumn("dbo.UserProfiles", "DisplayName", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.UserProfiles", "UserName", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Publishers", "Name", c => c.String(nullable: false, maxLength: 900));
            AlterColumn("dbo.Countries", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Journals", "Title", c => c.String(nullable: false, maxLength: 900));
        }
    }
}
