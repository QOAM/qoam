namespace RU.Uci.OAMarket.Data
{
    using System;
    using System.Data.Entity.Migrations;

    using RU.Uci.OAMarket.Data.Scripts;

    public partial class NumberOfScoreCards : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "NumberOfScoreCards", c => c.Int(nullable: false));
            AddColumn("dbo.Institutions", "NumberOfScoreCards", c => c.Int(nullable: false));

            this.Sql(ResourceReader.GetContentsOfResource("201403120902369_NumberOfScoreCards_trigger_Scorecards.Published.sql"));

            // Do an initial update of the columns
            Sql("UPDATE [dbo].[UserProfiles] SET [NumberOfScoreCards] = (SELECT COUNT(*) FROM [dbo].[ScoreCards] s WHERE s.[UserProfileId] = [dbo].[UserProfiles].[Id] AND s.[State] = 1);");
            Sql("UPDATE [dbo].[Institutions] SET [NumberOfScoreCards] = (SELECT COUNT(*) FROM [dbo].[ScoreCards] s INNER JOIN [dbo].[UserProfiles] u ON u.[Id] = s.[UserProfileId] WHERE u.[InstitutionId] = [dbo].[Institutions].[Id] AND s.[State] = 1);");
        }
        
        public override void Down()
        {
            this.Sql(ResourceReader.GetContentsOfResource("201402060818493_InitialCreate_trigger_Scorecards.Published.sql"));

            DropColumn("dbo.Institutions", "NumberOfScoreCards");
            DropColumn("dbo.UserProfiles", "NumberOfScoreCards");
        }

        
    }
}
