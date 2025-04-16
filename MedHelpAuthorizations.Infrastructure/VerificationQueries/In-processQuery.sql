DECLARE @ClientId INT = NULL;
DECLARE @ClientLocationIds NVARCHAR(MAX) = NULL;
DECLARE @ClientInsuranceIds NVARCHAR(MAX) = NULL;
DECLARE @ClientAuthTypeIds NVARCHAR(MAX) = NULL;
DECLARE @ClientExceptionReasonCategoryIds NVARCHAR(MAX)=NULL;
DECLARE @ClientProcedureCodes NVARCHAR(MAX) = NULL;
DECLARE @DelimitedLineItemStatusIds NVARCHAR(MAX) = NULL;
DECLARE @ClientProviderIds NVARCHAR(MAX) = NULL;
DECLARE @PatientId INT = NULL;
DECLARE @ClaimStatusBatchId INT = NULL;
DECLARE @ReceivedFrom DATETIME = NULL;
DECLARE @ReceivedTo DATETIME = NULL;
DECLARE @DateOfServiceFrom DATETIME = NULL;
DECLARE @DateOfServiceTo DATETIME = NULL;
DECLARE @TransactionDateFrom DATETIME = NULL;
DECLARE @TransactionDateTo DATETIME = NULL;
DECLARE @ClaimBilledFrom DATETIME2 = NULL;
DECLARE @ClaimBilledTo DATETIME2 = NULL;

SET @ClientId = 3
SET @DelimitedLineItemStatusIds = '10,17'
SET @ClaimBilledFrom = '2022-03-17T00:00:00.0000000'
SET @ClaimBilledTo = '2024-03-17T00:00:00.0000000'

SELECT [claimStatusTransactions].[ExceptionReason], [persons].[LastName], [persons].[FirstName], [persons].[DateOfBirth], [claimStatusBatchClaims].[PolicyNumber], [authTypes].[Name], [clientInsurances].[LookupName], [claimStatusBatchClaims].[ClaimNumber], [claimStatusTransactions].[ClaimNumber], [claimStatusTransactions].[LineItemControlNumber], [claimStatusBatchClaims].[ProcedureCode], [claimStatusBatchClaims].[DateOfServiceFrom], [claimStatusBatchClaims].[DateOfServiceTo], [claimLineItemStatuses].[Description], [claimStatusTransactions].[ClaimLineItemStatusId], [claimStatusTransactions].[ClaimLineItemStatusValue], CASE
    WHEN [claimStatusExceptionReasonCategories].[Id] IS NOT NULL THEN [claimStatusExceptionReasonCategories].[Description]
    ELSE N''
END, [claimStatusTransactions].[ExceptionRemark], [claimStatusTransactions].[ReasonCode], [claimStatusBatchClaims].[ClaimBilledOn], COALESCE([claimStatusBatchClaims].[BilledAmount], 0.0), COALESCE([claimStatusTransactions].[TotalAllowedAmount], 0.0), COALESCE([claimStatusTransactions].[TotalNonAllowedAmount], 0.0), [claimStatusTransactions].[LineItemPaidAmount], [claimStatusTransactions].[CheckPaidAmount], [claimStatusTransactions].[CheckDate], [claimStatusTransactions].[CheckNumber], [claimStatusTransactions].[ReasonDescription], [claimStatusTransactions].[RemarkCode], [claimStatusTransactions].[RemarkDescription], [claimStatusTransactions].[CoinsuranceAmount], [claimStatusTransactions].[CopayAmount], [claimStatusTransactions].[DeductibleAmount], [claimStatusTransactions].[CobAmount], [claimStatusTransactions].[PenalityAmount], [claimStatusTransactions].[EligibilityStatus], [claimStatusTransactions].[EligibilityInsurance], [claimStatusTransactions].[EligibilityPolicyNumber], [claimStatusTransactions].[EligibilityFromDate], [claimStatusTransactions].[VerifiedMemberId], [claimStatusTransactions].[CobLastVerified], [claimStatusTransactions].[LastActiveEligibleDateRange], [claimStatusTransactions].[PrimaryPayer], [claimStatusTransactions].[PrimaryPolicyNumber], [claimStatusBatches].[BatchNumber], [claimStatusBatchClaims].[CreatedOn], CASE
    WHEN [claimStatusTransactions].[LastModifiedOn] IS NOT NULL THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END, [claimStatusTransactions].[LastModifiedOn], [claimStatusTransactions].[CreatedOn], [provider_persons].[Id], [provider_persons].[AddressId], [provider_persons].[ClientId], [provider_persons].[CreatedBy], [provider_persons].[CreatedOn], [provider_persons].[DateOfBirth], [provider_persons].[Email], [provider_persons].[FaxNumber], [provider_persons].[FirstName], [provider_persons].[GenderIdentityId], [provider_persons].[HomePhoneNumber], [provider_persons].[LastModifiedBy], [provider_persons].[LastModifiedOn], [provider_persons].[LastName], [provider_persons].[MiddleName], [provider_persons].[MobilePhoneNumber], [provider_persons].[OfficePhoneNumber], [provider_persons].[SocialSecurityNumber], [c5].[Name], [c5].[Npi], [claimStatusTransactions].[ClaimStatusBatchClaimId]

FROM [IntegratedServices].[ClaimStatusTransactions] AS [claimStatusTransactions]
INNER JOIN (
    SELECT [c0].[Id], [c0].[BilledAmount], [c0].[ClaimBilledOn], [c0].[ClaimNumber], [c0].[ClaimStatusBatchId], [c0].[ClientLocationId], [c0].[ClientProviderId], [c0].[CreatedOn], [c0].[DateOfServiceFrom], [c0].[DateOfServiceTo], [c0].[IsDeleted], [c0].[IsSupplanted], [c0].[PatientId], [c0].[PolicyNumber], [c0].[ProcedureCode]
    FROM [IntegratedServices].[ClaimStatusBatchClaims] AS [c0]
    WHERE [c0].[IsDeleted] = CAST(0 AS bit)
) AS [claimStatusBatchClaims] ON [claimStatusTransactions].[ClaimStatusBatchClaimId] = [claimStatusBatchClaims].[Id]
LEFT JOIN [dbo].[Patients] AS [patients] ON [claimStatusBatchClaims].[PatientId] = [patients].[Id]
LEFT JOIN [dbo].[Persons] AS [persons] ON [patients].[PersonId] = [persons].[Id]
INNER JOIN (
    SELECT [c1].[Id], [c1].[AuthTypeId], [c1].[BatchNumber], [c1].[ClientInsuranceId]
    FROM [IntegratedServices].[ClaimStatusBatches] AS [c1]
    WHERE [c1].[IsDeleted] = CAST(0 AS bit)
) AS [claimStatusBatches] ON [claimStatusBatchClaims].[ClaimStatusBatchId] = [claimStatusBatches].[Id]
LEFT JOIN [dbo].[AuthTypes] AS [authTypes] ON [claimStatusBatches].[AuthTypeId] = [authTypes].[Id]
INNER JOIN [dbo].[ClientInsurances] AS [clientInsurances] ON [claimStatusBatches].[ClientInsuranceId] = [clientInsurances].[Id]
LEFT JOIN [IntegratedServices].[ClaimLineItemStatuses] AS [claimLineItemStatuses] ON [claimStatusTransactions].[ClaimLineItemStatusId] = [claimLineItemStatuses].[Id]
LEFT JOIN [IntegratedServices].[ClaimStatusExceptionReasonCategories] AS [claimStatusExceptionReasonCategories] ON [claimStatusTransactions].[ClaimStatusExceptionReasonCategoryId] = [claimStatusExceptionReasonCategories].[Id]
LEFT JOIN [dbo].[Providers] AS [providers] ON [claimStatusBatchClaims].[ClientProviderId] = [providers].[Id]
LEFT JOIN [dbo].[Persons] AS [provider_persons] ON [providers].[PersonId] = [provider_persons].[Id]
LEFT JOIN [dbo].[ClientLocations] AS [c5] ON [claimStatusBatchClaims].[ClientLocationId] = [c5].[Id]
WHERE [claimStatusBatchClaims].[IsSupplanted] = CAST(0 AS bit) AND [claimStatusBatchClaims].[IsDeleted] = CAST(0 AS bit) AND [claimStatusTransactions].[IsDeleted] = CAST(0 AS bit) 
	  AND [claimStatusTransactions].[ClientId] = @ClientId AND CONVERT(date, [claimStatusBatchClaims].[ClaimBilledOn]) >= @ClaimBilledFrom 
	  AND CONVERT(date, [claimStatusBatchClaims].[ClaimBilledOn]) <= @ClaimBilledTo AND [claimStatusTransactions].[IsDeleted] = CAST(0 AS bit) 
	  AND ([claimStatusTransactions].Id = NULL OR ([claimStatusTransactions].ClaimLineItemStatusId IN (SELECT convert(int, value)
		  FROM string_split(@DelimitedLineItemStatusIds, ',')) OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = ''))) 
		
		--@ReceivedFrom and @ReceivedTo
		AND ((CONVERT(date, [claimStatusBatchClaims].CreatedOn) >= @ReceivedFrom) OR @ReceivedFrom IS NULL)
		AND ((CONVERT(date, [claimStatusBatchClaims].CreatedOn) <= @ReceivedTo) OR @ReceivedTo IS NULL)
		--@DateOfServiceFrom and @DateOfServiceTo
		AND ((CONVERT(date,[claimStatusBatchClaims].DateOfServiceFrom) >= @DateOfServiceFrom) OR @DateOfServiceFrom IS NULL)
		AND ((CONVERT(date,[claimStatusBatchClaims].DateOfServiceTo  )<= @DateOfServiceTo) OR @DateOfServiceTo IS NULL)
		--@ClaimBilledFrom and @ClaimBilledTo
		AND ((CONVERT(date,[claimStatusBatchClaims].ClaimBilledOn) >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
		AND ((CONVERT(date,[claimStatusBatchClaims].ClaimBilledOn) <= @ClaimBilledTo) OR @ClaimBilledTo is null)
		--@TransactionDateFrom and @TransactionDateTo
		--The COALESCE function is used to return the first non-null expression among its arguments. 
		AND ((CONVERT(date,COALESCE([claimStatusTransactions].LastModifiedOn, [claimStatusTransactions].CreatedOn)) >= @TransactionDateFrom) OR @TransactionDateFrom is NULL )
		AND ((CONVERT(date,COALESCE([claimStatusTransactions].LastModifiedOn, [claimStatusTransactions].CreatedOn)) <= @TransactionDateTo) OR @TransactionDateTo is NULL)
    
	    ---Multi Select Filter----		
		AND ([claimStatusTransactions].[ClaimLineItemStatusId] IN (SELECT convert(int, value)
		FROM string_split(@DelimitedLineItemStatusIds, ',')) OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = ''))

		And ([claimStatusBatches].[ClientInsuranceId] in (SELECT convert(int, value)
			FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))

		And ((@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = '') OR ([claimStatusTransactions].[ClaimStatusExceptionReasonCategoryId]
				IN (SELECT convert(int, value) FROM string_split(@ClientExceptionReasonCategoryIds, ','))))

		And ([claimStatusBatchClaims].[ClientLocationId] IN (SELECT convert(int, value)
			FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))

		And ([providers].Id in (SELECT convert(int, value)
			FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))

		And ([claimStatusBatches].[AuthTypeId] IN (SELECT convert(int, value)
			FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))

		And ([claimStatusBatchClaims].[ProcedureCode] IN (SELECT convert(nvarchar(16), value)
			FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))