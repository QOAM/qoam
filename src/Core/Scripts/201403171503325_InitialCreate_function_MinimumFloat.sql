CREATE FUNCTION MinimumFloat(@a AS float, @b AS float) 
RETURNS float
AS
BEGIN
    RETURN CASE WHEN @a < @b THEN @a ELSE COALESCE(@b,@a) END
END