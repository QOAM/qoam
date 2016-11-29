namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastVisitedColumnOnCorner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Corners", "LastVisitedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Corners", "LastVisitedOn");
        }
    }
}
