namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JournalDataSource : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Journals", "DataSource", c => c.String(maxLength: 50));
            AddColumn("dbo.Journals", "OpenAccess", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Journals", "OpenAccess");
            DropColumn("dbo.Journals", "DataSource");
        }
    }
}
