namespace QOAM.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class LastLogin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "DateLastLogin", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "DateLastLogin");
        }
    }
}
