CREATE TRIGGER [dbo].[ValuationScoreCards.Deleted]
   ON [dbo].[ValuationScoreCards]
   AFTER DELETE
AS BEGIN
    
	MERGE INTO JournalScores j
    USING (
	    SELECT  
		    j.[Id] AS [JournalId]
                
            -- Recalculate the total number of reviewers
            ,COALESCE(COUNT(s.Id), 0) AS [NumberOfReviewers]
                
            -- Recalculate the total score of each category
            ,COALESCE(SUM([Score_ValuationScore_TotalScore]), 0) AS [ValuationScore_TotalScore]

            -- Recalculate the average of each of the categories. Here it is important that we take into account
            -- the fact that different score card versions can have a different number of questions for a category
            ,CAST(COALESCE(SUM([Score_ValuationScore_TotalScore]), 0) AS FLOAT) / COALESCE(SUM(v.ValuationNumberOfQuestions), 1) AS [ValuationScore_AverageScore]
        FROM [Journals] j

		-- We are only interested in published score cards (published = 1)             
		LEFT JOIN [ValuationScoreCards] s ON (s.[JournalId] = j.[Id] AND s.[State] = 1)
            
		-- We need the score card version to determine the number of questions of each category for the score card version
		LEFT JOIN [ScoreCardVersions] v ON v.Id = s.VersionId 
            
		-- Only update the journal scores of journals that have been updated    
		WHERE j.[Id] IN (SELECT JournalId FROM deleted)		     
            
		-- Group the results by the journal id so that we can calculate the sum of the score columns
		GROUP BY j.[Id]
    ) u ON u.JournalId = j.JournalId
    WHEN MATCHED THEN 
      UPDATE SET
      j.NumberOfValuationReviewers = u.NumberOfReviewers,
      j.ValuationScore_AverageScore = u.ValuationScore_AverageScore,	
      j.ValuationScore_TotalScore = u.ValuationScore_TotalScore;    
	  
	  -- Update the number of valuation score cards
	  UPDATE [dbo].[UserProfiles] 
	  SET [NumberOfValuationScoreCards] = (SELECT COUNT(*) FROM [dbo].[ValuationScoreCards] s WHERE s.[UserProfileId] = [dbo].[UserProfiles].[Id] AND s.[State] = 1)
	  WHERE [dbo].[UserProfiles].[Id] IN (SELECT UserProfileId FROM deleted);

	  UPDATE [dbo].[Institutions] 
	  SET [NumberOfValuationScoreCards] = (SELECT COUNT(*) FROM [dbo].[ValuationScoreCards] s INNER JOIN [dbo].[UserProfiles] u ON u.[Id] = s.[UserProfileId] WHERE u.[InstitutionId] = [dbo].[Institutions].[Id] AND s.[State] = 1)
	  WHERE [dbo].[Institutions].[Id] IN (SELECT InstitutionId FROM UserProfiles WHERE Id IN (SELECT UserProfileId FROM deleted));
	  
	  -- Update the total number of score cards
	  UPDATE [dbo].[UserProfiles] 
	  SET [NumberOfScoreCards] = [NumberOfBaseScoreCards] + [NumberOfValuationScoreCards]
	  WHERE [dbo].[UserProfiles].[Id] IN (SELECT UserProfileId FROM deleted);

	  UPDATE [dbo].[Institutions] 
	  SET [NumberOfScoreCards] = [NumberOfBaseScoreCards] + [NumberOfValuationScoreCards]
	  WHERE [dbo].[Institutions].[Id] IN (SELECT InstitutionId FROM UserProfiles WHERE Id IN (SELECT UserProfileId FROM deleted));
END
