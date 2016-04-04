namespace QOAM.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CascadeDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BaseScoreCards", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.BaseScoreCards", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.BaseJournalPrices", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.BaseJournalPrices", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.ValuationScoreCards", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.ValuationScoreCards", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.webpages_UsersInRoles", "UserId", "dbo.UserProfiles");
            DropForeignKey("dbo.webpages_UsersInRoles", "RoleId", "dbo.webpages_Roles");

            AddForeignKey("dbo.BaseScoreCards", "UserProfileId", "dbo.UserProfiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BaseScoreCards", "JournalId", "dbo.Journals", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BaseJournalPrices", "UserProfileId", "dbo.UserProfiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BaseJournalPrices", "JournalId", "dbo.Journals", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ValuationScoreCards", "UserProfileId", "dbo.UserProfiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ValuationScoreCards", "JournalId", "dbo.Journals", "Id", cascadeDelete: true);
            AddForeignKey("dbo.webpages_UsersInRoles", "UserId", "dbo.UserProfiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.webpages_UsersInRoles", "RoleId", "dbo.webpages_Roles", "RoleId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BaseScoreCards", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.BaseScoreCards", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.BaseJournalPrices", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.BaseJournalPrices", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.ValuationScoreCards", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.ValuationScoreCards", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.webpages_UsersInRoles", "UserId", "dbo.UserProfiles");
            DropForeignKey("dbo.webpages_UsersInRoles", "RoleId", "dbo.webpages_Roles");

            AddForeignKey("dbo.BaseScoreCards", "UserProfileId", "dbo.UserProfiles", "Id", cascadeDelete: false);
            AddForeignKey("dbo.BaseScoreCards", "JournalId", "dbo.Journals", "Id", cascadeDelete: false);
            AddForeignKey("dbo.BaseJournalPrices", "UserProfileId", "dbo.UserProfiles", "Id", cascadeDelete: false);
            AddForeignKey("dbo.BaseJournalPrices", "JournalId", "dbo.Journals", "Id", cascadeDelete: false);
            AddForeignKey("dbo.ValuationScoreCards", "UserProfileId", "dbo.UserProfiles", "Id", cascadeDelete: false);
            AddForeignKey("dbo.ValuationScoreCards", "JournalId", "dbo.Journals", "Id", cascadeDelete: false);
            AddForeignKey("dbo.webpages_UsersInRoles", "UserId", "dbo.UserProfiles", "Id", cascadeDelete: false);
            AddForeignKey("dbo.webpages_UsersInRoles", "RoleId", "dbo.webpages_Roles", "RoleId", cascadeDelete: false);
        }
    }
}
