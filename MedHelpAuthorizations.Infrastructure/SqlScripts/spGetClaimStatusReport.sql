SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [IntegratedServices].[spGetClaimStatusReport]
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
        [person_Patient].LastName as 'PatientLastName'
        ,[person_Patient].FirstName as 'PatientFirstName'
        ,[person_Patient].DateOfBirth as 'PatientDOB'
        ,[claimStatusBatchClaims].PolicyNumber as 'PolicyNumber'
        ,[authType].Name as 'ServiceType'
        ,[insurance].LookupName as 'PayerName'
        ,[claimStatusBatchClaims].ClaimNumber as 'OfficeClaimNumber'
        ,[claimStatusTransactions].ClaimNumber as 'PayerClaimNumber'
        ,[claimStatusTransactions].LineItemControlNumber as 'PayerLineItemControlNumber'
        ,[claimStatusBatchClaims].ProcedureCode as 'ProcedureCode'
        ,[claimStatusBatchClaims].DateOfServiceFrom as 'DateOfServiceFrom'
        ,[claimStatusBatchClaims].DateOfServiceTo as 'DateOfServiceTo'
        ,[claimLineItemStatuses].Description as ClaimLineItemStatus
        ,[claimStatusTransactions].ClaimLineItemStatusId as 'ClaimLineItemStatusId'
        ,[claimStatusTransactions].ClaimLineItemStatusValue as 'ClaimLineItemStatusValue'
        ,[claimStatusTransactions].ExceptionReason as 'ExceptionReason'
        ,[claimStatusTransactions].ExceptionRemark as 'ExceptionRemark'
        ,[claimStatusTransactions].ReasonCode as 'ReasonCode'
        ,[claimStatusBatchClaims].ClaimBilledOn as 'ClaimBilledOn'
        ,[claimStatusBatchClaims].BilledAmount as 'BilledAmount'
        ,[claimStatusTransactions].LineItemPaidAmount as 'LineItemPaidAmount'
        ,[claimStatusTransactions].TotalAllowedAmount as 'TotalAllowedAmount'
        ,[claimStatusTransactions].TotalNonAllowedAmount as 'NonAllowedAmount'
        ,[claimStatusTransactions].CheckPaidAmount as 'CheckPaidAmount'
        ,[claimStatusTransactions].CheckDate as 'CheckDate'
        ,[claimStatusTransactions].CheckNumber as 'CheckNumber'
        ,[claimStatusTransactions].ReasonDescription as 'ReasonDescription'
        ,[claimStatusTransactions].RemarkCode as 'RemarkCode'
        ,[claimStatusTransactions].RemarkDescription as 'RemarkDescription'
        ,[claimStatusTransactions].CoinsuranceAmount as 'CoinsuranceAmount'
        ,[claimStatusTransactions].CopayAmount as 'CopayAmount'
        ,[claimStatusTransactions].DeductibleAmount as 'DeductibleAmount'
        ,[claimStatusTransactions].CobAmount as 'CobAmount'
        ,[claimStatusTransactions].PenalityAmount as 'PenalityAmount'
        ,[claimStatusTransactions].EligibilityStatus as 'EligibilityStatus'
        ,[claimStatusTransactions].EligibilityInsurance as 'EligibilityInsurance'
        ,[claimStatusTransactions].EligibilityPolicyNumber as 'EligibilityPolicyNumber'
        ,[claimStatusTransactions].EligibilityFromDate as 'EligibilityFromDate'
        ,[claimStatusTransactions].VerifiedMemberId as 'VerifiedMemberId'
        ,[claimStatusTransactions].CobLastVerified as 'CobLastVerified'
        ,[claimStatusTransactions].LastActiveEligibleDateRange as 'LastActiveEligibleDateRange'
        ,[claimStatusTransactions].PrimaryPayer as 'PrimaryPayer'
        ,[claimStatusTransactions].PrimaryPolicyNumber as 'PrimaryPolicyNumber'
        ,[claimStatusBatches].BatchNumber as 'BatchNumber'
        ,CONVERT(VARCHAR, [claimStatusBatchClaims].CreatedOn, 101) as 'AitClaimReceivedDate'
        ,CONVERT(VARCHAR, [claimStatusBatchClaims].CreatedOn, 108) as 'AitClaimReceivedTime'
        ,CONVERT(VARCHAR, [claimStatusTransactions].LastModifiedOn, 101) as 'TransactionDate'
        ,CONVERT(VARCHAR, [claimStatusTransactions].LastModifiedOn, 108) as 'TransactionTime'
        ,CONCAT([person].LastName, ', ', [person].FirstName) as ClientProviderName
        ,[claimStatusTransactions].PaymentType as 'PaymentType'

    FROM [IntegratedServices].ClaimStatusBatchClaims as [claimStatusBatchClaims]
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
	) as c2 ON [claimStatusBatchClaims].ClaimBilledOn = c2.LatestClaimBilledOn AND [claimStatusBatchClaims].ClaimLevelMd5Hash = c2.ClaimLevelMd5Hash
	LEFT JOIN [IntegratedServices].ClaimStatusTransactions as [claimStatusTransactions] ON [claimStatusTransactions].ClaimStatusBatchClaimId = [claimStatusBatchClaims].Id
	JOIN [IntegratedServices].ClaimStatusBatches as [claimStatusBatches] ON [claimStatusBatchClaims].ClaimStatusBatchId = [claimStatusBatches].Id
    LEFT JOIN [dbo].[AuthTypes] as [authType] ON [claimStatusBatches].AuthTypeId = [authType].Id
	LEFT JOIN [IntegratedServices].ClaimLineItemStatuses as [claimLineItemStatuses] ON [claimStatusTransactions].ClaimLineItemStatusId = [claimLineItemStatuses].Id
	LEFT JOIN [dbo].Providers as [provider] ON [claimStatusBatchClaims].ClientProviderId = [provider].Id
	LEFT JOIN [dbo].Persons as [person] On [provider].PersonId = [person].Id -- person for the provider
    LEFT JOIN [dbo].Patients as [patient] ON [claimStatusBatchClaims].PatientId = [patient].Id
	LEFT JOIN [dbo].Persons as [person_Patient] On [patient].PersonId = [person_Patient].Id -- person for the patient
    LEFT JOIN [dbo].ClientInsurances as [insurance] ON [claimStatusBatches].ClientInsuranceId = [insurance].Id
	LEFT JOIN [dbo].ClientLocations as [location] On [claimStatusBatchClaims].ClientLocationId = [location].Id
	LEFT JOIN [IntegratedServices].ClaimStatusExceptionReasonCategories as [claimStatusExceptionReasonCategory] ON [claimStatusTransactions].ClaimStatusExceptionReasonCategoryId = [claimStatusExceptionReasonCategory].Id

    WHERE [claimStatusBatchClaims].ClientId = @ClientId
        AND ([claimStatusTransactions].ClaimLineItemStatusId IN (SELECT CONVERT(INT, value) FROM STRING_SPLIT(@DelimitedLineItemStatusIds, ',')) OR (@DelimitedLineItemStatusIds IS NULL OR @DelimitedLineItemStatusIds = ''))
        AND ([claimStatusBatches].ClientInsuranceId IN (SELECT CONVERT(INT, value) FROM STRING_SPLIT(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds IS NULL OR @ClientInsuranceIds = ''))
        AND ((@ClientExceptionReasonCategoryIds IS NULL OR @ClientExceptionReasonCategoryIds = '') OR ([claimStatusTransactions].ClaimStatusExceptionReasonCategoryId IN (SELECT CONVERT(INT, value) FROM STRING_SPLIT(@ClientExceptionReasonCategoryIds, ','))))
        AND ([location].Id IN (SELECT CONVERT(INT, value) FROM STRING_SPLIT(@ClientLocationIds, ',')) OR (@ClientLocationIds IS NULL OR @ClientLocationIds = ''))
        AND ([provider].Id IN (SELECT CONVERT(INT, value) FROM STRING_SPLIT(@ClientProviderIds, ',')) OR (@ClientProviderIds IS NULL OR @ClientProviderIds = ''))
        AND ([claimStatusBatches].AuthTypeId IN (SELECT CONVERT(INT, value) FROM STRING_SPLIT(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds IS NULL OR @ClientAuthTypeIds = ''))
        AND ([claimStatusBatchClaims].ProcedureCode IN (SELECT CONVERT(NVARCHAR(16), value) FROM STRING_SPLIT(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes IS NULL OR @ClientProcedureCodes = ''))
        AND ([claimStatusBatchClaims].CreatedOn >= COALESCE(@ReceivedFrom, [claimStatusBatchClaims].CreatedOn))
        AND ([claimStatusBatchClaims].CreatedOn <= COALESCE(@ReceivedTo, [claimStatusBatchClaims].CreatedOn))
        AND ([claimStatusBatchClaims].DateOfServiceFrom >= COALESCE(@DateOfServiceFrom, [claimStatusBatchClaims].DateOfServiceFrom))
        AND ([claimStatusBatchClaims].DateOfServiceTo <= COALESCE(@DateOfServiceTo, [claimStatusBatchClaims].DateOfServiceTo))
        AND ([claimStatusBatchClaims].ClaimBilledOn >= COALESCE(@ClaimBilledFrom, [claimStatusBatchClaims].ClaimBilledOn))
        AND ([claimStatusBatchClaims].ClaimBilledOn <= COALESCE(@ClaimBilledTo, [claimStatusBatchClaims].ClaimBilledOn))
        AND (COALESCE([claimStatusTransactions].LastModifiedOn, [claimStatusTransactions].CreatedOn) >= COALESCE(@TransactionDateFrom, COALESCE([claimStatusTransactions].LastModifiedOn, [claimStatusTransactions].CreatedOn)))
        AND (COALESCE([claimStatusTransactions].LastModifiedOn, [claimStatusTransactions].CreatedOn) <= COALESCE(@TransactionDateTo, COALESCE([claimStatusTransactions].LastModifiedOn, [claimStatusTransactions].CreatedOn)))
        AND [claimStatusBatchClaims].IsDeleted = 0
        AND [claimStatusBatchClaims].IsSupplanted = 0
        AND ([claimStatusBatchClaims].PatientId = COALESCE(@PatientId, [claimStatusBatchClaims].PatientId) OR @PatientId IS NULL)
        AND ([claimStatusBatchClaims].ClaimStatusBatchId = COALESCE(@ClaimStatusBatchId, [claimStatusBatchClaims].ClaimStatusBatchId) OR @ClaimStatusBatchId IS NULL OR @ClaimStatusBatchId = 0);
    END
GO
