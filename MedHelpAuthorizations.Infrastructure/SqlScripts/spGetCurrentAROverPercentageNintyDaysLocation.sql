SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER       PROCEDURE [IntegratedServices].[spGetCurrentAROverPercentageNintyDaysLocation]
	@ClientId NVARCHAR(MAX) = NULL
    ,@EndingARMonth DateTime = NULL
	,@EndingARYear DateTime = NULL

WITH RECOMPILE
AS
BEGIN

SET @EndingARMonth = MONTH(GETDATE())-1
SET @EndingARYear = YEAR(GETDATE())

SELECT 
		CL.Id AS 'ClientLocationId'
		,CL.[Name] AS 'ClientLocationName'
		,SUM(CASE WHEN ceom.[Month] = @EndingARMonth AND ceom.[Year] = @EndingARYear THEN ceom.ARTotalVisitsAbove90Days ELSE 0 END) AS 'Visits'
		,SUM(CASE WHEN ceom.[Month] = @EndingARMonth AND ceom.[Year] = @EndingARYear THEN ceom.ARTotalAbove90Days  ELSE 0 END) AS 'Charges'
		,SUM(CASE WHEN ceom.[Month] = @EndingARMonth AND ceom.[Year] = @EndingARYear THEN ceom.ARTotal  ELSE 0 END) AS 'ARTotals'
		,SUM(CASE WHEN ceom.[Month] = (@EndingARMonth -1) AND ceom.[Year] = @EndingARYear THEN ceom.ARTotalAbove90Days ELSE 0 END) AS 'LastMonthTotals'

FROM 
		[dbo].[ClientEndOfMonthTotals] AS ceom
		LEFT JOIN [dbo].[ClientLocations] AS CL ON CL.Id = ceom.ClientLocationId
		WHERE ceom.ClientId = @ClientId

		GROUP BY CL.Id, CL.[Name]
END