CREATE TRIGGER [dbo].[UserProfiles.Updated]
   ON [dbo].[UserProfiles]
   AFTER UPDATE
AS BEGIN

  UPDATE [dbo].[Institutions] 
  SET [NumberOfBaseScoreCards] = (SELECT COUNT(*)  FROM [dbo].[BaseScoreCards] s INNER JOIN [dbo].[UserProfiles] u ON u.[Id] = s.[UserProfileId] WHERE u.[InstitutionId] = [dbo].[Institutions].[Id] AND s.[State] = 1),
      [NumberOfValuationScoreCards] = (SELECT COUNT(*) FROM [dbo].[ValuationScoreCards] s INNER JOIN [dbo].[UserProfiles] u ON u.[Id] = s.[UserProfileId] WHERE u.[InstitutionId] = [dbo].[Institutions].[Id] AND s.[State] = 1)
  WHERE [dbo].[Institutions].[Id] IN (SELECT InstitutionId FROM inserted UNION SELECT InstitutionId FROM deleted);
    
  UPDATE [dbo].[Institutions] 
  SET [NumberOfScoreCards] = [NumberOfBaseScoreCards] + [NumberOfValuationScoreCards]
  WHERE [dbo].[Institutions].[Id] IN (SELECT InstitutionId FROM inserted UNION SELECT InstitutionId FROM deleted);

END
