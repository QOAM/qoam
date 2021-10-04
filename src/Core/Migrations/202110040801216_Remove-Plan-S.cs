namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePlanS : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Journals", "PlanS");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Journals", "PlanS", c => c.Boolean(nullable: false));
        }
    }
}
