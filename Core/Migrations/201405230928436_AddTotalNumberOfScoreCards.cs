namespace QOAM.Core.Migrations
{
    using System.Data.Entity.Migrations;

    using QOAM.Core.Scripts;

    public partial class AddTotalNumberOfScoreCards : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Institutions", "NumberOfScoreCards", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfiles", "NumberOfScoreCards", c => c.Int(nullable: false));

            CreateIndex("dbo.Institutions", "NumberOfScoreCards", unique: false);
            CreateIndex("dbo.UserProfiles", "NumberOfScoreCards", unique: false);

            Sql(ResourceReader.GetContentsOfResource("20140523_Trigger_BaseScorecards.Published.sql"));
            Sql(ResourceReader.GetContentsOfResource("20140523_Trigger_ValuationScorecards.Published.sql"));
            Sql(ResourceReader.GetContentsOfResource("UpdateTotalNumberOfScoreCards.sql"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "NumberOfScoreCards");
            DropColumn("dbo.Institutions", "NumberOfScoreCards");
        }
    }
}
