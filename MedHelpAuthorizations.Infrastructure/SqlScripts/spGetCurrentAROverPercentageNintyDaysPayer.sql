SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [IntegratedServices].[spGetCurrentAROverPercentageNintyDaysPayer]
	@ClientId int,
    @FromDate DateTime = NULL,
	@ToDate DateTime = NULL,
	@FirstDayOfLastMonth DateTime = NULL,
	@LastDayOfLastMonth DateTime = NULL
WITH RECOMPILE
AS
BEGIN
SET @FromDate = DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0);
SET @ToDate = EOMONTH(GETDATE());
SET @FirstDayOfLastMonth = DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 1, 0);
SET @LastDayOfLastMonth = EOMONTH(@FirstDayOfLastMonth);
SELECT 
		i.Id AS 'PayerId'
		,i.LookupName AS 'PayerName'
		,COUNT(DISTINCT(CASE WHEN c1.ClaimBilledOn >= @FromDate AND c1.ClaimBilledOn <= @ToDate THEN c1.ClaimLevelMd5Hash ELSE NULL END)) AS 'Visits'
		,SUM(CASE WHEN c1.ClaimBilledOn >= @FromDate AND c1.ClaimBilledOn <= @ToDate THEN c1.BilledAmount ELSE 0 END) AS 'Charges'
		--,AVG(CASE WHEN c1.ClaimBilledOn >= @FromDate AND c1.ClaimBilledOn <= @ToDate THEN c1.BilledAmount ELSE 0 END) AS 'AvgCharges'
		,SUM(CASE WHEN c1.ClaimBilledOn >= @FirstDayOfLastMonth AND c1.ClaimBilledOn <= @LastDayOfLastMonth THEN c1.BilledAmount ELSE 0 END) AS 'LastMonthTotals'
FROM 
		[IntegratedServices].ClaimStatusBatchClaims as c1
		JOIN(
			SELECT ClaimLevelMd5Hash as 'ClaimLevelMd5Hash'
			FROM [IntegratedServices].[ClaimStatusBatchClaims]
			WHERE ((ClaimBilledOn >= @FirstDayOfLastMonth) OR @FirstDayOfLastMonth IS NULL)
			AND ((ClaimBilledOn <= @ToDate) OR @ToDate is null)
			AND IsDeleted = 0
			AND IsSupplanted = 0
			GROUP BY ClaimLevelMd5Hash
			) as c2 ON c1.ClaimLevelMd5Hash = c2.ClaimLevelMd5Hash
		LEFT JOIN [IntegratedServices].ClaimStatusTransactions AS t ON t.ClaimStatusBatchClaimId = c1.Id
		JOIN [IntegratedServices].ClaimStatusBatches AS b ON c1.ClaimStatusBatchId = b.Id
		--LEFT JOIN [dbo].[Clients] AS c ON c1.ClientId = c.Id
		LEFT JOIN [dbo].[ClientInsurances] AS i ON i.Id = c1.ClientInsuranceId
		LEFT JOIN [IntegratedServices].[ClaimLineItemStatuses] AS cs ON cs.Id = t.ClaimLineItemStatusId
		WHERE c1.ClientId = @ClientId
		AND cs.ClaimStatusTypeId = 2
		GROUP BY i.Id, i.LookupName
	END
