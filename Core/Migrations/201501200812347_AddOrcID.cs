namespace QOAM.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddOrcID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "OrcId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "OrcId");
        }
    }
}
