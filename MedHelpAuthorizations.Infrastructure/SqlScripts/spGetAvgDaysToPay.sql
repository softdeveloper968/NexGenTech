SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER  PROCEDURE [IntegratedServices].[spGetAvgDaysToPay]
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
	,@ClaimStatusBatchId int = NULL
WITH RECOMPILE
AS
BEGIN

SELECT 
		CONVERT(VARCHAR, [claimStatusBatchClaim].DateOfServiceFrom, 101) as 'DateOfServiceFrom',
		CONVERT(VARCHAR, [claimStatusBatchClaim].[ClaimBilledOn], 101) as 'ClaimBilledOn',
		CONVERT(VARCHAR, [claimStatusTransaction].CheckDate, 101) as CheckDate,
		'Avg Days to Pay' = ABS(DATEDIFF(DAY, [claimStatusTransaction].CheckDate, ClaimBilledOn)),
		'Avg Days to Bill' = ABS(DATEDIFF(DAY, claimStatusBatchClaim.DateOfServiceFrom, ClaimBilledOn)),
		'Total Days From DOS to Pay' = ABS(DATEDIFF(DAY, claimStatusBatchClaim.DateOfServiceFrom, [claimStatusTransaction].CheckDate)), 
		[person].LastName as 'Last Name',
		[person].FirstName as 'First Name',
		[person].[DateOfBirth] as 'DOB',
		[claimStatusBatchClaim].[PolicyNumber] AS 'Policy Number',
		[insurance].[LookUpName] as 'Payer Name',
		[claimStatusBatchClaim].[ClaimNumber] as 'Office Claim #',
		[claimStatusBatchClaim].[ProcedureCode] AS 'CPT Code'

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
	) as claimLevelGroup ON claimStatusBatchClaim.ClaimLevelMd5Hash = claimLevelGroup.ClaimLevelMd5Hash
	JOIN IntegratedServices.ClaimStatusBatches as claimStatusBatch 
    ON claimStatusBatchClaim.ClaimStatusBatchId = claimStatusBatch.Id

	JOIN dbo.ClientInsurances as insurance 
		ON claimStatusBatch.ClientInsuranceId = insurance.Id

	LEFT JOIN IntegratedServices.ClaimStatusTransactions as claimStatusTransaction 
		ON claimStatusTransaction.ClaimStatusBatchClaimId = claimStatusBatchClaim.Id

	LEFT JOIN dbo.Providers as provider 
		ON claimStatusBatchClaim.ClientProviderId = provider.Id

	LEFT JOIN dbo.Persons as person 
		ON provider.PersonId = person.Id

WHERE 
    claimStatusBatchClaim.ClientId = @ClientId
    AND claimStatusBatchClaim.IsDeleted = 0
    AND claimStatusBatchClaim.IsSupplanted = 0
    AND (claimStatusTransaction.CheckDate IS NOT NULL)

    -- Filter data based on claim line item status type ID (moved to WHERE)
	AND (claimStatusTransaction.ClaimLineItemStatusId = 1)

    -- Filter using string_split for dynamic filtering (unchanged)
    AND (claimStatusTransaction.ClaimLineItemStatusId IN (SELECT convert(int, value) FROM string_split(@DelimitedLineItemStatusIds, ',')) OR (@DelimitedLineItemStatusIds IS NULL OR @DelimitedLineItemStatusIds = ''))
    AND (claimStatusBatch.ClientInsuranceId IN (SELECT convert(int, value) FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds IS NULL OR @ClientInsuranceIds = ''))
    AND ((@ClientExceptionReasonCategoryIds IS NULL OR @ClientExceptionReasonCategoryIds = '') 
        OR (claimStatusTransaction.ClaimStatusExceptionReasonCategoryId IN (SELECT convert(int, value) FROM string_split(@ClientExceptionReasonCategoryIds, ','))))
    AND (claimStatusBatchClaim.ClientLocationId IN (SELECT convert(int, value) FROM string_split(@ClientLocationIds, ',')) 
        OR (@ClientLocationIds IS NULL OR @ClientLocationIds = ''))
    AND (claimStatusBatchClaim.ClientProviderId IN (SELECT convert(int, value) FROM string_split(@ClientProviderIds, ',')) 
        OR (@ClientProviderIds IS NULL OR @ClientProviderIds = ''))
    AND (claimStatusBatch.AuthTypeId IN (SELECT convert(int, value) FROM string_split(@ClientAuthTypeIds, ',')) 
        OR (@ClientAuthTypeIds IS NULL OR @ClientAuthTypeIds = ''))
    AND (claimStatusBatchClaim.ProcedureCode IN (SELECT convert(nvarchar(16), value) FROM string_split(@ClientProcedureCodes, ',')) 
        OR (@ClientProcedureCodes IS NULL OR @ClientProcedureCodes = ''))

    -- Additional filter by patient and claim batch id
    AND ((claimStatusBatchClaim.PatientId = @PatientId) OR (@PatientId IS NULL))
    AND ((claimStatusBatchClaim.ClaimStatusBatchId = @ClaimStatusBatchId) 
        OR (@ClaimStatusBatchId IS NULL) OR (@ClaimStatusBatchId = 0))

END