namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JournalNumberOfArticles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Journals", "NumberOfArticles", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Journals", "NumberOfArticles");
        }
    }
}
