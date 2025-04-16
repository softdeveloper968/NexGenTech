SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER  PROCEDURE [IntegratedServices].[spGetAverageDaysToPayByProvider]
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
		[person].LastName as 'Last Name',
		[person].FirstName as 'First Name',
		[person].[DateOfBirth] as 'DOB',
		[claimStatusBatchClaim].[PolicyNumber] AS 'Policy Number',
		[insurance].[LookUpName] as 'Payer Name',
		[claimStatusBatchClaim].[ClaimNumber] as 'Office Claim #',
		[claimStatusBatchClaim].[ProcedureCode] AS 'CPT Code',
		[provider].Id as 'ProviderId',
		[provider].Npi as 'ProviderNPI',
		[person].LastName+','+[person].FirstName as 'ProviderName'

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
	JOIN [IntegratedServices].ClaimStatusBatches as claimStatusBatch ON claimStatusBatchClaim.ClaimStatusBatchId = claimStatusBatch.Id
	JOIN [dbo].ClientInsurances as [insurance] ON [claimStatusBatch].ClientInsuranceId = [insurance].Id
	LEFT JOIN [IntegratedServices].ClaimStatusTransactions as claimStatusTransaction ON claimStatusTransaction.ClaimStatusBatchClaimId = claimStatusBatchClaim.Id
	LEFT JOIN IntegratedServices.ClaimLineItemStatuses claimLineItemStatus on claimLineItemStatus.Id = claimStatusTransaction.ClaimLineItemStatusId
	LEFT JOIN [dbo].Providers as [provider] ON claimStatusBatchClaim.ClientProviderId = [provider].Id
	LEFT JOIN [dbo].Persons as [person] On [provider].PersonId = [person].Id
	LEFT JOIN [dbo].ClientLocations as [location] On [claimStatusBatchClaim].ClientLocationId = [location].Id

WHERE claimStatusBatchClaim.ClientId = @ClientId
	    --Filter data based on paid ClaimStatus Type Id--
	    And ([claimLineItemStatus].ClaimStatusTypeId = 1)
		AND (claimStatusTransaction.ClaimLineItemStatusId IN (SELECT convert(int, value) FROM string_split(@DelimitedLineItemStatusIds, ',')) OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = ''))
		And (claimStatusBatch.ClientInsuranceId in (SELECT convert(int, value) FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
		And ((@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = '') OR (claimStatusTransaction.ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value) FROM string_split(@ClientExceptionReasonCategoryIds, ','))))
		And (claimStatusBatchClaim.ClientLocationId in (SELECT convert(int, value) FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
		And ([provider].Id in (SELECT convert(int, value) FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))
		And (claimStatusBatch.AuthTypeId in (SELECT convert(int, value) FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
		And (claimStatusBatchClaim.ProcedureCode in (SELECT convert(nvarchar(16), value) FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))

		AND claimStatusBatchClaim.IsDeleted = 0
		AND claimStatusBatchClaim.IsSupplanted = 0
		AND ((claimStatusBatchClaim.PatientId = @PatientId) OR (@PatientId IS NULL))
	    AND ((claimStatusBatchClaim.ClaimStatusBatchId = @ClaimStatusBatchId) OR (@ClaimStatusBatchId IS NULL)  OR (@ClaimStatusBatchId = 0))	
		AND [claimStatusTransaction].CheckDate IS NOT NULL

Group by
		[claimStatusBatchClaim].DateOfServiceFrom,[claimStatusBatchClaim].[ClaimBilledOn],[claimStatusTransaction].CheckDate,
		[person].LastName,[person].FirstName,[person].[DateOfBirth],[claimStatusBatchClaim].[PolicyNumber],
		[insurance].[LookUpName],[claimStatusBatchClaim].[ClaimNumber],[claimStatusBatchClaim].[ProcedureCode],[provider].Id,
		[provider].Npi

END