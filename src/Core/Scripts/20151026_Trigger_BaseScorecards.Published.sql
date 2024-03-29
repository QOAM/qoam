ALTER TRIGGER [dbo].[ScoreCards.Published]
   ON [dbo].[BaseScoreCards]
   AFTER UPDATE
AS BEGIN
    
	MERGE INTO JournalScores j
    USING (
    SELECT         
        a.*,

        -- Calculate the overall average score by setting it to the lowest of the category scores (which have been
        -- calculated in the sub-query)
        dbo.MinimumFloat(a.EditorialInformationScore_AverageScore, dbo.MinimumFloat(a.PeerReviewScore_AverageScore, dbo.MinimumFloat(a.GovernanceScore_AverageScore, a.ProcessScore_AverageScore))) AS [OverallScore_AverageScore]        
        FROM
        (
             SELECT 
                j.[Id] AS [JournalId]
                
                -- Recalculate the total number of reviewers
                ,COALESCE(COUNT(s.Id), 0) AS [NumberOfReviewers]
                
                -- Recalculate the total score of each category
                ,COALESCE(SUM([Score_EditorialInformationScore_TotalScore]), 0) AS [EditorialInformationScore_TotalScore]
                ,COALESCE(SUM([Score_PeerReviewScore_TotalScore]), 0)           AS [PeerReviewScore_TotalScore]
                ,COALESCE(SUM([Score_GovernanceScore_TotalScore]), 0)           AS [GovernanceScore_TotalScore]
                ,COALESCE(SUM([Score_ProcessScore_TotalScore]), 0)              AS [ProcessScore_TotalScore]      

                -- Recalculate the average of each of the categories. Here it is important that we take into account
                -- the fact that different score card versions can have a different number of questions for a category
                ,CAST(COALESCE(SUM([Score_EditorialInformationScore_TotalScore]), 0) AS FLOAT) / COALESCE(SUM(v.EditorialInformationNumberOfQuestions), 1) AS [EditorialInformationScore_AverageScore]
                ,CAST(COALESCE(SUM([Score_PeerReviewScore_TotalScore]), 0) AS FLOAT)           / COALESCE(SUM(v.PeerReviewNumberOfQuestions), 1)           AS [PeerReviewScore_AverageScore]
                ,CAST(COALESCE(SUM([Score_GovernanceScore_TotalScore]), 0) AS FLOAT)           / COALESCE(SUM(v.GovernanceNumberOfQuestions), 1)           AS [GovernanceScore_AverageScore]
                ,CAST(COALESCE(SUM([Score_ProcessScore_TotalScore]), 0) AS FLOAT)              / COALESCE(SUM(v.ProcessNumberOfQuestions), 1)              AS [ProcessScore_AverageScore]
            FROM [Journals] j

			-- We are only interested in published score cards (published = 1)             
			LEFT JOIN [BaseScoreCards] s ON (s.[JournalId] = j.[Id] AND s.[State] = 1)
            
			-- We need the score card version to determine the number of questions of each category for the score card version
			LEFT JOIN [ScoreCardVersions] v ON v.Id = s.VersionId 
            
			-- Only update the journal scores of journals that have been updated    
			WHERE j.[Id] IN (SELECT JournalId FROM inserted UNION SELECT JournalId FROM deleted)		     
            
			-- Group the results by the journal id so that we can calculate the sum of the score columns
			GROUP BY j.[Id]                                 
        ) a
    ) u ON u.JournalId = j.JournalId
    WHEN MATCHED THEN 
      UPDATE SET
      j.NumberOfBaseReviewers = u.NumberOfReviewers,
      j.EditorialInformationScore_AverageScore = u.EditorialInformationScore_AverageScore,	
      j.EditorialInformationScore_TotalScore = u.EditorialInformationScore_TotalScore,	
      j.PeerReviewScore_AverageScore = u.PeerReviewScore_AverageScore,	
      j.PeerReviewScore_TotalScore = u.PeerReviewScore_TotalScore,	
      j.GovernanceScore_AverageScore = u.GovernanceScore_AverageScore,	
      j.GovernanceScore_TotalScore = u.GovernanceScore_TotalScore,
      j.ProcessScore_AverageScore = u.ProcessScore_AverageScore,
      j.ProcessScore_TotalScore = u.ProcessScore_TotalScore,	
      j.OverallScore_AverageScore = u.OverallScore_AverageScore;    
	  
	  -- Update the number of base score cards
	  UPDATE [dbo].[UserProfiles] 
	  SET [NumberOfBaseScoreCards] = (SELECT COUNT(*) FROM [dbo].[BaseScoreCards] s WHERE s.[UserProfileId] = [dbo].[UserProfiles].[Id] AND s.[State] = 1)
	  WHERE [dbo].[UserProfiles].[Id] IN (SELECT UserProfileId FROM inserted UNION SELECT UserProfileId FROM deleted);

	  UPDATE [dbo].[Institutions] 
	  SET [NumberOfBaseScoreCards] = (SELECT COUNT(*)  FROM [dbo].[BaseScoreCards] s INNER JOIN [dbo].[UserProfiles] u ON u.[Id] = s.[UserProfileId] WHERE u.[InstitutionId] = [dbo].[Institutions].[Id] AND s.[State] = 1)
	  WHERE [dbo].[Institutions].[Id] IN (SELECT InstitutionId FROM UserProfiles WHERE Id IN (SELECT UserProfileId FROM inserted UNION SELECT UserProfileId FROM deleted));
	  
	  -- Update the total number of score cards
	  UPDATE [dbo].[UserProfiles] 
	  SET [NumberOfScoreCards] = [NumberOfBaseScoreCards] + [NumberOfValuationScoreCards]
	  WHERE [dbo].[UserProfiles].[Id] IN (SELECT UserProfileId FROM inserted UNION SELECT UserProfileId FROM deleted);

	  UPDATE [dbo].[Institutions] 
	  SET [NumberOfScoreCards] = [NumberOfBaseScoreCards] + [NumberOfValuationScoreCards]
	  WHERE [dbo].[Institutions].[Id] IN (SELECT InstitutionId FROM UserProfiles WHERE Id IN (SELECT UserProfileId FROM inserted UNION SELECT UserProfileId FROM deleted));
END
