/****** Object:  UserDefinedFunction [IntegratedServices].[fnGetClaimLevelGroups]    Script Date: 04/19/2024 9:35:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER FUNCTION [IntegratedServices].[fnGetClaimLevelGroups](
	@ClientId int
    ,@DelimitedLineItemStatusIds nvarchar(MAX) = NULL
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
	,@ClaimStatusBatchId int = NULL)

RETURNS TABLE
AS
RETURN
	SELECT max(ClaimBilledOn) as 'LatestClaimBilledOn', ClaimLevelMd5Hash as 'ClaimLevelMd5Hash'
	FROM  [IntegratedServices].ClaimStatusBatchClaims bc
		LEFT JOIN [IntegratedServices].ClaimStatusTransactions as t ON t.ClaimStatusBatchClaimId = bc.Id
		JOIN [IntegratedServices].ClaimStatusBatches as b ON bc.ClaimStatusBatchId = b.Id
		JOIN [dbo].ClientInsurances as i ON b.ClientInsuranceId = i.Id
		LEFT JOIN [IntegratedServices].ClaimLineItemStatuses as s ON t.ClaimLineItemStatusId = s.Id
		--LEFT JOIN [dbo].Providers as Prv ON bc.ClientProviderId = Prv.Id
		--LEFT JOIN [dbo].Persons as Per On Prv.PersonId = Per.Id
		--LEFT JOIN [dbo].ClientLocations as ClientLoc On bc.ClientLocationId = ClientLoc.Id
		LEFT JOIN [IntegratedServices].ClaimStatusExceptionReasonCategories as er ON t.ClaimStatusExceptionReasonCategoryId = er.Id
	WHERE (@ClientId = -1 OR bc.ClientId = @ClientId)
		AND (t.ClaimLineItemStatusId IN (SELECT convert(int, value)
			FROM string_split(@DelimitedLineItemStatusIds, ',')) OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = ''))
		---Multi Select Filter----
		And (b.ClientInsuranceId in (SELECT convert(int, value)
			FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
		And ((@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = '') OR (t.ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value)
			FROM string_split(@ClientExceptionReasonCategoryIds, ','))))
		And (bc.ClientLocationId in (SELECT convert(int, value)
			FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
		And (bc.ClientProviderId in (SELECT convert(int, value)
			FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))
		And (b.AuthTypeId in (SELECT convert(int, value)
			FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
		And (bc.ProcedureCode in (SELECT convert(nvarchar(16), value)
			FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))
		AND ((bc.CreatedOn >= @ReceivedFrom) OR @ReceivedFrom IS NULL)
		AND ((bc.CreatedOn <= @ReceivedTo) OR @ReceivedTo IS NULL)
		AND ((bc.DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom IS NULL)
		AND ((bc.DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo IS NULL)
		AND ((bc.ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
		AND ((bc.ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)
		AND ((COALESCE(t.LastModifiedOn, t.CreatedOn) >= @TransactionDateFrom) OR @TransactionDateFrom is NULL )
		AND ((COALESCE(t.LastModifiedOn, t.CreatedOn) <= @TransactionDateTo) OR @TransactionDateTo is NULL)
		AND bc.IsDeleted = 0
		AND bc.IsSupplanted = 0
		AND ((bc.PatientId = @PatientId) OR (@PatientId IS NULL))
		AND ((bc.ClaimStatusBatchId = @ClaimStatusBatchId) OR (@ClaimStatusBatchId IS NULL)  OR (@ClaimStatusBatchId = 0))	
	GROUP BY ClaimLevelMd5Hash;
