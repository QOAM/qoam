namespace QOAM.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class BlockedISSNs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlockedISSNs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ISSN = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BlockedISSNs");
        }
    }
}
