namespace QOAM.Core.Migrations
{
    using System.Data.Entity.Migrations;
    using Scripts;

    public partial class UserInstitutionChange : DbMigration
    {
        public override void Up()
        {
            Sql(ResourceReader.GetContentsOfResource("20151029_Trigger_UserProfiles.Updated.sql"));
            Sql(ResourceReader.GetContentsOfResource("20151029_Trigger_UserProfiles.Deleted.sql"));
        }

        public override void Down()
        {
            Sql("DROP TRIGGER [UserProfiles.Updated]");
            Sql("DROP TRIGGER [UserProfiles.Deleted]");
        }
    }
}
