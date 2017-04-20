namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedpIssntoJournal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Journals", "PISSN", c => c.String(maxLength: 32));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Journals", "PISSN");
        }
    }
}
