SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER  PROCEDURE [IntegratedServices].[spGetClaimInProcessDateWise]
@ClientId int
    ,@DelimitedLineItemStatusIds nvarchar(MAX) = null
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
				i.id as 'ClientInsuranceId'
				,i.LookupName as 'ClientInsuranceName'
				,SUM(c1.BilledAmount) as 'ChargedSum'
				,COUNT(c1.ClaimLevelMd5Hash) as 'Quantity'

FROM [IntegratedServices].ClaimStatusBatchClaims as c1
JOIN(
		SELECT * FROM [IntegratedServices].[fnGetClaimLevelGroups](
					@ClientId, 
					null, 
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
	) as c2 ON c1.ClaimLevelMd5Hash = c2.ClaimLevelMd5Hash
	LEFT JOIN [IntegratedServices].ClaimStatusTransactions as claimStatusTransaction ON claimStatusTransaction.ClaimStatusBatchClaimId = c1.Id
	JOIN [IntegratedServices].ClaimStatusBatches as b ON c1.ClaimStatusBatchId = b.Id
	JOIN [dbo].ClientInsurances as i ON b.ClientInsuranceId = i.Id
	LEFT JOIN [IntegratedServices].ClaimLineItemStatuses as s ON claimStatusTransaction.ClaimLineItemStatusId = s.Id
	LEFT JOIN [dbo].Providers as Prv ON c1.ClientProviderId = Prv.Id
	LEFT JOIN [dbo].ClientLocations as ClientLoc On c1.ClientLocationId = ClientLoc.Id

WHERE C1.ClientId = @ClientId
	--AND (c1.ClaimStatusTransactionId is NULL OR claimStatusTransaction.ClaimLineItemStatusId in (10,17))
		AND ([claimStatusTransaction].Id IS NULL OR [claimStatusTransaction].ClaimLineItemStatusId IN (10,17))

		---Multi Select Filter----
		And (b.ClientInsuranceId in (SELECT convert(int, value)
			FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
		And ((@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = '') OR (claimStatusTransaction.ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value)
			FROM string_split(@ClientExceptionReasonCategoryIds, ','))))
		And (ClientLoc.Id in (SELECT convert(int, value)
			FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
		And (Prv.Id in (SELECT convert(int, value)
			FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))
		And (b.AuthTypeId in (SELECT convert(int, value)
			FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
		And (c1.ProcedureCode in (SELECT convert(nvarchar(16), value)
			FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))

		AND c1.IsDeleted = 0
		AND c1.IsSupplanted = 0

		AND ((c1.PatientId = @PatientId) OR (@PatientId IS NULL) OR (@PatientId = 0))
	    AND ((c1.ClaimStatusBatchId = @ClaimStatusBatchId) OR (@ClaimStatusBatchId IS NULL)  OR (@ClaimStatusBatchId = 0))	
		
	GROUP BY i.Id
		, i.LookupName
        ,c1.ClaimLevelMd5Hash

END
