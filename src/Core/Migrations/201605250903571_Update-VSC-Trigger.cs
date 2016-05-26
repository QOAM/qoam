using QOAM.Core.Scripts;

namespace QOAM.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UpdateVSCTrigger : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM dbo.__MigrationHistory WHERE [MigrationId] = '201605191108426_Update-VSC-Trigger'");
            Sql("DELETE FROM dbo.__MigrationHistory WHERE [MigrationId] = '201605201155561_Update-VSC-Trigger'");
            Sql("DELETE FROM dbo.__MigrationHistory WHERE [MigrationId] = '201605201300419_Update-VSC-Trigger'");
            Sql("DROP TRIGGER [ValuationScoreCards.Modified]");
            Sql(ResourceReader.GetContentsOfResource("20160519_Trigger_ValuationScorecards_v2.Modified.sql"));
        }

        public override void Down()
        {
            Sql("DROP TRIGGER [ValuationScoreCards.Modified]");
            Sql(ResourceReader.GetContentsOfResource("20160329_Trigger_ValuationScorecards.Modified.sql"));
        }
    }
}
