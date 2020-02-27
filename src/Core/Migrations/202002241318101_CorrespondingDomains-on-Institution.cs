namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrespondingDomainsonInstitution : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Institutions", "CorrespondingInstitutions", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Institutions", "CorrespondingInstitutions");
        }
    }
}
