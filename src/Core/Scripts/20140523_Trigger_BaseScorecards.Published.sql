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
                s.JournalId
                
                -- Recalculate the total number of reviewers
                ,COUNT(*) AS [NumberOfReviewers]
                
                -- Recalculate the total score of each category
                ,SUM([Score_EditorialInformationScore_TotalScore]) AS [EditorialInformationScore_TotalScore]
                ,SUM([Score_PeerReviewScore_TotalScore])           AS [PeerReviewScore_TotalScore]
                ,SUM([Score_GovernanceScore_TotalScore])           AS [GovernanceScore_TotalScore]
                ,SUM([Score_ProcessScore_TotalScore])              AS [ProcessScore_TotalScore]      

                -- Recalculate the average of each of the categories. Here it is important that we take into account
                -- the fact that different score card versions can have a different number of questions for a category
                ,CAST(SUM([Score_EditorialInformationScore_TotalScore]) AS FLOAT) / SUM(v.EditorialInformationNumberOfQuestions) AS [EditorialInformationScore_AverageScore]
                ,CAST(SUM([Score_PeerReviewScore_TotalScore]) AS FLOAT)           / SUM(v.PeerReviewNumberOfQuestions)           AS [PeerReviewScore_AverageScore]
                ,CAST(SUM([Score_GovernanceScore_TotalScore]) AS FLOAT)           / SUM(v.GovernanceNumberOfQuestions)           AS [GovernanceScore_AverageScore]
                ,CAST(SUM([Score_ProcessScore_TotalScore]) AS FLOAT)              / SUM(v.ProcessNumberOfQuestions)              AS [ProcessScore_AverageScore]
            FROM [BaseScoreCards] s
            INNER JOIN [ScoreCardVersions] v ON v.Id = s.VersionId -- We need the score card version to determine the number of questions of each category for the score card version
            WHERE s.[State] = 1                                    -- We are only interested in published score cards (published = 1) 
            AND s.[JournalId] IN (SELECT JournalId FROM inserted)  -- Only update the journal scores of journals that have been updated    
            GROUP BY s.[JournalId]                                 -- Group the results by the journal id so that we can calculate the sum of the score columns
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
	  WHERE [dbo].[UserProfiles].[Id] IN (SELECT UserProfileId FROM inserted);

	  UPDATE [dbo].[Institutions] 
	  SET [NumberOfBaseScoreCards] = (SELECT COUNT(*)  FROM [dbo].[BaseScoreCards] s INNER JOIN [dbo].[UserProfiles] u ON u.[Id] = s.[UserProfileId] WHERE u.[InstitutionId] = [dbo].[Institutions].[Id] AND s.[State] = 1)
	  WHERE [dbo].[Institutions].[Id] IN (SELECT InstitutionId FROM UserProfiles WHERE Id IN (SELECT UserProfileId FROM inserted));
	  
	  -- Update the total number of score cards
	  UPDATE [dbo].[UserProfiles] 
	  SET [NumberOfScoreCards] = [NumberOfBaseScoreCards] + [NumberOfValuationScoreCards]
	  WHERE [dbo].[UserProfiles].[Id] IN (SELECT UserProfileId FROM inserted);

	  UPDATE [dbo].[Institutions] 
	  SET [NumberOfScoreCards] = [NumberOfBaseScoreCards] + [NumberOfValuationScoreCards]
	  WHERE [dbo].[Institutions].[Id] IN (SELECT InstitutionId FROM UserProfiles WHERE Id IN (SELECT UserProfileId FROM inserted));
END
