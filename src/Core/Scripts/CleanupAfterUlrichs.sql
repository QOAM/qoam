DELETE FROM Journals 
WHERE Id
IN (
SELECT j.id
FROM Journals j
LEFT JOIN ulrichs u ON u.issn = j.issn
WHERE u.issn IS NULL
AND NOT EXISTS (SELECT 1 FROM ScoreCards WHERE JournalId = j.Id)
);


DELETE FROM Countries
WHERE NOT Id IN (
SELECT CountryId
FROM Journals
);

DELETE FROM Publishers
WHERE NOT Id IN (
SELECT PublisherId
FROM Journals
);