
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--AA-77 productivity dashboard date-time chart
CREATE OR ALTER PROCEDURE [IntegratedServices].[spGetClaimStatusUploadedChargeProductivityByDay]
	@ClientId int
    ,@DelimitedLineItemStatusIds nvarchar(MAX) = NULL
	--,@ClientInsuranceId int = NULL
	--,@AuthTypeId int = NULL 
	--,@ProcedureCode nvarchar(24) = NULL
	--,@ExceptionReasonCategory int = NULL
	,@DateOfServiceFrom DateTime = NULL
	,@DateOfServiceTo DateTime = NULL
	,@ClaimBilledFrom DateTime = NULL
	,@ClaimBilledTo DateTime = NULL
	--,@ClientProviderId int = NULL
 --   ,@ClientLocationId int = NULL
 
	,@ClientInsuranceIds nvarchar(MAX)= null
	,@ClientExceptionReasonCategoryIds nvarchar(MAX)= null
	,@ClientAuthTypeIds nvarchar(MAX)= null
	,@ClientProcedureCodes nvarchar(MAX)= null
	
    ,@ClientLocationIds nvarchar(MAX)= null
    ,@ClientProviderIds nvarchar(MAX)= null

AS
BEGIN
    SELECT c1.DateOfServiceFrom
        ,c1.ClaimBilledOn   
        ,COUNT(c1.ClaimLevelMD5Hash) as [Quantity]
        ,SUM(c1.BilledAmount) as [ChargedSum]
		,'PaidAmountSum' = 0.00
		,'ClaimLineItemStatusId' = null
		,'ExceptionReasonCategoryId'= null
        ,c1.ProcedureCode
        ,a.Name as 'ServiceType'
        ,a.Id as 'ServiceTypeId'
        ,i.LookupName as 'ClientInsuranceLookupName'
        ,i.Id as 'ClientinsuranceId'
		,Per.LastName + ',' + per.FirstName as 'ProviderName'
        ,ClientLoc.Name as 'LocationName'
        ,min(DATEADD(ww, DATEDIFF(ww, -1, c1.DateOfServiceFrom), -1)) as FirstDateOfWeekBilledOn
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
    FROM [IntegratedServices].ClaimStatusBatchClaims c1
	
	LEFT JOIN [IntegratedServices].ClaimStatusBatches as b  ON c1.ClaimStatusBatchId = b.Id
    LEFT JOIN [dbo].AuthTypes as a ON b.AuthTypeId = a.Id 
	LEFT JOIN [dbo].ClientInsurances as i ON b.ClientInsuranceId = i.Id 	
    LEFT join IntegratedServices.ClaimStatusTransactions as t on t.ClaimStatusBatchClaimId = c1.Id
	LEFT JOIN [dbo].Providers as Prv ON c1.ClientProviderId = Prv.Id
 	LEFT JOIN [dbo].Persons as Per On Prv.PersonId = Per.Id
 	LEFT JOIN [dbo].ClientLocations as ClientLoc On c1.ClientLocationId = ClientLoc.Id

	WHERE i.ClientId = @ClientId
		AND (c1.IsDeleted = 0)
		--AND ((b.ClientInsuranceId = @ClientInsuranceId) OR (@ClientInsuranceId = 0 OR @ClientInsuranceId is null))
		--AND (b.AuthTypeId = @AuthTypeId OR (@AuthTypeId = 0 Or @AuthTypeId is null))
		--AND (c1.ProcedureCode = @ProcedureCode OR (@ProcedureCode = '' OR @ProcedureCode is null))	
		
		------Multi Select Filter-------
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
			
		AND ((c1.DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom is NULL)
		AND ((c1.DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo is NULL)			
		AND ((c1.ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
		AND ((c1.ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)
		--AND (c1.ClientProviderId = @ClientProviderId OR (@ClientProviderId = 0 OR @ClientProviderId is null))
  --      AND (c1.ClientLocationId = @ClientLocationId OR (@ClientLocationId = 0 OR @ClientLocationId is null))
		AND c1.IsDeleted = 0 
        AND c1.IsSupplanted = 0

    GROUP BY c1.DateOfServiceFrom 
            ,c1.ClaimBilledOn  
            ,c1.ProcedureCode
            ,a.Name
            ,a.Id
            ,i.LookupName
            ,i.Id	
			,(Per.LastName + ',' + per.FirstName)
			, ClientLoc.Name
            , c1.ClaimLevelMD5Hash
	END
GO

