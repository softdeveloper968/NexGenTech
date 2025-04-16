SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [IntegratedServices].[spGetPayerReimbursementByLocation]
@ClientId int
    ,@DelimitedLineItemStatusIds nvarchar(MAX)
	,@ReceivedFrom DateTime = NULL
	,@ReceivedTo DateTime = NULL
	,@DateOfServiceFrom DateTime = NULL
	,@DateOfServiceTo DateTime = NULL
	,@TransactionDateFrom DateTime = NULL
	,@TransactionDateTo DateTime = NULL
	,@ClaimBilledFrom DateTime = NULL
	,@ClaimBilledTo DateTime = NULL
	,@ClientProviderIds nvarchar(MAX) = NULL
    ,@ClientLocationIds nvarchar(MAX) = NULL
	,@ClientInsuranceIds nvarchar(MAX)= null
	,@ClientExceptionReasonCategoryIds nvarchar(MAX)= null
	,@ClientAuthTypeIds nvarchar(MAX)= null
	,@ClientProcedureCodes nvarchar(MAX)= null
    ,@PatientId int = NULL
	,@ClaimStatusBatchId int = NULL
WITH RECOMPILE
AS
BEGIN
SELECT	
		clientLocation.[Name] as 'ClientLocationName'
		,clientLocation.Id as 'ClientLocationId'
		,SUM(claimStatusTransaction.TotalAllowedAmount) as 'AllowedAmount'
		,SUM(claimStatusTransaction.LineItemPaidAmount) as 'PaidAmount'
		,COUNT( claimStatusBatchClaim.ClaimLevelMd5Hash) as 'Quantity'
		,clientInsurance.id as 'ClientInsuranceId'
		,clientInsurance.LookupName as 'ClientInsuranceName'
		,claimStatusBatchClaim.ClaimLevelMd5Hash

FROM [IntegratedServices].ClaimStatusBatchClaims as claimStatusBatchClaim
JOIN(
		SELECT * FROM [IntegratedServices].[fnGetClaimLevelGroups](
					@ClientId, 
					@DelimitedLineItemStatusIds, 
					@ReceivedFrom,
					@ReceivedTo, 
					@DateOfServiceFrom, 
					@DateOfServiceTo, 
					@TransactionDateFrom, 
					@TransactionDateTo, 
					@ClaimBilledFrom, 
					@ClaimBilledTo, 
					@ClientProviderIds, 
					@ClientLocationIds, 
					@ClientInsuranceIds, 
					@ClientExceptionReasonCategoryIds, 
					@ClientAuthTypeIds, 
					@ClientProcedureCodes, 
					@PatientId, 
					@ClaimStatusBatchId 
				)
	) as c2 ON claimStatusBatchClaim.ClaimLevelMd5Hash = c2.ClaimLevelMd5Hash
	LEFT JOIN [IntegratedServices].ClaimStatusTransactions as claimStatusTransaction ON claimStatusTransaction.ClaimStatusBatchClaimId = claimStatusBatchClaim.Id
	JOIN [IntegratedServices].ClaimStatusBatches as claimStatusBatch ON claimStatusBatchClaim.ClaimStatusBatchId = claimStatusBatch.Id
	JOIN [dbo].ClientInsurances as clientInsurance ON claimStatusBatch.ClientInsuranceId = clientInsurance.Id
	LEFT JOIN [IntegratedServices].ClaimLineItemStatuses as claimLineItemStatus ON claimStatusTransaction.ClaimLineItemStatusId = claimLineItemStatus.Id
	LEFT JOIN [dbo].ClientLocations as clientLocation ON clientLocation.Id = claimStatusBatchClaim.ClientLocationId

	WHERE claimStatusBatchClaim.ClientId = @ClientId
	AND (claimLineItemStatus.ClaimStatusTypeId = 1 AND (claimStatusTransaction.ClaimLineItemStatusId IN (SELECT convert(int, value)
			FROM string_split(@DelimitedLineItemStatusIds, ',')) OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = '')))

	--AND (claimStatusTransaction.ClaimLineItemStatusId IN (SELECT convert(int, value)
		--FROM string_split(@DelimitedLineItemStatusIds, ',')) OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = ''))

		---Multi Select Filter----
		And (claimStatusBatch.ClientInsuranceId in (SELECT convert(int, value)
			FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
		And ((@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = '') OR (claimStatusTransaction.ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value)
			FROM string_split(@ClientExceptionReasonCategoryIds, ','))))
		And (claimStatusBatchClaim.ClientLocationId in (SELECT convert(int, value)
			FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
		And (claimStatusBatchClaim.ClientProviderId in (SELECT convert(int, value)
			FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))
		And (claimStatusBatch.AuthTypeId in (SELECT convert(int, value)
			FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
		And (claimStatusBatchClaim.ProcedureCode in (SELECT convert(nvarchar(16), value)
			FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))
		AND ((claimStatusBatchClaim.CreatedOn >= @ReceivedFrom) OR @ReceivedFrom IS NULL)
		AND ((claimStatusBatchClaim.CreatedOn <= @ReceivedTo) OR @ReceivedTo IS NULL)
		AND ((claimStatusBatchClaim.DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom IS NULL)
		AND ((claimStatusBatchClaim.DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo IS NULL)
		AND ((claimStatusBatchClaim.ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
		AND ((claimStatusBatchClaim.ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)
		AND ((COALESCE(claimStatusTransaction.LastModifiedOn, claimStatusTransaction.CreatedOn) >= @TransactionDateFrom) OR @TransactionDateFrom is NULL )
		AND ((COALESCE(claimStatusTransaction.LastModifiedOn, claimStatusTransaction.CreatedOn) <= @TransactionDateTo) OR @TransactionDateTo is NULL)
		AND claimStatusBatchClaim.IsDeleted = 0
		AND claimStatusBatchClaim.IsSupplanted = 0
		AND ((claimStatusBatchClaim.PatientId = @PatientId) OR (@PatientId IS NULL))
	    AND ((claimStatusBatchClaim.ClaimStatusBatchId = @ClaimStatusBatchId) OR (@ClaimStatusBatchId IS NULL)  OR (@ClaimStatusBatchId = 0))	

		GROUP BY clientLocation.[Name]
		, clientLocation.Id
		, clientInsurance.Id
		, clientInsurance.LookupName, claimStatusBatchClaim.ClaimLevelMd5Hash
	END
