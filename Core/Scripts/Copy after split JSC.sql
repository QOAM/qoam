INSERT INTO [dbo].[ValuationScoreCards]
	( DateStarted, DateExpiration, DatePublished, Remarks, UserProfileId, JournalId
	, VersionId, Score_ValuationScore_AverageScore, Score_ValuationScore_TotalScore
	,State, Submitted, Editor, BaseScoreCardId)

	SELECT DateStarted, DateExpiration, DatePublished, Remarks, UserProfileId, JournalId
	, VersionId, Score_ValuationScore_AverageScore, Score_ValuationScore_TotalScore
	,State, Submitted, Editor, Id

	FROM [dbo].[BaseScoreCards];

INSERT INTO [dbo].[ValuationJournalPrices]
	( DateAdded, Price_Amount, Price_Currency, JournalId, UserProfileId, Price_FeeType, ValuationScoreCardId )

	SELECT b.DateAdded, b.Price_Amount, b.Price_Currency, b.JournalId, b.UserProfileId, b.Price_FeeType, v.Id

	FROM [dbo].[BaseJournalPrices] b, [dbo].[ValuationScoreCards] v

	WHERE (v.BaseScoreCardId=b.BaseScoreCardId) AND (v.Submitted=1);

DELETE FROM [dbo].[BaseJournalPrices]
	WHERE BaseScoreCardId IN 

	( SELECT BaseScoreCardId 

	  FROM [dbo].[ValuationScoreCards] v

	  WHERE (v.Submitted=1) )

UPDATE [dbo].[Journals]
	SET BaseJournalPriceId = null 
	WHERE Id IN

	( SELECT JournalId

	  FROM [dbo].[ValuationScoreCards] v

	  WHERE (v.Submitted=1) )

INSERT INTO [dbo].[ValuationQuestionScores]
	(Score,ValuationScoreCardId,QuestionId)

	SELECT Score, v.Id, QuestionId
	
	FROM [dbo].[BaseQuestionScores] b, [dbo].[ValuationScoreCards] v

	WHERE (QuestionId >17) AND (v.BaseScoreCardId=b.BaseScoreCardId) ;

DELETE FROM [dbo].[BaseQuestionScores]

	WHERE (QuestionId >17);

UPDATE [dbo].[JournalScores] SET [NumberOfValuationReviewers] = [NumberOfBaseReviewers];

DELETE [dbo].[BaseJournalPrices]  
FROM [dbo].[BaseJournalPrices] 
LEFT OUTER JOIN (
   SELECT MAX(Id) as RowId, JournalId, UserProfileId
   FROM [dbo].[BaseJournalPrices] 
   GROUP BY JournalId, UserProfileId
) as KeepRows ON
   [dbo].[BaseJournalPrices].Id = KeepRows.RowId
WHERE
   KeepRows.RowId IS NULL;
   
DELETE [dbo].[ValuationJournalPrices]
FROM [dbo].[ValuationJournalPrices] 
LEFT OUTER JOIN (
   SELECT MAX(Id) as RowId, JournalId, UserProfileId
   FROM [dbo].[ValuationJournalPrices] 
   GROUP BY JournalId, UserProfileId
) as KeepRows ON
   [dbo].[ValuationJournalPrices].Id = KeepRows.RowId
WHERE
   KeepRows.RowId IS NULL;