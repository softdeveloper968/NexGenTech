SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER       PROCEDURE [IntegratedServices].[spGetCurrentAROverPercentageNintyDaysClient]
	@UserId NVARCHAR(MAX) = NULL
    ,@EndingARMonth DateTime = NULL
	,@EndingARYear DateTime = NULL

WITH RECOMPILE
AS
BEGIN

SET @EndingARMonth = MONTH(GETDATE())-1
SET @EndingARYear = YEAR(GETDATE())

SELECT 
		c.Id AS 'ClientId'
		,c.[Name] AS 'ClientName'
		,c.ClientCode AS 'ClientCode'
		,SUM(CASE WHEN ceom.[Month] = @EndingARMonth AND ceom.[Year] = @EndingARYear THEN ceom.ARTotalVisitsAbove90Days ELSE 0 END) AS 'Visits'
		,SUM(CASE WHEN ceom.[Month] = @EndingARMonth AND ceom.[Year] = @EndingARYear THEN ceom.ARTotalAbove90Days  ELSE 0 END) AS 'Charges'
		,SUM(CASE WHEN ceom.[Month] = @EndingARMonth AND ceom.[Year] = @EndingARYear THEN ceom.ARTotal  ELSE 0 END) AS 'ARTotals'
		,SUM(CASE WHEN ceom.[Month] = (@EndingARMonth -1) AND ceom.[Year] = @EndingARYear THEN ceom.ARTotalAbove90Days ELSE 0 END) AS 'LastMonthTotals'

FROM 
		[dbo].[ClientEndOfMonthTotals] AS ceom
		JOIN [dbo].[Clients] AS c ON ceom.ClientId = c.Id
		LEFT JOIN dbo.UserClients as uc on uc.ClientId = c.Id 
		WHERE uc.UserId = @UserId

		GROUP BY c.Id, c.[Name], c.ClientCode
END