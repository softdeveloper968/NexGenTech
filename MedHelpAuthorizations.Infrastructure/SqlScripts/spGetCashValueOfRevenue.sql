SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [IntegratedServices].[spGetCashValueOfRevenue]
    @ClientId int
    ,@FilterBy VARCHAR(MAX) = 'ServiceDate' -- 'BilledDate'
    --,@ClientInsuranceId Int = NULL
    ,@FilterForDays Int = -7
    --,@ClientInsuranceIds  VARCHAR(MAX) = ''
    ,@ClientLocationIds VARCHAR(MAX) = ''
    ,@ClientProviderIds VARCHAR(MAX)  = ''

AS
BEGIN

DECLARE @beginDate DATETIME
DECLARE @endDate DATETIME

SET @beginDate = DATEADD(dd, @FilterForDays, DATEDIFF(dd, 0, GETDATE()))
SET @endDate  = DATEADD(ms, -2, DATEADD(dd, 1, DATEDIFF(dd, 1, GetDate())))
IF @FilterBy = 'ServiceDate'
SELECT Count(c1.ClaimLevelMD5Hash) AS ClaimCount
    ,SUM(c1.BilledAmount) AS RevenueTotals, c1.ClaimLevelMd5Hash, c1.DateOfServiceFrom as 'ServiceDate', i.Id AS ClientInsuranceId, i.Name AS PayerName
    ,SUM(
        CASE
            WHEN ClientFeeScheduleEntryID IS NOT NULL THEN
                    cf.AllowedAmount
                ELSE 
                    c1.BilledAmount * cp.CollectionPercentage
            END
    ) AS 'CashValue'
        
FROM IntegratedServices.ClaimStatusBatchClaims AS c1 
JOIN(
		SELECT * FROM [IntegratedServices].[fnGetClaimLevelGroups](
					@ClientId, 
					null, 
					null, 
					null, 
					@beginDate, 
					@endDate, 
					null, 
					null, 
					null, 
					null, 
					@ClientProviderIds, 
					@ClientLocationIds, 
					null, 
					null, 
					null, 
					null, 
					null, 
					null
				)
	) as c2 ON c1.ClaimLevelMd5Hash = c2.ClaimLevelMd5Hash
LEFT JOIN ClientFeeScheduleEntries AS cf ON cf.Id = c1.ClientFeeScheduleEntryId
JOIN ClientInsurances AS i ON c1.ClientInsuranceId = i.Id
--JOIN ClientLocations as loc ON loc.Id = c1.ClientLocationId
--JOIN Providers AS prv ON prv.Id = c1.ClientProviderId
LEFT JOIN ClientInsuranceAverageCollectionPercentages as cp on cp.ClientInsuranceId = i.Id

WHERE c1.ClientId = @ClientId
AND c1.IsSupplanted = 0
AND c1.IsDeleted = 0
AND c1.DateOfServiceFrom >= @beginDate --DATEADD(dd, @FilterForDays, DATEDIFF(dd, 0, GETDATE())) 
AND c1.DateOfServiceFrom <= @endDate --DATEADD(ms, -2, DATEADD(dd, 1, DATEDIFF(dd, 1, GetDate())))
AND (c1.ClientLocationId in (SELECT convert(int, value)
    FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
AND (c1.ClientProviderId in (SELECT convert(int, value)
    FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))
--AND (c1.ClientInsuranceId = @ClientInsuranceId OR (@ClientInsuranceId is NULL OR @ClientInsuranceId = 0))

GROUP BY c1.ClaimLevelMd5Hash, i.Id, i.Name, c1.DateOfServiceFrom
ORDER BY c1.DateOfServiceFrom DESC
    -- Having COUNT(c1.ClaimLevelMd5Hash) > 1

ELSE
    SELECT Count(c1.ClaimLevelMD5Hash) AS ClaimCount
    ,SUM(c1.BilledAmount) AS RevenueTotals, c1.ClaimLevelMd5Hash, c1.ClaimBilledOn as 'BilledDate', i.Id AS ClientInsuranceId, i.Name AS PayerName
    ,SUM(
        CASE
            WHEN ClientFeeScheduleEntryID IS NOT NULL THEN
                    cf.AllowedAmount
                ELSE 
                    c1.BilledAmount * cp.CollectionPercentage
            END
    ) AS 'CashValue'
        
FROM IntegratedServices.ClaimStatusBatchClaims AS c1 
JOIN(
		SELECT * FROM [IntegratedServices].[fnGetClaimLevelGroups](
					@ClientId, 
					null, 
					null, 
					null, 
					null, 
					null, 
					null, 
					null,
					@beginDate, 
					@endDate,  
					@ClientProviderIds, 
					@ClientLocationIds, 
					null, 
					null, 
					null, 
					null, 
					null, 
					null
				)
	) as c2 ON c1.ClaimLevelMd5Hash = c2.ClaimLevelMd5Hash
--JOIN(
--		SELECT max(ClaimBilledOn) as 'LatestClaimBilledOn', ClaimLevelMd5Hash as 'ClaimLevelMd5Hash'
--		FROM  [IntegratedServices].ClaimStatusBatchClaims 
--		GROUP BY ClaimLevelMd5Hash
--	) as c2 ON c1.ClaimBilledOn = c2.LatestClaimBilledOn AND c1.ClaimLevelMd5Hash = c2.ClaimLevelMd5Hash
LEFT JOIN ClientFeeScheduleEntries AS cf ON cf.Id = c1.ClientFeeScheduleEntryId
JOIN ClientInsurances AS i ON c1.ClientInsuranceId = i.Id
--JOIN ClientLocations as loc ON loc.Id = c1.ClientLocationId
--JOIN Providers AS prv ON prv.Id = c1.ClientProviderId
LEFT JOIN ClientInsuranceAverageCollectionPercentages as cp on cp.ClientInsuranceId = i.Id

WHERE c1.ClientId = @ClientId
AND c1.IsSupplanted = 0
AND c1.IsDeleted = 0
AND c1.ClaimBilledOn >= @beginDate --DATEADD(dd, @FilterForDays, DATEDIFF(dd, 0, GETDATE())) 
AND c1.ClaimBilledOn <= @endDate --DATEADD(ms, -2, DATEADD(dd, 1, DATEDIFF(dd, 1, GetDate())))
AND (c1.ClientLocationId in (SELECT convert(int, value)
    FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
AND (c1.ClientProviderId in (SELECT convert(int, value)
    FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))
--AND (c1.ClientInsuranceId = @ClientInsuranceId OR (@ClientInsuranceId is NULL OR @ClientInsuranceId = 0))
--AND (c1.ClientInsuranceId in (SELECT convert(int, value)
   -- FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))

GROUP BY c1.ClaimLevelMd5Hash, i.Id, i.Name, c1.ClaimBilledOn
ORDER BY c1.ClaimBilledOn DESC
    -- Having COUNT(c1.ClaimLevelMd5Hash) > 1
END
GO