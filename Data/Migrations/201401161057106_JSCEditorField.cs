namespace RU.Uci.OAMarket.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JSCEditorField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScoreCards", "Editor", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScoreCards", "Editor");
        }
    }
}
