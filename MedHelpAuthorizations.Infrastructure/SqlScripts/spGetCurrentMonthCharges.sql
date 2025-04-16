SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [IntegratedServices].[spGetCurrentMonthCharges]
	@UserId NVARCHAR(MAX) = NULL,
    @FromDate DateTime = NULL,
	@ToDate DateTime = NULL,
	@FirstDayOfLastMonth DateTime = NULL,
	@LastDayOfLastMonth DateTime = NULL
WITH RECOMPILE
AS
BEGIN
SET @FromDate = DATEADD(month, DATEDIFF(month, 0, GETDATE())-1, 0);
SET @ToDate = EOMONTH(GETDATE());
SET @FirstDayOfLastMonth = DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 2, 0);
SET @LastDayOfLastMonth = EOMONTH(@FirstDayOfLastMonth);
SELECT 
		c.Id AS 'ClientId'
		,c.[Name] AS 'ClientName'
		,c.ClientCode AS 'ClientCode'
		,COUNT(DISTINCT(CASE WHEN c1.ClaimBilledOn >= @FromDate AND c1.ClaimBilledOn <= @ToDate THEN c1.ClaimLevelMd5Hash ELSE NULL END)) AS 'Visits'
		,SUM(CASE WHEN c1.ClaimBilledOn >= @FromDate AND c1.ClaimBilledOn <= @ToDate THEN c1.BilledAmount ELSE 0 END) AS 'Charges'
		,AVG(CASE WHEN c1.ClaimBilledOn >= @FromDate AND c1.ClaimBilledOn <= @ToDate THEN c1.BilledAmount ELSE 0 END) AS 'AvgCharges'
		,SUM(CASE WHEN c1.ClaimBilledOn >= @FirstDayOfLastMonth AND c1.ClaimBilledOn <= @LastDayOfLastMonth THEN c1.BilledAmount ELSE 0 END) AS 'LastMonthTotals'
FROM 
		[IntegratedServices].ClaimStatusBatchClaims as c1
		JOIN(
			SELECT max(ClaimBilledOn) as 'LatestClaimBilledOn', ClaimLevelMd5Hash as 'ClaimLevelMd5Hash'
			FROM [IntegratedServices].[ClaimStatusBatchClaims]
			WHERE ((ClaimBilledOn >= @FirstDayOfLastMonth) OR @FirstDayOfLastMonth IS NULL)
			AND ((ClaimBilledOn <= @ToDate) OR @ToDate is null)
			AND IsDeleted = 0
			AND IsSupplanted = 0
			GROUP BY ClaimLevelMd5Hash
			) as c2 ON c1.ClaimLevelMd5Hash = c2.ClaimLevelMd5Hash
		LEFT JOIN [IntegratedServices].ClaimStatusTransactions AS t ON t.ClaimStatusBatchClaimId = c1.Id
		JOIN [IntegratedServices].ClaimStatusBatches AS b ON c1.ClaimStatusBatchId = b.Id
		LEFT JOIN [dbo].[Clients] AS c ON c1.ClientId = c.Id
		LEFT JOIN [dbo].[UserClients] AS uc ON c.Id = uc.ClientId
		WHERE uc.UserId = @UserId
		GROUP BY c.Id, c.[Name], c.ClientCode
	END