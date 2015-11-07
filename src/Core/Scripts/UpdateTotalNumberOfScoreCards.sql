UPDATE [dbo].[UserProfiles] 
	  SET [NumberOfValuationScoreCards] = (SELECT COUNT(*) FROM [dbo].[ValuationScoreCards] s WHERE s.[UserProfileId] = [dbo].[UserProfiles].[Id] AND s.[State] = 1),
	      [NumberOfBaseScoreCards] = (SELECT COUNT(*) FROM [dbo].[BaseScoreCards] s WHERE s.[UserProfileId] = [dbo].[UserProfiles].[Id] AND s.[State] = 1);

UPDATE [dbo].[Institutions] 
	  SET [NumberOfValuationScoreCards] = (SELECT COUNT(*)  FROM [dbo].[ValuationScoreCards] s INNER JOIN [dbo].[UserProfiles] u ON u.[Id] = s.[UserProfileId] WHERE u.[InstitutionId] = [dbo].[Institutions].[Id] AND s.[State] = 1),
	      [NumberOfBaseScoreCards] = (SELECT COUNT(*)  FROM [dbo].[BaseScoreCards] s INNER JOIN [dbo].[UserProfiles] u ON u.[Id] = s.[UserProfileId] WHERE u.[InstitutionId] = [dbo].[Institutions].[Id] AND s.[State] = 1);
		  
UPDATE [dbo].[UserProfiles] SET [NumberOfScoreCards] = [NumberOfBaseScoreCards] + [NumberOfValuationScoreCards];
UPDATE [dbo].[Institutions] SET [NumberOfScoreCards] = [NumberOfBaseScoreCards] + [NumberOfValuationScoreCards];