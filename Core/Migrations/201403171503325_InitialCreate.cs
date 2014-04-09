namespace QOAM.Core.Migrations
{
    using System.Data.Entity.Migrations;

    using QOAM.Core.Scripts;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Journals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 1000),
                        ISSN = c.String(nullable: false, maxLength: 32),
                        Link = c.String(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        CountryId = c.Int(nullable: false),
                        PublisherId = c.Int(nullable: false),
                        JournalScoreId = c.Int(),
                        JournalPriceId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .ForeignKey("dbo.Publishers", t => t.PublisherId, cascadeDelete: true)
                .ForeignKey("dbo.JournalScores", t => t.JournalScoreId)
                .ForeignKey("dbo.JournalPrices", t => t.JournalPriceId)
                .Index(t => t.CountryId)
                .Index(t => t.PublisherId)
                .Index(t => t.JournalScoreId)
                .Index(t => t.JournalPriceId)
                .Index(t => t.ISSN, unique: true);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Publishers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.JournalScores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OverallScore_AverageScore = c.Single(nullable: false),
                        OverallScore_TotalScore = c.Int(nullable: false),
                        EditorialInformationScore_AverageScore = c.Single(nullable: false),
                        EditorialInformationScore_TotalScore = c.Int(nullable: false),
                        PeerReviewScore_AverageScore = c.Single(nullable: false),
                        PeerReviewScore_TotalScore = c.Int(nullable: false),
                        GovernanceScore_AverageScore = c.Single(nullable: false),
                        GovernanceScore_TotalScore = c.Int(nullable: false),
                        ProcessScore_AverageScore = c.Single(nullable: false),
                        ProcessScore_TotalScore = c.Int(nullable: false),
                        ValuationScore_AverageScore = c.Single(nullable: false),
                        ValuationScore_TotalScore = c.Int(nullable: false),
                        NumberOfReviewers = c.Int(nullable: false),
                        JournalId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Journals", t => t.JournalId, cascadeDelete: false)
                .Index(t => t.JournalId)
                .Index(t => t.NumberOfReviewers);
            
            CreateTable(
                "dbo.JournalPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateAdded = c.DateTime(nullable: false),
                        Price_Amount = c.Decimal(precision: 18, scale: 2),
                        Price_Currency = c.Int(),
                        Price_FeeType = c.Int(),
                        JournalId = c.Int(nullable: false),
                        ScoreCardId = c.Int(nullable: false),
                        UserProfileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.UserProfileId, cascadeDelete: true)
                .ForeignKey("dbo.ScoreCards", t => t.ScoreCardId, cascadeDelete: true)
                .ForeignKey("dbo.Journals", t => t.JournalId, cascadeDelete: false)
                .Index(t => t.UserProfileId)
                .Index(t => t.ScoreCardId)
                .Index(t => t.JournalId)
                .Index(t => new { t.JournalId, t.UserProfileId });
            
            CreateTable(
                "dbo.ScoreCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateStarted = c.DateTime(nullable: false),
                        DateExpiration = c.DateTime(),
                        DatePublished = c.DateTime(),
                        Remarks = c.String(),
                        UserProfileId = c.Int(nullable: false),
                        JournalId = c.Int(nullable: false),
                        VersionId = c.Int(nullable: false),
                        Score_OverallScore_AverageScore = c.Single(),
                        Score_OverallScore_TotalScore = c.Int(),
                        Score_EditorialInformationScore_AverageScore = c.Single(),
                        Score_EditorialInformationScore_TotalScore = c.Int(),
                        Score_PeerReviewScore_AverageScore = c.Single(),
                        Score_PeerReviewScore_TotalScore = c.Int(),
                        Score_GovernanceScore_AverageScore = c.Single(),
                        Score_GovernanceScore_TotalScore = c.Int(),
                        Score_ProcessScore_AverageScore = c.Single(),
                        Score_ProcessScore_TotalScore = c.Int(),
                        Score_ValuationScore_AverageScore = c.Single(),
                        Score_ValuationScore_TotalScore = c.Int(),
                        State = c.Int(nullable: false),
                        Submitted = c.Boolean(nullable: false),
                        Editor = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.UserProfileId, cascadeDelete: false)
                .ForeignKey("dbo.Journals", t => t.JournalId, cascadeDelete: true)
                .ForeignKey("dbo.ScoreCardVersions", t => t.VersionId, cascadeDelete: true)
                .Index(t => t.UserProfileId)
                .Index(t => t.JournalId)
                .Index(t => t.VersionId)
                .Index(t => t.DatePublished)
                .Index(t => new { t.JournalId, t.UserProfileId })
                .Index(t => new { t.JournalId, t.State })
                .Index(t => new { t.UserProfileId, t.State })
                .Index(t => new { t.UserProfileId, t.JournalId, t.VersionId }, unique: true);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 255),
                        DisplayName = c.String(nullable: false, maxLength: 255),
                        Email = c.String(maxLength: 255),
                        DateRegistered = c.DateTime(nullable: false),
                        NumberOfScoreCards = c.Int(nullable: false),
                        InstitutionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Institutions", t => t.InstitutionId, cascadeDelete: true)
                .Index(t => t.InstitutionId)
                .Index(t => t.DisplayName);
            
            CreateTable(
                "dbo.Institutions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        ShortName = c.String(nullable: false, maxLength: 255),
                        NumberOfScoreCards = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.InstitutionJournals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateAdded = c.DateTime(nullable: false),
                        Link = c.String(nullable: false),
                        InstitutionId = c.Int(nullable: false),
                        JournalId = c.Int(nullable: false),
                        UserProfileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Institutions", t => t.InstitutionId, cascadeDelete: true)
                .ForeignKey("dbo.Journals", t => t.JournalId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfiles", t => t.UserProfileId, cascadeDelete: false)
                .Index(t => t.InstitutionId)
                .Index(t => t.JournalId)
                .Index(t => t.UserProfileId)
                .Index(t => t.DateAdded)
                .Index(t => new { t.InstitutionId, t.JournalId }, unique: true);
            
            CreateTable(
                "dbo.ScoreCardVersions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        OverallNumberOfQuestions = c.Int(nullable: false),
                        EditorialInformationNumberOfQuestions = c.Int(nullable: false),
                        PeerReviewNumberOfQuestions = c.Int(nullable: false),
                        GovernanceNumberOfQuestions = c.Int(nullable: false),
                        ProcessNumberOfQuestions = c.Int(nullable: false),
                        ValuationNumberOfQuestions = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Number, unique: true);
            
            CreateTable(
                "dbo.QuestionScores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Score = c.Int(nullable: false),
                        ScoreCardId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ScoreCards", t => t.ScoreCardId, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.ScoreCardId)
                .Index(t => t.QuestionId)
                .Index(t => new { t.ScoreCardId, t.QuestionId }, unique: true);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.Int(nullable: false),
                        Category = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Key, unique: true);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.LanguageJournals",
                c => new
                    {
                        Language_Id = c.Int(nullable: false),
                        Journal_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Language_Id, t.Journal_Id })
                .ForeignKey("dbo.Languages", t => t.Language_Id, cascadeDelete: true)
                .ForeignKey("dbo.Journals", t => t.Journal_Id, cascadeDelete: true)
                .Index(t => t.Language_Id)
                .Index(t => t.Journal_Id);
            
            CreateTable(
                "dbo.SubjectJournals",
                c => new
                    {
                        Subject_Id = c.Int(nullable: false),
                        Journal_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Subject_Id, t.Journal_Id })
                .ForeignKey("dbo.Subjects", t => t.Subject_Id, cascadeDelete: true)
                .ForeignKey("dbo.Journals", t => t.Journal_Id, cascadeDelete: true)
                .Index(t => t.Subject_Id)
                .Index(t => t.Journal_Id);

            Sql(ResourceReader.GetContentsOfResource("201403171503325_InitialCreate_function_MinimumFloat.sql"));
            Sql(ResourceReader.GetContentsOfResource("201403171503325_InitialCreate_trigger_Scorecards.Published.sql"));
            Sql(ResourceReader.GetContentsOfResource("201403171503325_InitialCreate_MembershipTables.sql"));
        }
        
        public override void Down()
        {
            Sql("DROP TRIGGER [dbo].[ScoreCards.Published]");
            Sql("DROP FUNCTION [dbo].[MinimumFloat]");

            DropIndex("dbo.SubjectJournals", new[] { "Journal_Id" });
            DropIndex("dbo.SubjectJournals", new[] { "Subject_Id" });
            DropIndex("dbo.LanguageJournals", new[] { "Journal_Id" });
            DropIndex("dbo.LanguageJournals", new[] { "Language_Id" });
            DropIndex("dbo.QuestionScores", new[] { "QuestionId" });
            DropIndex("dbo.QuestionScores", new[] { "ScoreCardId" });
            DropIndex("dbo.InstitutionJournals", new[] { "UserProfileId" });
            DropIndex("dbo.InstitutionJournals", new[] { "JournalId" });
            DropIndex("dbo.InstitutionJournals", new[] { "InstitutionId" });
            DropIndex("dbo.UserProfiles", new[] { "InstitutionId" });
            DropIndex("dbo.ScoreCards", new[] { "VersionId" });
            DropIndex("dbo.ScoreCards", new[] { "JournalId" });
            DropIndex("dbo.ScoreCards", new[] { "UserProfileId" });
            DropIndex("dbo.JournalPrices", new[] { "JournalId" });
            DropIndex("dbo.JournalPrices", new[] { "ScoreCardId" });
            DropIndex("dbo.JournalPrices", new[] { "UserProfileId" });
            DropIndex("dbo.JournalScores", new[] { "JournalId" });
            DropIndex("dbo.Journals", new[] { "JournalPriceId" });
            DropIndex("dbo.Journals", new[] { "JournalScoreId" });
            DropIndex("dbo.Journals", new[] { "PublisherId" });
            DropIndex("dbo.Journals", new[] { "CountryId" });
            DropForeignKey("dbo.SubjectJournals", "Journal_Id", "dbo.Journals");
            DropForeignKey("dbo.SubjectJournals", "Subject_Id", "dbo.Subjects");
            DropForeignKey("dbo.LanguageJournals", "Journal_Id", "dbo.Journals");
            DropForeignKey("dbo.LanguageJournals", "Language_Id", "dbo.Languages");
            DropForeignKey("dbo.QuestionScores", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.QuestionScores", "ScoreCardId", "dbo.ScoreCards");
            DropForeignKey("dbo.InstitutionJournals", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.InstitutionJournals", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.InstitutionJournals", "InstitutionId", "dbo.Institutions");
            DropForeignKey("dbo.UserProfiles", "InstitutionId", "dbo.Institutions");
            DropForeignKey("dbo.ScoreCards", "VersionId", "dbo.ScoreCardVersions");
            DropForeignKey("dbo.ScoreCards", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.ScoreCards", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.JournalPrices", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.JournalPrices", "ScoreCardId", "dbo.ScoreCards");
            DropForeignKey("dbo.JournalPrices", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.JournalScores", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.Journals", "JournalPriceId", "dbo.JournalPrices");
            DropForeignKey("dbo.Journals", "JournalScoreId", "dbo.JournalScores");
            DropForeignKey("dbo.Journals", "PublisherId", "dbo.Publishers");
            DropForeignKey("dbo.Journals", "CountryId", "dbo.Countries");
            DropTable("dbo.SubjectJournals");
            DropTable("dbo.LanguageJournals");
            DropTable("dbo.Subjects");
            DropTable("dbo.Languages");
            DropTable("dbo.Questions");
            DropTable("dbo.QuestionScores");
            DropTable("dbo.ScoreCardVersions");
            DropTable("dbo.InstitutionJournals");
            DropTable("dbo.Institutions");
            DropTable("dbo.UserProfiles");
            DropTable("dbo.ScoreCards");
            DropTable("dbo.JournalPrices");
            DropTable("dbo.JournalScores");
            DropTable("dbo.Publishers");
            DropTable("dbo.Countries");
            DropTable("dbo.Journals");
            DropTable("dbo.webpages_Membership");
            DropTable("dbo.webpages_OAuthMembership");
            DropTable("dbo.webpages_Roles");
            DropTable("dbo.webpages_UsersInRoles");
        }
    }
}
