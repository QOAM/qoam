namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using Scripts;

    public partial class RemoveScoreCards : DbMigration
    {
        public override void Up()
        {
            Sql(ResourceReader.GetContentsOfResource("20151027_Trigger_BaseScorecards.Deleted.sql"));
            Sql(ResourceReader.GetContentsOfResource("20151027_Trigger_ValuationScorecards.Deleted.sql"));
        }

        public override void Down()
        {
            Sql(ResourceReader.GetContentsOfResource("20151026_Trigger_BaseScorecards.Published.sql"));
            Sql(ResourceReader.GetContentsOfResource("20151026_Trigger_ValuationScorecards.Published.sql"));
        }
    }
}
