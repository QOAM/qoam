namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewLastUpdatedOncolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Journals", "LastUpdatedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Journals", "LastUpdatedOn");
        }
    }
}
