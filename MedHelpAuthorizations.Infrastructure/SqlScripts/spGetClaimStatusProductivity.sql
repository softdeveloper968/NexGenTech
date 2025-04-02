SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [IntegratedServices].[spGetClaimStatusProductivity]
	@ClientId int
    ,@DelimitedLineItemStatusIds nvarchar(MAX) = NULL
	,@DateOfServiceFrom DateTime = NULL
	,@DateOfServiceTo DateTime = NULL
	,@ClaimBilledFrom DateTime = NULL
	,@ClaimBilledTo DateTime = NULL
	,@ClientInsuranceIds nvarchar(MAX)= null
	,@ClientExceptionReasonCategoryIds nvarchar(MAX)= null
	,@ClientAuthTypeIds nvarchar(MAX)= null
	,@ClientProcedureCodes nvarchar(MAX)= null
	,@ClientProviderIds nvarchar(MAX) = NULL
    ,@ClientLocationIds nvarchar(MAX) = NULL
AS
BEGIN
       SELECT 
        c1.DateOfServiceFrom
        ,c1.ClaimBilledOn   
        ,Count(c1.ClaimLevelMD5Hash) as [Quantity]
        ,SUM(c1.BilledAmount) as [ChargedSum]
        ,SUM(t.LineItemPaidAmount) as 'PaidAmountSum'
        ,SUM(t.TotalAllowedAmount) as 'AllowedAmountSum' 
        ,t.ClaimLineItemStatusId  as 'ClaimLineItemStatusId'
        ,t.ClaimStatusExceptionReasonCategoryId as 'ExceptionReasonCategoryId'
        ,c1.ProcedureCode
        ,a.Name as 'ServiceType'
        ,a.Id as 'ServiceTypeId'
        ,i.LookupName as 'ClientInsuranceLookupName'
    	,i.Id as 'ClientinsuranceId'
        ,c1.ClientLocationId as 'ClientLocationId'
        ,c1.ClientProviderId as 'ClientProviderId'
        ,Per.LastName + ',' + per.FirstName as 'ProviderName'
        ,ClientLoc.Name as 'LocationName'
        ,min(DATEADD(ww, DATEDIFF(ww, -1, c1.ClaimBilledOn), -1)) as FirstDateOfWeekBilledOn
		,min(DATEADD(ww, DATEDIFF(ww, -1, c1.DateOfServiceFrom), -1)) as FirstDateOfWeekServiceDate
		,DATEPART(ww, c1.ClaimBilledOn) as 'WeekNumberBilledOn'
		,DATEPART(ww, c1.DateOfServiceFrom) as 'WeekNumberServiceDate'
		,CASE
			WHEN month(min(c1.ClaimBilledOn)) = 1 
					AND DATEPART(ww, c1.ClaimBilledOn) >= 208
				THEN year(c1.ClaimBilledOn) - 4  
			WHEN month(min(c1.ClaimBilledOn)) = 1 
					AND DATEPART(ww, c1.ClaimBilledOn) >= 156
				THEN year(c1.ClaimBilledOn) - 3 
			WHEN month(min(c1.ClaimBilledOn)) = 1 
					AND DATEPART(ww, c1.ClaimBilledOn) >= 104
				THEN year(c1.ClaimBilledOn) - 2 
			WHEN month(min(c1.ClaimBilledOn)) = 1 
					AND DATEPART(ww, c1.ClaimBilledOn) >= 52
				THEN year(c1.ClaimBilledOn) - 1 
			WHEN month(min(c1.ClaimBilledOn)) = 12
					AND DATEPART(ww, c1.ClaimBilledOn) = 1
				THEN year(c1.ClaimBilledOn) + 1 
			ELSE year(c1.ClaimBilledOn)
		END as 'WeekYearBilledOn'
		,CASE
			WHEN month(min(c1.DateOfServiceFrom)) = 1 
					AND DATEPART(ww, c1.DateOfServiceFrom) >= 208
				THEN year(c1.DateOfServiceFrom) - 4  
			WHEN month(min(c1.DateOfServiceFrom)) = 1 
					AND DATEPART(ww, c1.DateOfServiceFrom) >= 156
				THEN year(c1.DateOfServiceFrom) - 3 
			WHEN month(min(c1.DateOfServiceFrom)) = 1 
					AND DATEPART(ww, c1.DateOfServiceFrom) >= 104
				THEN year(c1.DateOfServiceFrom) - 2 
			WHEN month(min(c1.DateOfServiceFrom)) = 1 
					AND DATEPART(ww, c1.DateOfServiceFrom) >= 52
				THEN year(c1.DateOfServiceFrom) - 1 
			WHEN month(min(c1.DateOfServiceFrom)) = 12
					AND DATEPART(ww, c1.DateOfServiceFrom) = 1
				THEN year(c1.DateOfServiceFrom) + 1 
			ELSE year(c1.DateOfServiceFrom) 
		END as 'WeekYearServiceDate'
    FROM [IntegratedServices].ClaimStatusBatchClaims as c1
	JOIN(
			SELECT * FROM [IntegratedServices].[fnGetClaimLevelGroups](
						@ClientId, 
						@DelimitedLineItemStatusIds, 
						null,
						null, 
						@DateOfServiceFrom, 
						@DateOfServiceTo, 
						null,
						null,
						@ClaimBilledFrom, 
						@ClaimBilledTo, 
						@ClientProviderIds, 
						@ClientLocationIds, 
						@ClientInsuranceIds, 
						@ClientExceptionReasonCategoryIds, 
						@ClientAuthTypeIds, 
						@ClientProcedureCodes, 
						null,
						null
					)
		) as c2 ON c1.ClaimLevelMd5Hash = c2.ClaimLevelMd5Hash
    LEFT JOIN [IntegratedServices].ClaimStatusTransactions as t ON t.ClaimStatusBatchClaimId = c1.Id
    LEFT JOIN [IntegratedServices].ClaimStatusBatches as b  ON c1.ClaimStatusBatchId = b.Id
    LEFT JOIN [dbo].AuthTypes as a ON b.AuthTypeId = a.Id 
    LEFT JOIN [dbo].ClientInsurances as i ON b.ClientInsuranceId = i.Id 

 	LEFT JOIN [dbo].Providers as Prv ON c1.ClientProviderId = Prv.Id
 	LEFT JOIN [dbo].Persons as Per On Prv.PersonId = Per.Id
 	LEFT JOIN [dbo].ClientLocations as ClientLoc On c1.ClientLocationId = ClientLoc.Id
    LEFT JOIN [IntegratedServices].ClaimLineItemStatuses as cs ON t.ClaimLineItemStatusId = cs.Id
    --JOIN [IntegratedServices].ClaimStatusExceptionReasonCategories as er ON t.ClaimStatusExceptionReasonCategoryId = er.ClaimStatusExceptionReasonCategoryId
    
    WHERE i.ClientId = @ClientId
        AND (c1.IsDeleted = 0)
		AND (t.ClaimLineItemStatusId IN (SELECT convert(int, value) FROM string_split(@DelimitedLineItemStatusIds, ',')) OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = ''))

        And (b.ClientInsuranceId in (SELECT convert(int, value)
			FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
		And (t.ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value)
			FROM string_split(@ClientExceptionReasonCategoryIds, ',')) OR (@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = ''))
		And (b.AuthTypeId in (SELECT convert(int, value)
			FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
		And (c1.ProcedureCode in (SELECT convert(nvarchar(16), value)
			FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))
			
		And (ClientLoc.Id in (SELECT convert(int, value)
			FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
		And (Prv.Id in (SELECT convert(int, value)
			FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))		

        --AND ((c1.DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom is NULL)
        --AND ((c1.DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo is NULL)			
        --AND ((c1.ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
        --AND ((c1.ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)
		AND c1.IsDeleted = 0 
        AND c1.IsSupplanted = 0
		

    GROUP BY i.LookupName,  c1.ProcedureCode, (Per.LastName + ',' + per.FirstName)
	, ClientLoc.Name, c1.DateOfServiceFrom, c1.ClaimBilledOn, t.ClaimLineItemStatusId, t.ClaimStatusExceptionReasonCategoryId 
	,a.Name ,a.Id ,i.Id, c1.ClientLocationId
    , c1.ClientProviderId, c1.ClaimLevelMD5Hash
END
GO
