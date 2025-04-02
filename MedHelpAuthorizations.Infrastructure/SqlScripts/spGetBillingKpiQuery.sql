SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [IntegratedServices].[spGetBillingKpiQuery]
	@ClientId NVARCHAR(MAX)
	,@ClaimBilledFrom DATETIME = NULL
	,@ClaimBilledTo DATETIME = NULL
WITH RECOMPILE
AS
BEGIN

set @ClaimBilledTo = GETDATE()
set @ClaimBilledFrom = DATEADD(DAY, -180, GETDATE())

SELECT
	
	C2.BDRateGoal AS 'BDRateGoal',
	--c1.Id,
	--C1.ClientId,
	C2.VisitsGoal,
	C2.ChargesGoal,
	C2.CleanClaimRateGoal,
	C2.DenialRateGoal,
	C2.CollectionPercentageGoal,
	C2.CashCollectionsGoal,
	C2.Over90DaysGoal,
	C2.DaysInARGoal,
	--c1.DailyClaimCountValue,
	c1.CleanClaimRateValue,
	C1.DenialRateValue,
	C1.ChargesValue,
	C1.VisitsValue,
	C1.CashCollectionsValue

FROM
(

SELECT 
	--COUNT(CASE WHEN CSBC.IsSupplanted = 0 THEN CSBC.ClaimLevelMd5Hash ELSE NULL END) AS 'DailyClaimCountValue'
	[IntegratedServices].[fnCalculatePercentage](COUNT(CASE WHEN cst.ClaimStatusTypeId = 1 THEN CSBC.ClaimLevelMd5Hash ELSE NULL END), COUNT(CASE WHEN cst.ClaimStatusTypeId IN (1,2) THEN CSBC.ClaimLevelMd5Hash ELSE NULL END)) AS 'CleanClaimRateValue'
	,[IntegratedServices].[fnCalculatePercentage](COUNT(CASE WHEN cst.ClaimStatusTypeId = 2 THEN CSBC.ClaimLevelMd5Hash ELSE NULL END), COUNT(CASE WHEN cst.ClaimStatusTypeId IN (1,2) THEN CSBC.ClaimLevelMd5Hash ELSE NULL END)) AS 'DenialRateValue'
	,SUM(CSBC.BilledAmount) AS 'ChargesValue'
	,COUNT(DISTINCT(CSBC.ClaimLevelMd5Hash)) AS 'VisitsValue'
	,SUM(CASE WHEN cst.ClaimStatusTypeId = 1 THEN t.LineItemPaidAmount ELSE 0 END) AS 'CashCollectionsValue' --lineItemPaidAmount OR LineItemAllowedAmount
	--Over 90 Days = claims billed on greater than 90 days and are not paid type (claimStatusTypeId != 1)
	--Days In AR
	--BD Rate

FROM 
	[dbo].ClientKpi AS CK
	LEFT JOIN [IntegratedServices].[ClaimStatusBatchClaims] AS CSBC ON CSBC.ClientId = CK.ClientId
	JOIN(
	SELECT min(bc.Id) as 'MinId'
		, EntryMd5Hash as 'HashKey' --, min(CreatedOn) as 'ReceivedDate'
		FROM  [IntegratedServices].ClaimStatusBatchClaims bc
		GROUP BY EntryMd5Hash
	) as c2 ON CSBC.EntryMd5Hash = c2.HashKey AND CSBC.Id = c2.MinId
	LEFT JOIN [IntegratedServices].ClaimStatusTransactions AS t ON t.ClaimStatusBatchClaimId = CSBC.Id
	LEFT JOIN [IntegratedServices].[ClaimLineItemStatuses] AS cst ON cst.Id = t.ClaimLineItemStatusId

	WHERE CK.ClientId = @ClientId
	AND CSBC.IsDeleted = 0
	--AND CSBC.IsSupplanted = 0
	AND ((CSBC.ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
	AND ((CSBC.ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)

	GROUP BY CK.Id, CK.ClientId, CK.Visits, CK.CleanClaimRate, CK.DenialRate, CK.DaysInAR, CK.CollectionPercentage, CK.CashCollections, CK.Over90Days, CK.BDRate, CK.Charges
) AS C1

CROSS JOIN (
SELECT * FROM [IntegratedServices].[fnCalculateBillingKpi](
    @ClientId,
    @ClaimBilledFrom,
    @ClaimBilledTo
)
) AS C2
END


