namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using Scripts;

    public partial class MoveScoreCards : DbMigration
    {
        public override void Up()
        {
            Sql(ResourceReader.GetContentsOfResource("20151026_Trigger_BaseScorecards.Published.sql"));
            Sql(ResourceReader.GetContentsOfResource("20151026_Trigger_ValuationScorecards.Published.sql"));
        }
        
        public override void Down()
        {
            Sql(ResourceReader.GetContentsOfResource("20140523_Trigger_BaseScorecards.Published.sql"));
            Sql(ResourceReader.GetContentsOfResource("20140523_Trigger_ValuationScorecards.Published.sql"));
        }
    }
}
