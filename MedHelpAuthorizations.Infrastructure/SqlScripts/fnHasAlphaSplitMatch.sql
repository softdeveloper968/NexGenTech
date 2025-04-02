

/****** Object:  UserDefinedFunction [IntegratedServices].[fnHasAlphaSplitMatch]    Script Date: 05/06/2024 2:41:40 PM ******/
--DROP FUNCTION [IntegratedServices].[fnHasAlphaSplitMatch]
--GO


--/****** Object:  UserDefinedTableType [dbo].[EmployeeClientAlphaSplitTable]    Script Date: 05/06/2024 2:40:45 PM ******/
--DROP TYPE [dbo].[EmployeeClientAlphaSplitTable]
--GO

IF TYPE_ID(N'EmployeeClientAlphaSplitTable') IS NULL
	CREATE TYPE EmployeeClientAlphaSplitTable AS TABLE (
		ClientId int,
		EmployeeId int, 
		EmployeeClientId int,
		BeginingAlpha NVARCHAR,
		EndingAlpha NVARCHAR
	)
GO 

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER FUNCTION [IntegratedServices].[fnHasAlphaSplitMatch](
	@ClientId int = -1
    ,@CompareString nvarchar(36) = NULL
	,@AlphaSplits EmployeeClientAlphaSplitTable READONLY)

RETURNS BIT 
AS
BEGIN
RETURN CAST(
			   CASE WHEN EXISTS(SELECT 1 FROM @AlphaSplits spl
								WHERE 1=1
								AND spl.ClientId = @ClientId
								AND spl.BeginingAlpha >= @CompareString
								AND spl.EndingAlpha <= @CompareString ) THEN 1 
			   ELSE 0 
			   END 
		AS BIT)
END

