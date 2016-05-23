CREATE TRIGGER [dbo].[ValuationScoreCards.Modified]
   ON [dbo].[ValuationScoreCards]
   AFTER UPDATE, DELETE
AS BEGIN
    
	;WITH X AS
		(
			SELECT  
				j.[Id] AS [JournalId]
                
				-- Recalculate the total number of reviewers
				,COALESCE(COUNT(s.Id), 0) AS [NumberOfReviewers]                
				-- Recalculate the total score of each category
				,COALESCE(SUM([Score_ValuationScore_TotalScore]), 0) AS [ValuationScore_TotalScore]
			FROM [Journals] j

			-- We are only interested in published score cards (published = 1)             
			LEFT JOIN [ValuationScoreCards] s ON (s.[JournalId] = j.[Id] AND s.[State] = 1)
            
			-- Only update the journal scores of journals that have been updated    
			WHERE j.[Id] IN (SELECT JournalId FROM inserted UNION SELECT JournalId FROM deleted)		     
            
			-- Group the results by the journal id so that we can calculate the sum of the score columns
			GROUP BY j.[Id]
		), v1 AS
		(
			SELECT  
				j.[Id]
				-- Recalculate the average of each of the categories. Here it is important that we take into account
				-- the fact that different score card versions can have a different number of questions for a category
				,CAST(COALESCE(SUM([Score_ValuationScore_TotalScore]), 0) AS FLOAT) / COALESCE(SUM(v.ValuationNumberOfQuestions), 1) AS [ValuationScore_AverageScore]
			FROM [Journals] j

			-- We are only interested in published score cards (published = 1)             
			LEFT JOIN [ValuationScoreCards] s ON (s.[JournalId] = j.[Id] AND s.[State] = 1)
            
			-- We need the score card version to determine the number of questions of each category for the score card version
			LEFT JOIN [ScoreCardVersions] v ON v.Id = s.VersionId 
            
			-- Only update the journal scores of journals that have been updated    
			WHERE v.[Number] = 1 AND j.[Id] IN (SELECT JournalId FROM inserted UNION SELECT JournalId FROM deleted)		     
            
			-- Group the results by the journal id so that we can calculate the sum of the score columns
			GROUP BY j.[Id]
		), v2 AS
		(
			SELECT  
				j.[Id]
				-- Recalculate the average of each of the categories. Here it is important that we take into account
				-- the fact that different score card versions can have a different number of questions for a category
				,CAST(COALESCE(SUM([Score_ValuationScore_TotalScore]), 0) AS FLOAT) / COALESCE(SUM(v.ValuationNumberOfQuestions), 1) AS [ValuationScore_AverageScore]
			FROM [Journals] j

			-- We are only interested in published score cards (published = 1)             
			LEFT JOIN [ValuationScoreCards] s ON (s.[JournalId] = j.[Id] AND s.[State] = 1)
            
			-- We need the score card version to determine the number of questions of each category for the score card version
			LEFT JOIN [ScoreCardVersions] v ON v.Id = s.VersionId 
            
			-- Only update the journal scores of journals that have been updated    
			WHERE v.[Number] = 2 AND j.[Id] IN (SELECT JournalId FROM inserted UNION SELECT JournalId FROM deleted)		     
            
			-- Group the results by the journal id so that we can calculate the sum of the score columns
			GROUP BY j.[Id]
		)
	MERGE INTO Journals j
    USING (
		SELECT DISTINCT
		    j.[Id] AS [JournalId]
                
            -- Recalculate the total number of reviewers
            ,X.[NumberOfReviewers]
                
            -- Recalculate the total score of each category
            ,X.[ValuationScore_TotalScore]

            -- Recalculate the average of each of the categories. Here it is important that we take into account
            -- the fact that different score card versions can have a different number of questions for a category
            ,COALESCE(v1.[ValuationScore_AverageScore], 0) AS [ValuationScore_AverageScore_v1]
			,COALESCE(v2.[ValuationScore_AverageScore], 0) AS [ValuationScore_AverageScore_v2]
        FROM [Journals] j

		LEFT OUTER JOIN X on X.JournalId = j.Id
		LEFT OUTER JOIN v1 on v1.Id = j.Id
		LEFT OUTER JOIN v2 on v2.Id = j.Id
            
		-- Only update the journal scores of journals that have been updated    
		WHERE j.[Id] IN (SELECT JournalId FROM inserted UNION SELECT JournalId FROM deleted)
    ) u ON u.JournalId = j.Id
    WHEN MATCHED THEN 
      UPDATE SET
      j.NumberOfValuationReviewers = u.NumberOfReviewers,
      j.ValuationScore_AverageScore = (u.ValuationScore_AverageScore_v1 + u.ValuationScore_AverageScore_v2) / (CASE WHEN u.NumberOfReviewers > 0 THEN u.NumberOfReviewers ELSE 1 END),
      j.ValuationScore_TotalScore = u.ValuationScore_TotalScore;    
	  
	  -- Update the number of valuation score cards
	  UPDATE [dbo].[UserProfiles] 
	  SET [NumberOfValuationScoreCards] = (SELECT COUNT(*) FROM [dbo].[ValuationScoreCards] s WHERE s.[UserProfileId] = [dbo].[UserProfiles].[Id] AND s.[State] = 1)
	  WHERE [dbo].[UserProfiles].[Id] IN (SELECT UserProfileId FROM inserted UNION SELECT UserProfileId FROM deleted);

	  UPDATE [dbo].[Institutions] 
	  SET [NumberOfValuationScoreCards] = (SELECT COUNT(*) FROM [dbo].[ValuationScoreCards] s INNER JOIN [dbo].[UserProfiles] u ON u.[Id] = s.[UserProfileId] WHERE u.[InstitutionId] = [dbo].[Institutions].[Id] AND s.[State] = 1)
	  WHERE [dbo].[Institutions].[Id] IN (SELECT InstitutionId FROM UserProfiles WHERE Id IN (SELECT UserProfileId FROM inserted UNION SELECT UserProfileId FROM deleted));
	  
	  -- Update the total number of score cards
	  UPDATE [dbo].[UserProfiles] 
	  SET [NumberOfScoreCards] = [NumberOfBaseScoreCards] + [NumberOfValuationScoreCards]
	  WHERE [dbo].[UserProfiles].[Id] IN (SELECT UserProfileId FROM inserted UNION SELECT UserProfileId FROM deleted);

	  UPDATE [dbo].[Institutions] 
	  SET [NumberOfScoreCards] = [NumberOfBaseScoreCards] + [NumberOfValuationScoreCards]
	  WHERE [dbo].[Institutions].[Id] IN (SELECT InstitutionId FROM UserProfiles WHERE Id IN (SELECT UserProfileId FROM inserted UNION SELECT UserProfileId FROM deleted));
END