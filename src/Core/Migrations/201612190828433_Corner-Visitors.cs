namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CornerVisitors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CornerVisitors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CornerId = c.Int(nullable: false),
                        IpAddress = c.String(),
                        VisitedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Corners", t => t.CornerId, cascadeDelete: true)
                .Index(t => t.CornerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CornerVisitors", "CornerId", "dbo.Corners");
            DropIndex("dbo.CornerVisitors", new[] { "CornerId" });
            DropTable("dbo.CornerVisitors");
        }
    }
}
