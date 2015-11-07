namespace QOAM.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class doajseal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Journals", "DoajSeal", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Journals", "DoajSeal");
        }
    }
}
