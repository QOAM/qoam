namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubmissionPageLink : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Journals", "SubmissionPageLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Journals", "SubmissionPageLink");
        }
    }
}
