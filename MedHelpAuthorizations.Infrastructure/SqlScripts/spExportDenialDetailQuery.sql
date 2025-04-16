CREATE OR ALTER   PROCEDURE [dbo].[spExportDenialDetailQuery]
	@ClientId INT,
    @ClientLocationIds NVARCHAR(MAX) = NULL,
    @ClientInsuranceIds NVARCHAR(MAX) = NULL,
    @ClientAuthTypeIds NVARCHAR(MAX) = NULL,
    @ClientExceptionReasonCategoryIds NVARCHAR(MAX) = NULL,
    @ClientProcedureCodes NVARCHAR(MAX) = NULL,
    @DelimitedLineItemStatusIds NVARCHAR(MAX) = NULL,
    @ClientProviderIds NVARCHAR(MAX) = NULL,
    @PatientId INT = NULL,
    @ClaimStatusBatchId INT = NULL,
    @ReceivedFrom DATETIME = NULL,
    @ReceivedTo DATETIME = NULL,
    @DateOfServiceFrom DATETIME = NULL,
    @DateOfServiceTo DATETIME = NULL,
    @TransactionDateFrom DATETIME = NULL,
    @TransactionDateTo DATETIME = NULL,
    @ClaimBilledFrom DATETIME = NULL,
    @ClaimBilledTo DATETIME = NULL
AS
BEGIN

SELECT 
	  [claimStatusTransactions].[Id], 
	  [claimStatusTransactions].[AuthorizationFound], 
	  [claimStatusTransactions].[AuthorizationNumber], 
	  [claimStatusTransactions].[AuthorizationStatusId],
	  [claimStatusTransactions].[BillingProviderNpi], 
	  [claimStatusTransactions].[CheckDate], 
	  [claimStatusTransactions].[CheckNumber], 
	  [claimStatusTransactions].[CheckPaidAmount],
	  [claimStatusTransactions].[ClaimLineItemStatusId], 
	  [claimStatusTransactions].[ClaimLineItemStatusValue], 
	  [claimStatusTransactions].[ClaimNumber], 
	  [claimStatusTransactions].[ClaimStatusBatchClaimId],
	  [claimStatusTransactions].[ClaimStatusExceptionReasonCategoryId],
	  [claimStatusTransactions].[ClaimStatusTransactionBeginDateTimeUtc], 
	  [claimStatusTransactions].[ClaimStatusTransactionEndDateTimeUtc], 
	  [claimStatusTransactions].[ClaimStatusTransactionLineItemStatusChangẹId],
	  [claimStatusTransactions].[ClientId], 
	  [claimStatusTransactions].[CobAmount],
	  [claimStatusTransactions].[CobLastVerified], 
	  [claimStatusTransactions].[CobaInsurerEffective], 
	  [claimStatusTransactions].[CobaInsurerName], 
	  [claimStatusTransactions].[CoinsuranceAmount],
	  [claimStatusTransactions].[CopayAmount], 
	  [claimStatusTransactions].[CreatedBy], 
	  [claimStatusTransactions].[CreatedOn], 
	  [claimStatusTransactions].[CurrentCoverage], 
	  [claimStatusTransactions].[DateClaimFinalized], 
	  [claimStatusTransactions].[DateEntered], 
	  [claimStatusTransactions].[DatePaid], 
	  [claimStatusTransactions].[DateReceived], 
	  [claimStatusTransactions].[DeductibleAmount],
	  [claimStatusTransactions].[DiagnosisCode], 
	  [claimStatusTransactions].[DiagnosisDescription], 
	  [claimStatusTransactions].[EligibilityFromDate],
	  [claimStatusTransactions].[EligibilityInsurance],
	  [claimStatusTransactions].[EligibilityPhone], 
	  [claimStatusTransactions].[EligibilityPolicyNumber], 
	  [claimStatusTransactions].[EligibilityStatus],
	  [claimStatusTransactions].[EligibilityUrl],
	  [claimStatusTransactions].[ExceptionReason],
	  [claimStatusTransactions].[ExceptionRemark], 
	  [claimStatusTransactions].[HippaStatus],
	  [claimStatusTransactions].[Icn],
	  [claimStatusTransactions].[InputDataFileName],
	  [claimStatusTransactions].[InputDataListIndex], 
	  [claimStatusTransactions].[IsDeleted],
	  [claimStatusTransactions].[LastActiveEligibleDateRange],
	  [claimStatusTransactions].[LastModifiedBy], 
	  [claimStatusTransactions].[LastModifiedOn],
	  [claimStatusTransactions].[LineItemChargeAmount],
	  [claimStatusTransactions].[LineItemControlNumber],
	  [claimStatusTransactions].[LineItemPaidAmount],
	  [claimStatusTransactions].[OtCapUsedAmount],
	  [claimStatusTransactions].[OtCapYearFrom],
	  [claimStatusTransactions].[OtCapYearTo],
	  [claimStatusTransactions].[PartA_DeductibleFrom],
	  [claimStatusTransactions].[PartA_DeductibleToDate],
	  [claimStatusTransactions].[PartA_EligibilityFrom],
	  [claimStatusTransactions].[PartA_EligibilityTo],
	  [claimStatusTransactions].[PartA_RemainingDeductible],
	  [claimStatusTransactions].[PartB_DeductibleFrom],
	  [claimStatusTransactions].[PartB_DeductibleTo],
	  [claimStatusTransactions].[PartB_EligibilityFrom],
	  [claimStatusTransactions].[PartB_EligibilityTo],
	  [claimStatusTransactions].[PartB_RemainingDeductible], 
	  [claimStatusTransactions].[PaymentType],
	  [claimStatusTransactions].[PenalityAmount],
	  [claimStatusTransactions].[PlanType], 
	  [claimStatusTransactions].[PrimaryPayer], 
	  [claimStatusTransactions].[PrimaryPolicyNumber],
	  [claimStatusTransactions].[PtCapUsedAmount], 
	  [claimStatusTransactions].[PtCapYearFrom],
	  [claimStatusTransactions].[PtCapYearTo], 
	  [claimStatusTransactions].[ReasonCode], 
	  [claimStatusTransactions].[ReasonDescription], 
	  [claimStatusTransactions].[ReferringProviderName],
	  [claimStatusTransactions].[RemarkCode],
	  [claimStatusTransactions].[RemarkDescription],
	  [claimStatusTransactions].[ServiceLineDenialReason], 
	  [claimStatusTransactions].[TotalAllowedAmount], 
	  [claimStatusTransactions].[TotalClaimChargeAmount], 
	  [claimStatusTransactions].[TotalClaimPaidAmount],
	  [claimStatusTransactions].[TotalClaimStatusId], 
	  [claimStatusTransactions].[TotalClaimStatusValue],
	  [claimStatusTransactions].[TotalMemberResponsibilityAmount],
	  [claimStatusTransactions].[TotalNonAllowedAmount],
	  [claimStatusTransactions].[VerifiedMemberId],
	  [claimStatusTransactions].[WriteoffAmount],
	  [claimStatusBatchClaims].[Id],
	  [claimStatusBatchClaims].[BilledAmount],
	  [claimStatusBatchClaims].[CalculatedLookupHash], 
	  [claimStatusBatchClaims].[CalculatedLookupHashInput],
	  [claimStatusBatchClaims].[ClaimBilledOn], 
	  [claimStatusBatchClaims].[ClaimLevelMd5Hash],
	  [claimStatusBatchClaims].[ClaimNumber],
	  [claimStatusBatchClaims].[ClaimStatusBatchClaimRootId], 
	  [claimStatusBatchClaims].[ClaimStatusBatchId],
	  [claimStatusBatchClaims].[ClaimStatusTransactionId], 
	  [claimStatusBatchClaims].[ClientCptCodeId],
	  [claimStatusBatchClaims].[ClientFeeScheduleEntryId],
	  [claimStatusBatchClaims].[ClientId],
	  [claimStatusBatchClaims].[ClientInsuranceId], 
	  [claimStatusBatchClaims].[ClientLocationId],
	  [claimStatusBatchClaims].[ClientProviderId], 
	  [claimStatusBatchClaims].[CreatedBy], 
	  [claimStatusBatchClaims].[CreatedOn], 
	  [claimStatusBatchClaims].[DateOfBirth],
	  [claimStatusBatchClaims].[DateOfServiceFrom], 
	  [claimStatusBatchClaims].[DateOfServiceTo], 
	  [claimStatusBatchClaims].[EntryMd5Hash],
	  [claimStatusBatchClaims].[GroupNpi],
	  [claimStatusBatchClaims].[IsDeleted], 
	  [claimStatusBatchClaims].[IsSupplanted],
	  [claimStatusBatchClaims].[LastModifiedBy], 
	  [claimStatusBatchClaims].[LastModifiedOn], 
	  [claimStatusBatchClaims].[Modifiers],
	  [claimStatusBatchClaims].[NormalizedClaimNumber], 
	  [claimStatusBatchClaims].[OriginalClaimBilledOn], 
	  [claimStatusBatchClaims].[PatientFirstName],
	  [claimStatusBatchClaims].[PatientId],
	  [claimStatusBatchClaims].[PatientLastName],
	  [claimStatusBatchClaims].[PolicyNumber], 
	  [claimStatusBatchClaims].[PolicyNumberUpdatedOn],
	  [claimStatusBatchClaims].[ProcedureCode], 
	  [claimStatusBatchClaims].[Quantity], 
	  [claimStatusBatchClaims].[RenderingNpi], 
	  [claimStatusBatchClaims].[WriteOffAmount],
	  [claimStatusBatches].[Id], 
	  [claimStatusBatches].[AbortedOnUtc],
	  [claimStatusBatches].[AbortedReason], 
	  [claimStatusBatches].[AllClaimStatusesResolvedOrExpired],
	  [claimStatusBatches].[AssignedClientRpaConfigurationId], 
	  [claimStatusBatches].[AssignedDateTimeUtc],
	  [claimStatusBatches].[AssignedToHostName], 
	  [claimStatusBatches].[AssignedToIpAddress], 
	  [claimStatusBatches].[AssignedToRpaCode], 
	  [claimStatusBatches].[AssignedToRpaLocalProcessIds], 
	  [claimStatusBatches].[AuthTypeId], 
	  [claimStatusBatches].[BatchNumber],
	  [claimStatusBatches].[ClientId],
	  [claimStatusBatches].[ClientInsuranceId], 
	  [claimStatusBatches].[CompletedDateTimeUtc], 
	  [claimStatusBatches].[CreatedBy],
	  [claimStatusBatches].[CreatedOn],
	  [claimStatusBatches].[InputDocumentId],
	  [claimStatusBatches].[IsDeleted],
	  [claimStatusBatches].[LastModifiedBy],
	  [claimStatusBatches].[LastModifiedOn],
	  [claimStatusBatches].[Priority],
	  [claimStatusBatches].[ReviewedBy],
	  [claimStatusBatches].[ReviewedOnUtc],
	  [claimStatusBatches].[RpaInsuranceId],
	  [authTypes].[Id], 
	  [authTypes].[CreatedBy],
	  [authTypes].[CreatedOn],
	  [authTypes].[Description], 
	  [authTypes].[LastModifiedBy],
	  [authTypes].[LastModifiedOn],
	  [authTypes].[Name], 
	  [clientInsurances].[Id],
	  [clientInsurances].[ClientId], 
	  [clientInsurances].[CreatedBy],
	  [clientInsurances].[CreatedOn],
	  [clientInsurances].[PayerIdentifier], 
	  [clientInsurances].[ExternalId],
	  [clientInsurances].[FaxNumber],
	  [clientInsurances].[LastModifiedBy],
	  [clientInsurances].[LastModifiedOn],
	  [clientInsurances].[LookupName],
	  [clientInsurances].[Name], 
	  [clientInsurances].[PhoneNumber],
	  [clientInsurances].[RpaInsuranceId], 
	  [rpaInsurances].[Id],
	  [rpaInsurances].[ApprovalWaitPeriodDays],
	  [rpaInsurances].[ClaimBilledOnWaitDays],
	  [rpaInsurances].[Code], 
	  [rpaInsurances].[CreatedBy], 
	  [rpaInsurances].[CreatedOn],
	  [rpaInsurances].[InactivatedOn], 
	  [rpaInsurances].[IsDeleted],
	  [rpaInsurances].[LastModifiedBy],
	  [rpaInsurances].[LastModifiedOn], 
	  [rpaInsurances].[Name], 
	  [rpaInsurances].[RpaInsuranceGroupId], 
	  [rpaInsurances].[TargetUrl],
	  [clientLocation].[Id],
	  [clientLocation].[AddressId],
	  [clientLocation].[ClientId],
	  [clientLocation].[CreatedBy], 
	  [clientLocation].[CreatedOn], 
	  [clientLocation].[EligibilityLocationId], 
	  [clientLocation].[ExternalId],
	  [clientLocation].[LastModifiedBy], 
	  [clientLocation].[LastModifiedOn],
	  [clientLocation].[Name],
	  [clientLocation].[Npi], 
	  [clientLocation].[OfficeFaxNumber],
	  [clientLocation].[OfficePhoneNumber], 
	  [claimStatusExceptionReasonCategories].[Id],
	  [claimStatusExceptionReasonCategories].[Code], 
	  [claimStatusExceptionReasonCategories].[CreatedBy], 
	  [claimStatusExceptionReasonCategories].[CreatedOn], 
	  [claimStatusExceptionReasonCategories].[Description],
	  [claimStatusExceptionReasonCategories].[LastModifiedBy], 
	  [claimStatusExceptionReasonCategories].[LastModifiedOn]

FROM [IntegratedServices].[ClaimStatusTransactions] AS [claimStatusTransactions]
INNER JOIN (
    SELECT [claimStatusBatchClaims].[Id], [claimStatusBatchClaims].[BilledAmount], [claimStatusBatchClaims].[CalculatedLookupHash], [claimStatusBatchClaims].[CalculatedLookupHashInput], [claimStatusBatchClaims].[ClaimBilledOn], [claimStatusBatchClaims].[ClaimLevelMd5Hash], [claimStatusBatchClaims].[ClaimNumber], [claimStatusBatchClaims].[ClaimStatusBatchClaimRootId], [claimStatusBatchClaims].[ClaimStatusBatchId], [claimStatusBatchClaims].[ClaimStatusTransactionId], [claimStatusBatchClaims].[ClientCptCodeId], [claimStatusBatchClaims].[ClientFeeScheduleEntryId], [claimStatusBatchClaims].[ClientId], [claimStatusBatchClaims].[ClientInsuranceId], [claimStatusBatchClaims].[ClientLocationId], [claimStatusBatchClaims].[ClientProviderId], [claimStatusBatchClaims].[CreatedBy], [claimStatusBatchClaims].[CreatedOn], [claimStatusBatchClaims].[DateOfBirth], [claimStatusBatchClaims].[DateOfServiceFrom], [claimStatusBatchClaims].[DateOfServiceTo], [claimStatusBatchClaims].[EntryMd5Hash], [claimStatusBatchClaims].[GroupNpi], [claimStatusBatchClaims].[IsDeleted], [claimStatusBatchClaims].[IsSupplanted], [claimStatusBatchClaims].[LastModifiedBy], [claimStatusBatchClaims].[LastModifiedOn], [claimStatusBatchClaims].[Modifiers], [claimStatusBatchClaims].[NormalizedClaimNumber], [claimStatusBatchClaims].[OriginalClaimBilledOn], [claimStatusBatchClaims].[PatientFirstName], [claimStatusBatchClaims].[PatientId], [claimStatusBatchClaims].[PatientLastName], [claimStatusBatchClaims].[PolicyNumber], [claimStatusBatchClaims].[PolicyNumberUpdatedOn], [claimStatusBatchClaims].[ProcedureCode], [claimStatusBatchClaims].[Quantity], [claimStatusBatchClaims].[RenderingNpi], [claimStatusBatchClaims].[WriteOffAmount]
    FROM [IntegratedServices].[ClaimStatusBatchClaims] AS [claimStatusBatchClaims]
    WHERE [claimStatusBatchClaims].[IsDeleted] = CAST(0 AS bit)
) AS [claimStatusBatchClaims] ON [claimStatusTransactions].[ClaimStatusBatchClaimId] = [claimStatusBatchClaims].[Id]
INNER JOIN (
    SELECT [claimStatusBatches].[Id], [claimStatusBatches].[AbortedOnUtc], [claimStatusBatches].[AbortedReason], [claimStatusBatches].[AllClaimStatusesResolvedOrExpired], [claimStatusBatches].[AssignedClientRpaConfigurationId], [claimStatusBatches].[AssignedDateTimeUtc], [claimStatusBatches].[AssignedToHostName], [claimStatusBatches].[AssignedToIpAddress], [claimStatusBatches].[AssignedToRpaCode], [claimStatusBatches].[AssignedToRpaLocalProcessIds], [claimStatusBatches].[AuthTypeId], [claimStatusBatches].[BatchNumber], [claimStatusBatches].[ClientId], [claimStatusBatches].[ClientInsuranceId], [claimStatusBatches].[CompletedDateTimeUtc], [claimStatusBatches].[CreatedBy], [claimStatusBatches].[CreatedOn], [claimStatusBatches].[InputDocumentId], [claimStatusBatches].[IsDeleted], [claimStatusBatches].[LastModifiedBy], [claimStatusBatches].[LastModifiedOn], [claimStatusBatches].[Priority], [claimStatusBatches].[ReviewedBy], [claimStatusBatches].[ReviewedOnUtc], [claimStatusBatches].[RpaInsuranceId]
    FROM [IntegratedServices].[ClaimStatusBatches] AS [claimStatusBatches]
    WHERE [claimStatusBatches].[IsDeleted] = CAST(0 AS bit)
) AS [claimStatusBatches] ON [claimStatusBatchClaims].[ClaimStatusBatchId] = [claimStatusBatches].[Id]
LEFT JOIN [dbo].[AuthTypes] AS [authTypes] ON [claimStatusBatches].[AuthTypeId] = [authTypes].[Id]
INNER JOIN [dbo].[ClientInsurances] AS [clientInsurances] ON [claimStatusBatches].[ClientInsuranceId] = [clientInsurances].[Id]
LEFT JOIN (
    SELECT [rpaInsurances].[Id], [rpaInsurances].[ApprovalWaitPeriodDays], [rpaInsurances].[ClaimBilledOnWaitDays], [rpaInsurances].[Code], [rpaInsurances].[CreatedBy], [rpaInsurances].[CreatedOn], [rpaInsurances].[InactivatedOn], [rpaInsurances].[IsDeleted], [rpaInsurances].[LastModifiedBy], [rpaInsurances].[LastModifiedOn], [rpaInsurances].[Name], [rpaInsurances].[RpaInsuranceGroupId], [rpaInsurances].[TargetUrl]
    FROM [IntegratedServices].[RpaInsurances] AS [rpaInsurances]
    WHERE [rpaInsurances].[IsDeleted] = CAST(0 AS bit)
) AS [rpaInsurances] ON [clientInsurances].[RpaInsuranceId] = [rpaInsurances].[Id]
LEFT JOIN [dbo].[Providers] AS [providers] ON [claimStatusBatchClaims].[ClientProviderId] = [providers].[Id]
LEFT JOIN [dbo].[Persons] AS [provider_persons] ON [providers].[PersonId] = [provider_persons].[Id]
LEFT JOIN [dbo].[ClientLocations] AS [clientLocation] ON [claimStatusBatchClaims].[ClientLocationId] = [clientLocation].[Id]
LEFT JOIN [IntegratedServices].[ClaimStatusExceptionReasonCategories] AS [claimStatusExceptionReasonCategories] ON [claimStatusTransactions].[ClaimStatusExceptionReasonCategoryId] = [claimStatusExceptionReasonCategories].[Id]
WHERE [claimStatusBatchClaims].[IsSupplanted] = CAST(0 AS bit) AND [claimStatusBatchClaims].[IsDeleted] = CAST(0 AS bit) 
AND [claimStatusTransactions].[IsDeleted] = CAST(0 AS bit) AND [claimStatusTransactions].[ClientId] = @ClientId 

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


END
GO
