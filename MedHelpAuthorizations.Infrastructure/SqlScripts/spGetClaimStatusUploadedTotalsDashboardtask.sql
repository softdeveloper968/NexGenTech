SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--AA-77 
CREATE OR ALTER PROCEDURE [IntegratedServices].[spGetClaimStatusUploadedTotalsDashboardtask]
	 @ClientId int
	, @DelimitedLineItemStatusIds nvarchar(MAX) = NULL 
	, @ReceivedFrom DateTime = NULL 
	, @ReceivedTo DateTime = NULL 
	, @DateOfServiceFrom DateTime = NULL 
	, @DateOfServiceTo DateTime = NULL 
	, @TransactionDateFrom DateTime = NULL 
	, @TransactionDateTo DateTime = NULL 
	, @ClaimBilledFrom DateTime = NULL 
	, @ClaimBilledTo DateTime = NULL 

	,@ClientInsuranceIds nvarchar(MAX)= null
	,@ClientExceptionReasonCategoryIds nvarchar(MAX)= null
	,@ClientAuthTypeIds nvarchar(MAX)= null
	,@ClientProcedureCodes nvarchar(MAX)= null	
    ,@ClientLocationIds nvarchar(MAX)= null
    ,@ClientProviderIds nvarchar(MAX)= null
AS
BEGIN

   SELECT  i.LookupName as 'ClientInsuranceName'
        ,'ClaimLineItemStatus' = 'Received/Upload'
		,'ClaimStatusExceptionReasonCategory' = 'ClaimStatusExceptionReasonCategory'
		,TRIM(c1.[ProcedureCode]) as 'ProcedureCode'
        ,COUNT(c1.ClaimLevelMD5Hash) as 'Quantity'
        ,SUM(c1.BilledAmount) as 'ChargedSum'
        ,'AllowedAmountSum' = 0.00
        ,'NonAllowedAmountSum' = 0.00
		,'PaidAmountSum' = 0.00
        ,Per.LastName + ',' + per.FirstName as 'ProviderName'
        ,ClientLoc.Name as 'LocationName'
        FROM [IntegratedServices].ClaimStatusBatchClaims as c1       

 JOIN [IntegratedServices].ClaimStatusBatches as b  ON c1.ClaimStatusBatchId = b.Id
 JOIN [dbo].ClientInsurances as i ON b.ClientInsuranceId = i.Id 
 LEFT JOIN [IntegratedServices].ClaimStatusTransactions as t on t.Id = c1.ClaimStatusTransactionId
 LEFT JOIN [dbo].Providers as Prv ON c1.ClientProviderId = Prv.Id
 LEFT JOIN [dbo].Persons as Per On Prv.PersonId = Per.Id
 LEFT JOIN [dbo].ClientLocations as ClientLoc On ClientLoc.Id = c1.ClientLocationId

 WHERE c1.ClientId = @ClientId
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

		AND ((c1.CreatedOn >= @ReceivedFrom) OR @ReceivedFrom IS NULL)
		AND ((c1.CreatedOn <= @ReceivedTo) OR @ReceivedTo IS NULL)
		AND ((c1.DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom is NULL)
		AND ((c1.DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo is NULL)			
		AND ((c1.ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
		AND ((c1.ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)

        --AND (c1.ClientProviderId = @ClientProviderId OR (@ClientProviderId = 0 OR @ClientProviderId is null))
        --AND (c1.ClientLocationId = @ClientLocationId OR (@ClientLocationId = 0 OR @ClientLocationId is null))
		AND c1.IsDeleted = 0 
        AND c1.IsSupplanted = 0
GROUP BY i.LookupName, c1.ProcedureCode
    	,(Per.LastName + ',' + per.FirstName)    
		, ClientLoc.[Name]
        , c1.ClaimLevelMD5Hash
END
GO
