SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [IntegratedServices].[spCurrentSummaryExportQuery]
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
	[claimStatusTransactions].[ClientId], [claimStatusTransactions].[CobAmount], 
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
	[claimStatusTransactions].[CreatedOn], 
	[claimStatusTransactions].[LastModifiedOn], 

	[subQuery_claimStatusBatchClaims].[Id], 
	[subQuery_claimStatusBatchClaims].[BilledAmount], 
	[subQuery_claimStatusBatchClaims].[CalculatedLookupHash], 
	[subQuery_claimStatusBatchClaims].[CalculatedLookupHashInput], 
	[subQuery_claimStatusBatchClaims].[ClaimBilledOn], 
	[subQuery_claimStatusBatchClaims].[ClaimLevelMd5Hash], 
	[subQuery_claimStatusBatchClaims].[ClaimNumber], 
	[subQuery_claimStatusBatchClaims].[ClaimStatusBatchClaimRootId], 
	[subQuery_claimStatusBatchClaims].[ClaimStatusBatchId], 
	[subQuery_claimStatusBatchClaims].[ClaimStatusTransactionId],
	[subQuery_claimStatusBatchClaims].[ClientCptCodeId], 
	[subQuery_claimStatusBatchClaims].[ClientFeeScheduleEntryId],
	[subQuery_claimStatusBatchClaims].[ClientId], 
	[subQuery_claimStatusBatchClaims].[ClientInsuranceId], 
	[subQuery_claimStatusBatchClaims].[ClientLocationId],
	[subQuery_claimStatusBatchClaims].[ClientProviderId],
	[subQuery_claimStatusBatchClaims].[CreatedBy], [subQuery_claimStatusBatchClaims].[CreatedOn], [subQuery_claimStatusBatchClaims].[DateOfBirth], [subQuery_claimStatusBatchClaims].[DateOfServiceFrom], 
	[subQuery_claimStatusBatchClaims].[DateOfServiceTo], [subQuery_claimStatusBatchClaims].[EntryMd5Hash], [subQuery_claimStatusBatchClaims].[GroupNpi], [subQuery_claimStatusBatchClaims].[IsDeleted], 
	[subQuery_claimStatusBatchClaims].[IsSupplanted], [subQuery_claimStatusBatchClaims].[LastModifiedBy], [subQuery_claimStatusBatchClaims].[LastModifiedOn], [subQuery_claimStatusBatchClaims].[Modifiers], 
	[subQuery_claimStatusBatchClaims].[NormalizedClaimNumber], [subQuery_claimStatusBatchClaims].[OriginalClaimBilledOn], [subQuery_claimStatusBatchClaims].[PatientFirstName],
	[subQuery_claimStatusBatchClaims].[PatientId], [subQuery_claimStatusBatchClaims].[PatientLastName], [subQuery_claimStatusBatchClaims].[PolicyNumber], [subQuery_claimStatusBatchClaims].[PolicyNumberUpdatedOn], 
	[subQuery_claimStatusBatchClaims].[ProcedureCode], [subQuery_claimStatusBatchClaims].[Quantity], [subQuery_claimStatusBatchClaims].[RenderingNpi], [subQuery_claimStatusBatchClaims].[WriteOffAmount], 
	
	[subQuery_claimStatusBatches].[Id], [subQuery_claimStatusBatches].[AbortedOnUtc], [subQuery_claimStatusBatches].[AbortedReason], 
	[subQuery_claimStatusBatches].[AllClaimStatusesResolvedOrExpired], [subQuery_claimStatusBatches].[AssignedClientRpaConfigurationId],
	[subQuery_claimStatusBatches].[AssignedDateTimeUtc], [subQuery_claimStatusBatches].[AssignedToHostName], [subQuery_claimStatusBatches].[AssignedToIpAddress],
	[subQuery_claimStatusBatches].[AssignedToRpaCode], [subQuery_claimStatusBatches].[AssignedToRpaLocalProcessIds], [subQuery_claimStatusBatches].[AuthTypeId], 
	[subQuery_claimStatusBatches].[BatchNumber], [subQuery_claimStatusBatches].[ClientId], [subQuery_claimStatusBatches].[ClientInsuranceId], [subQuery_claimStatusBatches].[CompletedDateTimeUtc], 
	[subQuery_claimStatusBatches].[CreatedBy], [subQuery_claimStatusBatches].[CreatedOn], [subQuery_claimStatusBatches].[InputDocumentId], [subQuery_claimStatusBatches].[IsDeleted], 
	[subQuery_claimStatusBatches].[LastModifiedBy], [subQuery_claimStatusBatches].[LastModifiedOn], [subQuery_claimStatusBatches].[Priority], [subQuery_claimStatusBatches].[ReviewedBy], 
	[subQuery_claimStatusBatches].[ReviewedOnUtc], [subQuery_claimStatusBatches].[RpaInsuranceId], 
	
	[authTypes].[Id], [authTypes].[CreatedBy],
	[authTypes].[CreatedOn], [authTypes].[Description], [authTypes].[LastModifiedBy],
	[authTypes].[LastModifiedOn], [authTypes].[Name], 

	[clientInsurances].[Id], [clientInsurances].[ClientId], [clientInsurances].[CreatedBy], 
	[clientInsurances].[CreatedOn], [clientInsurances].[PayerIdentifier], [clientInsurances].[ExternalId], 
	[clientInsurances].[FaxNumber], [clientInsurances].[LastModifiedBy], 
	[clientInsurances].[LastModifiedOn], [clientInsurances].[LookupName],
	[clientInsurances].[Name], [clientInsurances].[PhoneNumber], [clientInsurances].[RpaInsuranceId],
	
	[subQuery_rpaInsurances].[Id], [subQuery_rpaInsurances].[ApprovalWaitPeriodDays], [subQuery_rpaInsurances].[ClaimBilledOnWaitDays], [subQuery_rpaInsurances].[Code], [subQuery_rpaInsurances].[CreatedBy], 
	[subQuery_rpaInsurances].[CreatedOn], [subQuery_rpaInsurances].[InactivatedOn], [subQuery_rpaInsurances].[IsDeleted], [subQuery_rpaInsurances].[LastModifiedBy], [subQuery_rpaInsurances].[LastModifiedOn],
	[subQuery_rpaInsurances].[Name], [subQuery_rpaInsurances].[RpaInsuranceGroupId], [subQuery_rpaInsurances].[TargetUrl], 
	
	[clientLocations].[Id], [clientLocations].[AddressId], [clientLocations].[ClientId],
	[clientLocations].[CreatedBy], [clientLocations].[CreatedOn], [clientLocations].[EligibilityLocationId],
	[clientLocations].[ExternalId], [clientLocations].[LastModifiedBy], [clientLocations].[LastModifiedOn],
	[clientLocations].[Name], [clientLocations].[Npi], [clientLocations].[OfficeFaxNumber], 
	[clientLocations].[OfficePhoneNumber], 
	
	[patients].[Id], [patients].[AccountNumber], [patients].[AdministrativeGenderId],
	[patients].[BenefitsCheckedOn], [patients].[ClientId], [patients].[CreatedBy], 
	[patients].[CreatedOn], [patients].[ExternalId], [patients].[InsuranceGroupNumber], 
	[patients].[InsurancePolicyNumber], [patients].[LastModifiedBy], [patients].[LastModifiedOn], 
	[patients].[PersonId], [patients].[PrimaryProviderId], [patients].[ReferringProviderId], 
	[patients].[ResponsiblePartyId], [patients].[ResponsiblePartyRelationshipToPatient], 
	
	[persons].[Id], [persons].[AddressId], [persons].[ClientId], [persons].[CreatedBy], [persons].[CreatedOn], 
	[persons].[DateOfBirth], [persons].[Email], [persons].[FaxNumber], [persons].[FirstName], [persons].[GenderIdentityId], 
	[persons].[HomePhoneNumber], [persons].[LastModifiedBy], [persons].[LastModifiedOn], [persons].[LastName], 
	[persons].[MiddleName], [persons].[MobilePhoneNumber], [persons].[OfficePhoneNumber], [persons].[SocialSecurityNumber], 
	
	[providers].[Id], [providers].[ClientId], [providers].[CreatedBy], [providers].[CreatedOn], 
	[providers].[Credentials], [providers].[DaysToBillKpi], [providers].[ExternalId], 
	[providers].[LastModifiedBy], [providers].[LastModifiedOn], [providers].[License], 
	[providers].[NoShowRateKpi], [providers].[Npi], [providers].[PatientsSeenPerDayKpi],
	[providers].[PersonId], [providers].[ProviderLevelId], [providers].[ScheduledVisitsPerDayKpi], 
	[providers].[SpecialtyId], [providers].[SupervisingProviderId], [providers].[TaxId], 
	[providers].[TaxonomyCode], [providers].[Upin], 
	
	[p3].[Id], [p3].[AddressId], [p3].[ClientId], [p3].[CreatedBy], 
	[p3].[CreatedOn], [p3].[DateOfBirth], [p3].[Email], [p3].[FaxNumber], 
	[p3].[FirstName], [p3].[GenderIdentityId], [p3].[HomePhoneNumber], 
	[p3].[LastModifiedBy], [p3].[LastModifiedOn], [p3].[LastName], [p3].[MiddleName], 
	[p3].[MobilePhoneNumber], [p3].[OfficePhoneNumber], [p3].[SocialSecurityNumber], 
	
	[claimStatusExceptionReasonCategories].[Id], [claimStatusExceptionReasonCategories].[Code], 
	[claimStatusExceptionReasonCategories].[CreatedBy], [claimStatusExceptionReasonCategories].[CreatedOn], 
	[claimStatusExceptionReasonCategories].[Description], [claimStatusExceptionReasonCategories].[LastModifiedBy], 
	[claimStatusExceptionReasonCategories].[LastModifiedOn], 
	
	[claimLineItemStatuses].[Id], [claimLineItemStatuses].[Code], [claimLineItemStatuses].[CreatedBy], 
	[claimLineItemStatuses].[CreatedOn], [claimLineItemStatuses].[DaysWaitBetweenAttempts], 
	[claimLineItemStatuses].[Description], [claimLineItemStatuses].[LastModifiedBy], 
	[claimLineItemStatuses].[LastModifiedOn], [claimLineItemStatuses].[MaximumPipelineDays],
	[claimLineItemStatuses].[MaximumResolutionAttempts], [claimLineItemStatuses].[MinimumResolutionAttempts], 
	[claimLineItemStatuses].[Rank]
FROM [IntegratedServices].[ClaimStatusTransactions] AS [claimStatusTransactions]
INNER JOIN (
    SELECT [claimStatusBatchClaims].[Id], [claimStatusBatchClaims].[BilledAmount], [claimStatusBatchClaims].[CalculatedLookupHash], [claimStatusBatchClaims].[CalculatedLookupHashInput], [claimStatusBatchClaims].[ClaimBilledOn], [claimStatusBatchClaims].[ClaimLevelMd5Hash], [claimStatusBatchClaims].[ClaimNumber], [claimStatusBatchClaims].[ClaimStatusBatchClaimRootId], [claimStatusBatchClaims].[ClaimStatusBatchId], [claimStatusBatchClaims].[ClaimStatusTransactionId], [claimStatusBatchClaims].[ClientCptCodeId], [claimStatusBatchClaims].[ClientFeeScheduleEntryId], [claimStatusBatchClaims].[ClientId], [claimStatusBatchClaims].[ClientInsuranceId], [claimStatusBatchClaims].[ClientLocationId], [claimStatusBatchClaims].[ClientProviderId], [claimStatusBatchClaims].[CreatedBy], [claimStatusBatchClaims].[CreatedOn], [claimStatusBatchClaims].[DateOfBirth], [claimStatusBatchClaims].[DateOfServiceFrom], [claimStatusBatchClaims].[DateOfServiceTo], [claimStatusBatchClaims].[EntryMd5Hash], [claimStatusBatchClaims].[GroupNpi], [claimStatusBatchClaims].[IsDeleted], [claimStatusBatchClaims].[IsSupplanted], [claimStatusBatchClaims].[LastModifiedBy], [claimStatusBatchClaims].[LastModifiedOn], [claimStatusBatchClaims].[Modifiers], [claimStatusBatchClaims].[NormalizedClaimNumber], [claimStatusBatchClaims].[OriginalClaimBilledOn], [claimStatusBatchClaims].[PatientFirstName], [claimStatusBatchClaims].[PatientId], [claimStatusBatchClaims].[PatientLastName], [claimStatusBatchClaims].[PolicyNumber], [claimStatusBatchClaims].[PolicyNumberUpdatedOn], [claimStatusBatchClaims].[ProcedureCode], [claimStatusBatchClaims].[Quantity], [claimStatusBatchClaims].[RenderingNpi], [claimStatusBatchClaims].[WriteOffAmount]
    FROM [IntegratedServices].[ClaimStatusBatchClaims] AS [claimStatusBatchClaims]
    WHERE [claimStatusBatchClaims].[IsDeleted] = CAST(0 AS bit)
) AS [subQuery_claimStatusBatchClaims] ON [claimStatusTransactions].[ClaimStatusBatchClaimId] = [subQuery_claimStatusBatchClaims].[Id]
INNER JOIN (
    SELECT [claimStatusBatches].[Id], [claimStatusBatches].[AbortedOnUtc], [claimStatusBatches].[AbortedReason], [claimStatusBatches].[AllClaimStatusesResolvedOrExpired], [claimStatusBatches].[AssignedClientRpaConfigurationId], [claimStatusBatches].[AssignedDateTimeUtc], [claimStatusBatches].[AssignedToHostName], [claimStatusBatches].[AssignedToIpAddress], [claimStatusBatches].[AssignedToRpaCode], [claimStatusBatches].[AssignedToRpaLocalProcessIds], [claimStatusBatches].[AuthTypeId], [claimStatusBatches].[BatchNumber], [claimStatusBatches].[ClientId], [claimStatusBatches].[ClientInsuranceId], [claimStatusBatches].[CompletedDateTimeUtc], [claimStatusBatches].[CreatedBy], [claimStatusBatches].[CreatedOn], [claimStatusBatches].[InputDocumentId], [claimStatusBatches].[IsDeleted], [claimStatusBatches].[LastModifiedBy], [claimStatusBatches].[LastModifiedOn], [claimStatusBatches].[Priority], [claimStatusBatches].[ReviewedBy], [claimStatusBatches].[ReviewedOnUtc], [claimStatusBatches].[RpaInsuranceId]
    FROM [IntegratedServices].[ClaimStatusBatches] AS [claimStatusBatches]
    WHERE [claimStatusBatches].[IsDeleted] = CAST(0 AS bit)
) AS [subQuery_claimStatusBatches] ON [subQuery_claimStatusBatchClaims].[ClaimStatusBatchId] = [subQuery_claimStatusBatches].[Id]
LEFT JOIN [dbo].[AuthTypes] AS [authTypes] ON [subQuery_claimStatusBatches].[AuthTypeId] = [authTypes].[Id]
INNER JOIN [dbo].[ClientInsurances] AS [clientInsurances] ON [subQuery_claimStatusBatches].[ClientInsuranceId] = [clientInsurances].[Id]
LEFT JOIN (
    SELECT [rpaInsurances].[Id], [rpaInsurances].[ApprovalWaitPeriodDays], [rpaInsurances].[ClaimBilledOnWaitDays], [rpaInsurances].[Code], [rpaInsurances].[CreatedBy], [rpaInsurances].[CreatedOn], [rpaInsurances].[InactivatedOn], [rpaInsurances].[IsDeleted], [rpaInsurances].[LastModifiedBy], [rpaInsurances].[LastModifiedOn], [rpaInsurances].[Name], [rpaInsurances].[RpaInsuranceGroupId], [rpaInsurances].[TargetUrl]
    FROM [IntegratedServices].[RpaInsurances] AS [rpaInsurances]
    WHERE [rpaInsurances].[IsDeleted] = CAST(0 AS bit)
) AS [subQuery_rpaInsurances] ON [clientInsurances].[RpaInsuranceId] = [subQuery_rpaInsurances].[Id]

LEFT JOIN [dbo].[ClientLocations] AS [clientLocations] ON [subQuery_claimStatusBatchClaims].[ClientLocationId] = [clientLocations].[Id]
LEFT JOIN [dbo].[Patients] AS [patients] ON [subQuery_claimStatusBatchClaims].[PatientId] = [patients].[Id]
LEFT JOIN [dbo].[Persons] AS [persons] ON [patients].[PersonId] = [persons].[Id]
LEFT JOIN [dbo].[Providers] AS [providers] ON [subQuery_claimStatusBatchClaims].[ClientProviderId] = [providers].[Id]
LEFT JOIN [dbo].[Persons] AS [p3] ON [providers].[PersonId] = [p3].[Id]
LEFT JOIN [IntegratedServices].[ClaimStatusExceptionReasonCategories] AS [claimStatusExceptionReasonCategories] ON [claimStatusTransactions].[ClaimStatusExceptionReasonCategoryId] = [claimStatusExceptionReasonCategories].[Id]
LEFT JOIN [IntegratedServices].[ClaimLineItemStatuses] AS [claimLineItemStatuses] ON [claimStatusTransactions].[ClaimLineItemStatusId] = [claimLineItemStatuses].[Id]

WHERE [subQuery_claimStatusBatchClaims].[IsSupplanted] = CAST(0 AS bit) 
	  AND [subQuery_claimStatusBatchClaims].[IsDeleted] = CAST(0 AS bit) 
	  AND [claimStatusTransactions].[IsDeleted] = CAST(0 AS bit) 
	  AND [claimStatusTransactions].[ClientId] = @ClientId 
	  
		--@ReceivedFrom and @ReceivedTo
		AND ((CONVERT(date, [subQuery_claimStatusBatchClaims].CreatedOn) >= @ReceivedFrom) OR @ReceivedFrom IS NULL)
		AND ((CONVERT(date, [subQuery_claimStatusBatchClaims].CreatedOn) <= @ReceivedTo) OR @ReceivedTo IS NULL)
		--@DateOfServiceFrom and @DateOfServiceTo
		AND ((CONVERT(date,[subQuery_claimStatusBatchClaims].DateOfServiceFrom) >= @DateOfServiceFrom) OR @DateOfServiceFrom IS NULL)
		AND ((CONVERT(date,[subQuery_claimStatusBatchClaims].DateOfServiceTo  )<= @DateOfServiceTo) OR @DateOfServiceTo IS NULL)
		--@ClaimBilledFrom and @ClaimBilledTo
		AND ((CONVERT(date,[subQuery_claimStatusBatchClaims].ClaimBilledOn) >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
		AND ((CONVERT(date,[subQuery_claimStatusBatchClaims].ClaimBilledOn) <= @ClaimBilledTo) OR @ClaimBilledTo is null)
		--@TransactionDateFrom and @TransactionDateTo
		--The COALESCE function is used to return the first non-null expression among its arguments. 
		AND ((CONVERT(date,COALESCE([claimStatusTransactions].LastModifiedOn, [claimStatusTransactions].CreatedOn)) >= @TransactionDateFrom) OR @TransactionDateFrom is NULL )
		AND ((CONVERT(date,COALESCE([claimStatusTransactions].LastModifiedOn, [claimStatusTransactions].CreatedOn)) <= @TransactionDateTo) OR @TransactionDateTo is NULL)
    
	    ---Multi Select Filter----		
		AND ([claimStatusTransactions].[ClaimLineItemStatusId] IN (SELECT convert(int, value)
		FROM string_split(@DelimitedLineItemStatusIds, ',')) OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = ''))

		And ([subQuery_claimStatusBatches].[ClientInsuranceId] in (SELECT convert(int, value)
			FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))

		And ((@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = '') OR ([claimStatusTransactions].[ClaimStatusExceptionReasonCategoryId]
				IN (SELECT convert(int, value) FROM string_split(@ClientExceptionReasonCategoryIds, ','))))

		And ([subQuery_claimStatusBatchClaims].[ClientLocationId] IN (SELECT convert(int, value)
			FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))

		And ([providers].Id in (SELECT convert(int, value)
			FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))

		And ([subQuery_claimStatusBatches].[AuthTypeId] IN (SELECT convert(int, value)
			FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))

		And ([subQuery_claimStatusBatchClaims].[ProcedureCode] IN (SELECT convert(nvarchar(16), value)
			FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))

	END
GO




