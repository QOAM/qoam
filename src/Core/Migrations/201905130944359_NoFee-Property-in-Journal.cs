namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoFeePropertyinJournal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Journals", "NoFee", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Journals", "NoFee");
        }
    }
}
