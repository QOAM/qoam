namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlanSPropertyinJournal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Journals", "PlanS", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Journals", "PlanS");
        }
    }
}
