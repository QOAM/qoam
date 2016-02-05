namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeveloperRole : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO [webpages_Roles] (RoleName) VALUES ('Developer');");
        }
        
        public override void Down()
        {
            Sql("DELETE FRMO [webpages_Roles] WHERE RoleName = 'Developer';");
        }
    }
}
