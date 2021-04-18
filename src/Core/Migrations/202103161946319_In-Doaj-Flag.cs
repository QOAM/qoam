namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InDoajFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Journals", "InDoaj", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Journals", "InDoaj");
        }
    }
}
