SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER FUNCTION [IntegratedServices].[fnCalculatePercentage]
(
    @Numerator DECIMAL,
    @Denominator DECIMAL
)
RETURNS DECIMAL
AS
BEGIN
    DECLARE @Percentage DECIMAL;
    
    IF @Denominator = 0
        SET @Percentage = 0;
    ELSE
        SET @Percentage = (@Numerator / @Denominator) * 100;
    
    RETURN @Percentage;
END;
