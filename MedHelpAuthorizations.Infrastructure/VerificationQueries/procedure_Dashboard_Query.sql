DECLARE @ClientId int
DECLARE @DelimitedLineItemStatusIds nvarchar(MAX) = NULL
DECLARE @ReceivedFrom DateTime = NULL
DECLARE @ReceivedTo DateTime = NULL
DECLARE @DateOfServiceFrom DateTime = NULL
DECLARE @DateOfServiceTo DateTime = NULL
DECLARE @TransactionDateFrom DateTime = NULL
DECLARE @TransactionDateTo DateTime = NULL
DECLARE @ClaimBilledFrom DateTime = NULL
DECLARE @ClaimBilledTo DateTime = NULL
DECLARE @ClientProviderIds nvarchar(MAX) = NULL
DECLARE @ClientLocationIds nvarchar(MAX) = NULL
DECLARE @ClientInsuranceIds nvarchar(MAX)= null
DECLARE @ClientExceptionReasonCategoryIds nvarchar(MAX)= null
DECLARE @ClientAuthTypeIds nvarchar(MAX)= null
DECLARE @ClientProcedureCodes nvarchar(MAX)= null
DECLARE @PatientId int = NULL
DECLARE @ClaimStatusBatchId int = NULL
DECLARE @FlattenedLineItemStatus NVARCHAR(MAX)=NULL
DECLARE @DashboardType NVARCHAR(MAX)=NULL
DECLARE @ClaimStatusType int = NULL
DECLARE @ClaimStatusTypeStatus NVARCHAR(MAX)=NULL
DECLARE @DenialStatusIds NVARCHAR(MAX)=NULL
DECLARE @HasTest bit =  CAST(0 AS BIT);
DECLARE @DeniedClaimStatusType int;

SET @ClientId = 3
SET @ClaimBilledFrom = '2023-12-07 00:00:00'
SET @ClaimBilledTo = '2024-01-05 23:59:59'

--SET @ClaimStatusType = 2
--SET @DeniedClaimStatusType = 2
SET @ClientProcedureCodes='80307'

SET @DashboardType = N'ProceduresDashboard'
--SET @DenialStatusIds = N'7,12,23,9,3,13,19,18,24'
--SET @DelimitedLineItemStatusIds = N'7,12,23,9,3,13,19,18,24'

Select SUM([claimStatusBatchClaim].[BilledAmount]) as Charges,SUM([claimStatusTransaction].TotalAllowedAmount) as TotalAllowedAmount, COUNT([claimStatusBatchClaim].ClaimLevelMd5Hash) as Actual_LineItems, COUNT(DISTINCT [claimStatusBatchClaim].ClaimLevelMd5Hash) as Disctinct_LineItems
        FROM [IntegratedServices].ClaimStatusBatchClaims as [claimStatusBatchClaim] 

    LEFT JOIN [IntegratedServices].ClaimStatusTransactions as [claimStatusTransaction] ON [claimStatusTransaction].ClaimStatusBatchClaimId = [claimStatusBatchClaim].Id
    
    JOIN [IntegratedServices].ClaimStatusBatches as [claimStatusBatch] ON [claimStatusBatchClaim].ClaimStatusBatchId = [claimStatusBatch].Id
    
    LEFT JOIN [dbo].[AuthTypes] as [authType] ON [claimStatusBatch].AuthTypeId = [authType].Id
    
    LEFT JOIN [IntegratedServices].ClaimLineItemStatuses as [claimLineItemStatus] ON [claimStatusTransaction].ClaimLineItemStatusId = [claimLineItemStatus].Id
    
    INNER JOIN [dbo].ClientInsurances as [insurance] ON [claimStatusBatch].ClientInsuranceId = [insurance].Id
    
    LEFT JOIN [dbo].Providers as [provider] ON [claimStatusBatchClaim].ClientProviderId = [provider].Id
    LEFT JOIN [dbo].Persons as [person] On [provider].PersonId = [person].Id -- person for the provider
    
    LEFT JOIN [dbo].Patients as [patient] ON [claimStatusBatchClaim].PatientId = [patient].Id
    LEFT JOIN [dbo].Persons as [person_Patient] On [patient].PersonId = [person_Patient].Id
    
    LEFT JOIN [dbo].ClientLocations as [location] On [claimStatusBatchClaim].ClientLocationId = [location].Id


WHERE 
    	[claimStatusBatchClaim].[IsDeleted] = CAST(0 AS BIT) AND 
    	[claimStatusBatchClaim].[IsSupplanted] = CAST(0 AS BIT)
    					 
        AND [claimStatusBatchClaim].[ClientId] = @ClientId 
        And ([claimStatusBatchClaim].[ProcedureCode] IN (SELECT convert(nvarchar(MAX), value) FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))

		---Multi Select Filter----
		And ([claimStatusBatch].ClientInsuranceId in (SELECT convert(int, value)
			FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
		And ((@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = '') OR ([claimStatusTransaction].ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value)
			FROM string_split(@ClientExceptionReasonCategoryIds, ','))))
		And ([claimStatusBatchClaim].ClientLocationId in (SELECT convert(int, value)
			FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
		And ([claimStatusBatchClaim].ClientProviderId in (SELECT convert(int, value)
			FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))
		And ([claimStatusBatch].AuthTypeId in (SELECT convert(int, value)
			FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
		----As Claim Level grouping is disabled, So enableing below where clauses for check data based in dates----

		AND (([claimStatusBatchClaim].CreatedOn >= @ReceivedFrom) OR @ReceivedFrom IS NULL)
		AND (([claimStatusBatchClaim].CreatedOn <= @ReceivedTo) OR @ReceivedTo IS NULL)
		AND (([claimStatusBatchClaim].DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom IS NULL)
		AND (([claimStatusBatchClaim].DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo IS NULL)
		AND (([claimStatusBatchClaim].ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
		AND (([claimStatusBatchClaim].ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)
		AND ((COALESCE([claimStatusTransaction].LastModifiedOn, [claimStatusTransaction].CreatedOn) >= @TransactionDateFrom) OR @TransactionDateFrom is NULL )
		AND ((COALESCE([claimStatusTransaction].LastModifiedOn, [claimStatusTransaction].CreatedOn) <= @TransactionDateTo) OR @TransactionDateTo is NULL)

		AND [claimStatusBatchClaim].IsDeleted = 0
		AND [claimStatusBatchClaim].IsSupplanted = 0
		AND (([claimStatusBatchClaim].PatientId = @PatientId) OR (@PatientId IS NULL))
	    AND (([claimStatusBatchClaim].ClaimStatusBatchId = @ClaimStatusBatchId) OR (@ClaimStatusBatchId IS NULL)  OR (@ClaimStatusBatchId = 0))	

   GROUP BY  
			--CONVERT(DATE, [claimStatusBatchClaim].ClaimBilledOn)    
			--,CONVERT(DATE, [claimStatusBatchClaim].DateOfServiceFrom)
			--,CONVERT(DATE, [claimStatusBatchClaim].CreatedOn)        
			--,CONVERT(DATE, [claimStatusTransaction].LastModifiedOn)
		 [claimStatusTransaction].ClaimStatusExceptionReasonCategoryId
		, TRIM([claimStatusBatchClaim].[ProcedureCode])

                
                             
                             