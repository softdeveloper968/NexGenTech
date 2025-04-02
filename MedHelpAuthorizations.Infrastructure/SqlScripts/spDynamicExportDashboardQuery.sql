CREATE OR ALTER   PROCEDURE [dbo].[spDynamicExportDashboardQuery]
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
    @ClaimBilledTo DATETIME = NULL,
    @FlattenedLineItemStatus NVARCHAR(MAX)=NULL,
    @DashboardType NVARCHAR(MAX)= NULL,
    @ClaimStatusType int = NULL,
    @ClaimStatusTypeStatus NVARCHAR(MAX)=NULL,
    @DenialStatusIds NVARCHAR(MAX)=NULL,
    @HasIncludeClaimStatusTransactionLineItemStatusChange bit = 0
WITH RECOMPILE
AS
--Declare 	@ClientId INT
--Declare     @ClientLocationIds NVARCHAR(MAX) = NULL
--Declare     @ClientInsuranceIds NVARCHAR(MAX) = NULL
--Declare     @ClientAuthTypeIds NVARCHAR(MAX) = NULL
--Declare     @ClientExceptionReasonCategoryIds NVARCHAR(MAX) = NULL
--Declare     @ClientProcedureCodes NVARCHAR(MAX) = NULL
--Declare     @DelimitedLineItemStatusIds NVARCHAR(MAX) = NULL
--Declare     @ClientProviderIds NVARCHAR(MAX) = NULL
--Declare     @PatientId INT = NULL
--Declare     @ClaimStatusBatchId INT = NULL
--Declare     @ReceivedFrom DATETIME = NULL
--Declare     @ReceivedTo DATETIME = NULL
--Declare     @DateOfServiceFrom DATETIME = NULL
--Declare     @DateOfServiceTo DATETIME = NULL
--Declare     @TransactionDateFrom DATETIME = NULL
--Declare     @TransactionDateTo DATETIME = NULL
--Declare     @ClaimBilledFrom DATETIME = NULL
--Declare     @ClaimBilledTo DATETIME = NULL
--Declare     @FlattenedLineItemStatus NVARCHAR(MAX)=NULL
--Declare     @DashboardType NVARCHAR(MAX)= NULL
--Declare     @ClaimStatusType int = NULL
--Declare     @ClaimStatusTypeStatus NVARCHAR(MAX)=NULL
--Declare     @DenialStatusIds NVARCHAR(MAX)=NULL
--Declare     @HasIncludeClaimStatusTransactionLineItemStatusChange bit = 0

DECLARE @HasTest bit =  CAST(1 AS BIT);---LOCAL TEST USE 0
--SET @ClientId=3
--SET @ClaimStatusType=2
--SET @HasIncludeClaimStatusTransactionLineItemStatusChange=1

BEGIN
    
    -----Declare SQL Columns variables--------------
    DECLARE @ProceduresDashboard NVARCHAR(MAX)= N'ProceduresDashboard'
    DECLARE @in_Process NVARCHAR(MAX) = N'In-Process'
    DECLARE @initialSummaryDashboardClaimLevelGroupingJoinClause NVARCHAR(MAX) =N''
    DECLARE @fnGetClaimLevelGroupingJoinClause NVARCHAR(MAX) =N''
    DECLARE @additionalPatientSQLColumns NVARCHAR(MAX)= N''
    DECLARE @additionalReceivedDateSQLColumns NVARCHAR(MAX)= N''
    DECLARE @additionalDateOfServiceDateSQLColumns NVARCHAR(MAX)= N''
    DECLARE @additionalTransactionDateSQLColumns NVARCHAR(MAX)= N''
    DECLARE @additionalClaimBilledDateSQLColumns NVARCHAR(MAX)= N''
    DECLARE @additionalClaimStatusBatchIdSQLColumns NVARCHAR(MAX)= N''
    DECLARE @additionalProviderSQLColumns NVARCHAR(MAX) = N''
    DECLARE @additionalDelimitedLineItemStatusIdsSQLColumns NVARCHAR(MAX) = N''
    DECLARE @additionalLocationSQLColumns NVARCHAR(MAX)= N''
    DECLARE @additionalInsuranceSQLColumns NVARCHAR(MAX)= N''
    DECLARE @additionalAuthTypeSQLColumns NVARCHAR(MAX)= N''
    DECLARE @additionalExceptionReasonCategorySQLColumns NVARCHAR(MAX)= N''
    DECLARE @additionalProcedureCodesSQLColumns NVARCHAR(MAX)= N''
    DECLARE @customDashboardTypeGroupingJoinQuery NVARCHAR(MAX) =N''
    DECLARE @claimStatusTransactionLineItemStatusChangeSQLColumns NVARCHAR(MAX)= N''
    DECLARE @claimStatusTransactionLineItemStatusChangeJoinQuery NVARCHAR(MAX) =N''
    
    -----Declare SQL Where Clause variables--------------
    DECLARE @SqlWhereClause NVARCHAR(MAX)=''
    DECLARE @clientIdWhereClause NVARCHAR(MAX)=''
    DECLARE @patientWhereClause NVARCHAR(MAX)= N''
    DECLARE @receivedDateWhereClause NVARCHAR(MAX)= N''
    DECLARE @dateOfServiceDateWhereClause NVARCHAR(MAX)= N''
    DECLARE @transactionDateWhereClause NVARCHAR(MAX)= N''
    DECLARE @claimBilledDateWhereClause NVARCHAR(MAX)= N''
    DECLARE @ClaimStatusBatchIdWhereClause NVARCHAR(MAX)= N''
    DECLARE @providerWhereClause NVARCHAR(MAX) = N''
    DECLARE @delimitedLineItemStatusIdsWhereClause NVARCHAR(MAX) = N''
    DECLARE @locationWhereClause NVARCHAR(MAX)= N''
    DECLARE @InsuranceWhereClause NVARCHAR(MAX)= N''
    DECLARE @AuthTypeWhereClause NVARCHAR(MAX)= N''
    DECLARE @ExceptionReasonCategoryWhereClause NVARCHAR(MAX)= N''
    DECLARE @ProcedureCodesWhereClause NVARCHAR(MAX)= N''
    DECLARE @ClaimStatusTypeWhereClause NVARCHAR(MAX)=N''
    DECLARE @claimStatusTransactionLineItemStatusChangeWhereClause NVARCHAR(MAX)=N''
    
    ---------Declare Claim Status Type--------------
    DECLARE @PaidClaimStatusType             INT = 1;
    DECLARE @DeniedClaimStatusType           INT = 2;
    DECLARE @OpenClaimStatusType             INT = 3;
    DECLARE @OtherAdjudicatedClaimStatusType INT = 4;
    DECLARE @OtherOpenClaimStatusType        INT = 5;
    
    DECLARE @writeOffStatus NVARCHAR(MAX)= 'WriteOff'
    DECLARE @OpenStatus NVARCHAR(MAX)= 'Open'
    DECLARE @ContractualStatus NVARCHAR(MAX)= 'Contractual'
    DECLARE @writeOffStatusVal int = 20
    DECLARE @contractualStatusVal int = 22
    
    DECLARE @selectColumnInitializer NVARCHAR(MAX) = N' 
    SELECT 
    '
    
    DECLARE @person_patientColumns NVARCHAR(MAX)=N'
             ,[person_Patient].[LastName] as PatientLastName
            ,[person_Patient].[FirstName] as PatientFirstName
            ,[person_Patient].[DateOfBirth] as PatientDOB
    '
    
    DECLARE @authTypeColumns NVARCHAR(MAX) = N'
            ,[authType].[Name] as ServiceType
    '
    
    DECLARE @insuranceColumns NVARCHAR(MAX) = N'
        ,[insurance].[LookupName] as PayerName
        ,[insurance].[Name] as InsuranceName
    '
    
    DECLARE @claimLineItemStatusesColumns NVARCHAR(MAX) = N'
        ,[claimLineItemStatus].[Description] as ClaimLineItemStatus
        ,[claimLineItemStatus].[Id] as ClaimLineItemStatusId
    	,[claimLineItemStatus].[ClaimStatusTypeId] as ClaimLineItemStatus_ClaimStatusTypeId
    '
    
    DECLARE @providerColumns NVARCHAR(MAX) = N'
         ,CONCAT([person].LastName, '', '', [person].FirstName) as ClientProviderName
         ,[provider].Npi as ProViderNPI
         ,[provider].SpecialtyId as ProViderSpecialtyId
    '
    
    DECLARE @claimStatusBatchesColumns NVARCHAR(MAX) = N'
         	,[claimStatusBatch].[ClientInsuranceId] AS [claimStatusBatches_ClientInsuranceId]
    		,[claimStatusBatch].[BatchNumber] AS [claimStatusBatches_BatchNumber]
    		,[claimStatusBatch].[CreatedOn] AS [claimStatusBatches_ClaimReceivedDate]
    		,[claimStatusBatch].[ReviewedOnUtc] AS [claimStatusBatches_ReviewedOnUtc]
    		,[claimStatusBatch].[AbortedOnUtc] AS [claimStatusBatches_AbortedOnUtc]
    		,[claimStatusBatch].[AbortedReason] AS [claimStatusBatches_AbortedReason]
    '
    
    DECLARE @claimStatusTransactionsColumns NVARCHAR(MAX) = N'
         	,[claimStatusTransaction].ClaimNumber as PayerClaimNumber
            ,[claimStatusTransaction].LineItemControlNumber as PayerLineItemControlNumber
            ,[claimStatusTransaction].ClaimLineItemStatusId as ClaimLineItemStatusId
            ,[claimStatusTransaction].ClaimLineItemStatusValue as ClaimLineItemStatusValue
            ,[claimStatusTransaction].ExceptionReason as ExceptionReason
            ,[claimStatusTransaction].ExceptionRemark as ExceptionRemark
            ,[claimStatusTransaction].ReasonCode as ReasonCode
            ,
			CASE 
				WHEN ([claimLineItemStatus].ClaimStatusTypeId = 1)  -- Paid Types
				THEN [claimStatusTransaction].LineItemPaidAmount
				ELSE 0
			END
			 AS LineItemPaidAmount
            ,[claimStatusTransaction].LineItemChargeAmount as LineItemChargeAmount
            ,[claimStatusTransaction].TotalAllowedAmount as TotalAllowedAmount
            ,[claimStatusTransaction].TotalNonAllowedAmount as NonAllowedAmount
            ,[claimStatusTransaction].CheckPaidAmount as CheckPaidAmount
            ,[claimStatusTransaction].CheckDate as CheckDate
            ,[claimStatusTransaction].CheckNumber as CheckNumber
            ,[claimStatusTransaction].ReasonDescription as ReasonDescription
            ,[claimStatusTransaction].RemarkCode as RemarkCode
            ,[claimStatusTransaction].RemarkDescription as RemarkDescription
            ,[claimStatusTransaction].CoinsuranceAmount as CoinsuranceAmount
            ,[claimStatusTransaction].CopayAmount as CopayAmount
            ,[claimStatusTransaction].DeductibleAmount as DeductibleAmount
            ,[claimStatusTransaction].CobAmount as CobAmount
            ,[claimStatusTransaction].PenalityAmount as PenalityAmount
            ,[claimStatusTransaction].EligibilityStatus as EligibilityStatus
            ,[claimStatusTransaction].EligibilityInsurance as EligibilityInsurance
            ,[claimStatusTransaction].EligibilityPolicyNumber as EligibilityPolicyNumber
            ,[claimStatusTransaction].EligibilityFromDate as EligibilityFromDate
            ,[claimStatusTransaction].VerifiedMemberId as VerifiedMemberId
            ,[claimStatusTransaction].CobLastVerified as CobLastVerified
            ,[claimStatusTransaction].LastActiveEligibleDateRange as LastActiveEligibleDateRange
            ,[claimStatusTransaction].PrimaryPayer as PrimaryPayer
            ,[claimStatusTransaction].PrimaryPolicyNumber as PrimaryPolicyNumber
            ,CONVERT(VARCHAR, [claimStatusTransaction].LastModifiedOn, 101) as TransactionDate
            ,CONVERT(VARCHAR, [claimStatusTransaction].LastModifiedOn, 108) as TransactionTime
            ,[claimStatusTransaction].PaymentType as PaymentType 
    		,[claimStatusTransaction].PtCapUsedAmount AS claimStatusTransaction_PtCapUsedAmount 
    		,[claimStatusTransaction].PtCapYearTo   AS claimStatusTransaction_PtCapYearTo
    		,[claimStatusTransaction].PtCapYearFrom   AS claimStatusTransaction_PtCapYearFrom
    		,[claimStatusTransaction].OtCapUsedAmount   AS claimStatusTransaction_OtCapUsedAmount
    		,[claimStatusTransaction].OtCapYearTo   AS claimStatusTransaction_OtCapYearTo
    		,[claimStatusTransaction].OtCapYearFrom   AS claimStatusTransaction_OtCapYearFrom
    		,[claimStatusTransaction].PartB_RemainingDeductible   AS claimStatusTransaction_PartB_RemainingDeductible
    		,[claimStatusTransaction].PartB_DeductibleTo   AS claimStatusTransaction_PartB_DeductibleTo
    		,[claimStatusTransaction].PartB_DeductibleFrom   AS claimStatusTransaction_PartB_DeductibleFrom
    		,[claimStatusTransaction].PartB_EligibilityFrom   AS claimStatusTransaction_PartB_EligibilityFrom
    		,[claimStatusTransaction].PartA_RemainingDeductible   AS claimStatusTransaction_PartA_RemainingDeductible
    		,[claimStatusTransaction].PartA_DeductibleToDate   AS claimStatusTransaction_PartA_DeductibleToDate
    		,[claimStatusTransaction].PartA_DeductibleFrom   AS claimStatusTransaction_PartA_DeductibleFrom
    		,[claimStatusTransaction].PartA_EligibilityTo   AS claimStatusTransaction_PartA_EligibilityTo
    		,[claimStatusTransaction].PartA_EligibilityFrom  AS claimStatusTransaction_PartA_EligibilityFrom
            ,CONVERT(VARCHAR, (SELECT TOP 1 CreatedOn FROM IntegratedServices.ClaimStatusTransactionHistories  WHERE ClaimStatusTransactionId = [claimStatusTransaction].Id 
             ORDER BY CreatedOn DESC), 101) as LastCheckedDate
            ,CONVERT(VARCHAR,  (SELECT TOP 1 CreatedOn FROM IntegratedServices.ClaimStatusTransactionHistories WHERE ClaimStatusTransactionId = [claimStatusTransaction].Id 
            ORDER BY CreatedOn DESC), 108) as LastCheckedTime
    
    '
    
    DECLARE @claimStatusBatchClaimsColumns NVARCHAR(MAX) = N'
         	
    		[claimStatusBatchClaim].[Id] AS ClaimStatusBatchClaimId 
    		,[claimStatusBatchClaim].[BilledAmount] 
    		,[claimStatusBatchClaim].[ClaimBilledOn] 
    		,[claimStatusBatchClaim].[ClaimNumber] as OfficeClaimNumber 
    		,[claimStatusBatchClaim].[DateOfServiceFrom] 
    		,[claimStatusBatchClaim].[DateOfServiceTo] 
    		,[claimStatusBatchClaim].[IsDeleted] AS [claimStatusBatchClaims_IsDeleted]
    		,[claimStatusBatchClaim].[IsSupplanted] AS [claimStatusBatchClaims_IsSupplanted]
    		,[claimStatusBatchClaim].[LastModifiedBy] AS [claimStatusBatchClaims_LastModifiedBy] 
    		,[claimStatusBatchClaim].[LastModifiedOn] AS [claimStatusBatchClaims_LastModifiedOn]
    		,CONVERT(VARCHAR, [claimStatusBatchClaim].CreatedOn, 101) as AitClaimReceivedDate
    		,CONVERT(VARCHAR, [claimStatusBatchClaim].CreatedOn, 108) as AitClaimReceivedTime
    		,[claimStatusBatchClaim].[PatientFirstName] AS [claimStatusBatchClaims_PatientFirstName] 
    		,[claimStatusBatchClaim].[PatientId] AS [claimStatusBatchClaims_PatientId]
    		,[claimStatusBatchClaim].[PatientLastName] AS [claimStatusBatchClaims_PatientLastName] 
    		,[claimStatusBatchClaim].[PolicyNumber] AS [claimStatusBatchClaims_PolicyNumber]
    		,[claimStatusBatchClaim].[ProcedureCode] AS [claimStatusBatchClaims_ProcedureCode]
    		,[claimStatusBatchClaim].[Quantity] AS [claimStatusBatchClaims_Quantity]
    		,[claimStatusBatchClaim].[ClientLocationId] AS [claimStatusBatchClaims_ClientLocationId]
    		,[claimStatusBatchClaim].[ClientInsuranceId] AS [claimStatusBatchClaims_ClientInsuranceId]
			,[claimStatusBatchClaim].ClaimStatusTransactionId as claimStatusBatchClaim_ClaimStatusTransactionId
            ,[claimStatusBatchClaim].ClaimLevelMd5Hash as claimStatusBatchClaim_ClaimLevelMd5Hash
    '
    
    DECLARE @claimStatusExceptionReasonColumns NVARCHAR(MAX) = N'
       ,[claimStatusTransaction].ClaimStatusExceptionReasonCategoryId as claimStatusExceptionReasonCategory_Id
     '
    
    DECLARE @ClientLocationColumns NVARCHAR(MAX)=N'
     ,[location].[Id] AS ClientLocation_Id
     ,[location].Npi AS ClientLocation_NPI
     ,[location].[Name] AS ClientLocation_Name
    '
    
    SET @claimStatusTransactionLineItemStatusChangeSQLColumns = N'
            ,claimStatusTransactionLineItemStatusChangẹ.PreviousClaimLineItemStatusId
            ,claimStatusTransactionLineItemStatusChangẹ.UpdatedClaimLineItemStatusId
    '

    SET @claimStatusTransactionLineItemStatusChangeJoinQuery = N'
            JOIN IntegratedServices.[ClaimStatusTransactionLineItemStatusChangẹs] as claimStatusTransactionLineItemStatusChangẹ on claimStatusTransactionLineItemStatusChangẹ.Id = [claimStatusTransaction].ClaimStatusTransactionLineItemStatusChangẹId    
    '
    
    SET @claimStatusTransactionLineItemStatusChangeWhereClause = N'
        AND (
             claimStatusTransactionLineItemStatusChangẹ.UpdatedClaimLineItemStatusId IN (SELECT CONVERT(int, value) FROM string_split(@DelimitedLineItemStatusIds, '','')) 
             OR (@DelimitedLineItemStatusIds IS NULL OR @DelimitedLineItemStatusIds = '''')
            )  
    '

    DECLARE @fromMainEntityInitializer NVARCHAR(MAX) = N'
        FROM [IntegratedServices].ClaimStatusBatchClaims as [claimStatusBatchClaim] 
    '
    
    IF(@FlattenedLineItemStatus IS NOT NULL AND LOWER(@FlattenedLineItemStatus) = LOWER(@in_Process) AND ((@DelimitedLineItemStatusIds IS NOT NULL OR @DelimitedLineItemStatusIds <> '') AND @DelimitedLineItemStatusIds = '10,17'))
        BEGIN
        
                SET @fnGetClaimLevelGroupingJoinClause =N'
                JOIN(SELECT (ClaimLevelMd5Hash) FROM [IntegratedServices].[fnGetClaimLevelGroups](@ClientId,NULL,@ReceivedFrom,@ReceivedTo,@DateOfServiceFrom,
        		@DateOfServiceTo,@TransactionDateFrom,@TransactionDateTo,@ClaimBilledFrom,@ClaimBilledTo,@ClientProviderIds,@ClientLocationIds,@ClientInsuranceIds,	        @ClientExceptionReasonCategoryIds,@ClientAuthTypeIds,@ClientProcedureCodes,@PatientId,@ClaimStatusBatchId)) 
        	    AS c2 ON c2.ClaimLevelMd5Hash = [claimStatusBatchClaim].ClaimLevelMd5Hash
        '
        END
    ELSE 
        BEGIN
            
            SET @fnGetClaimLevelGroupingJoinClause =N'
            JOIN(SELECT (ClaimLevelMd5Hash) FROM [IntegratedServices].[fnGetClaimLevelGroups](@ClientId,@DelimitedLineItemStatusIds,@ReceivedFrom,@ReceivedTo,@DateOfServiceFrom,
    		@DateOfServiceTo,@TransactionDateFrom,@TransactionDateTo,@ClaimBilledFrom,@ClaimBilledTo,@ClientProviderIds,@ClientLocationIds,@ClientInsuranceIds,	    @ClientExceptionReasonCategoryIds,@ClientAuthTypeIds,@ClientProcedureCodes,@PatientId,@ClaimStatusBatchId)) 
    	    AS c2 ON c2.ClaimLevelMd5Hash = [claimStatusBatchClaim].ClaimLevelMd5Hash
    '
        END
    
    SET @initialSummaryDashboardClaimLevelGroupingJoinClause = N'
    --JOIN(
    --				SELECT min(Id) as ''MinId'', EntryMd5Hash as ''HashKey'' 
    --				FROM  [IntegratedServices].ClaimStatusBatchClaims 
    --				GROUP BY EntryMd5Hash
    --            ) as c2 ON [claimStatusBatchClaim].Id = c2.MinId

    JOIN(
    		SELECT * FROM [IntegratedServices].[fnGetInitialClaimEntry](
    					@ClientId, 
    					NULL, 
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
    	) as [fnGetInitialClaim] ON claimStatusBatchClaim.Id = [fnGetInitialClaim].MinId 
    JOIN(
    		SELECT * FROM [IntegratedServices].[fnGetClaimLevelGroups]( 
    					@ClientId, 
    					NULL, 
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
    	) as [fnGetClaimLevelGroup] ON claimStatusBatchClaim.ClaimLevelMd5Hash = [fnGetClaimLevelGroup].ClaimLevelMd5Hash
    '
    
    DECLARE @claimStatusTransactionJoinClause NVARCHAR(MAX) = N'
    LEFT JOIN [IntegratedServices].ClaimStatusTransactions as [claimStatusTransaction] ON [claimStatusTransaction].ClaimStatusBatchClaimId = [claimStatusBatchClaim].Id
    '
    
    DECLARE @claimStatusBatchesJoinclause NVARCHAR(MAX)=N'
    JOIN [IntegratedServices].ClaimStatusBatches as [claimStatusBatch] ON [claimStatusBatchClaim].ClaimStatusBatchId = [claimStatusBatch].Id
    '
    
    DECLARE @authTypeJoinClause NVARCHAR(MAX) = N'
    LEFT JOIN [dbo].[AuthTypes] as [authType] ON [claimStatusBatch].AuthTypeId = [authType].Id
    '
    
    DECLARE @claimLineItenStatusJoinClause NVARCHAR(MAX) = N'
    LEFT JOIN [IntegratedServices].ClaimLineItemStatuses as [claimLineItemStatus] ON [claimStatusTransaction].ClaimLineItemStatusId = [claimLineItemStatus].Id
    '
    
    DECLARE @providerAndPersonCombinedJoinClause NVARCHAR(MAX) = N'
    LEFT JOIN [dbo].Providers as [provider] ON [claimStatusBatchClaim].ClientProviderId = [provider].Id
    LEFT JOIN [dbo].Persons as [person] On [provider].PersonId = [person].Id -- person for the provider
    
    LEFT JOIN [dbo].Patients as [patient] ON [claimStatusBatchClaim].PatientId = [patient].Id
    LEFT JOIN [dbo].Persons as [person_Patient] On [patient].PersonId = [person_Patient].Id
    '
    
    DECLARE @clientInsuranceJoinClause NVARCHAR(MAX) = N'
    INNER JOIN [dbo].ClientInsurances as [insurance] ON [claimStatusBatch].ClientInsuranceId = [insurance].Id
    '
    
    DECLARE @clientLocationJoinClause NVARCHAR(MAX) = N'
    LEFT JOIN [dbo].ClientLocations as [location] On [claimStatusBatchClaim].ClientLocationId = [location].Id
    '
    
    DECLARE @claimStatusExceptionReasonCategoryJoinClause NVARCHAR(MAX) = N'
    Left JOIN [IntegratedServices].ClaimStatusExceptionReasonCategories as [claimStatusExceptionReasonCategory] ON [claimStatusTransaction].ClaimStatusExceptionReasonCategoryId =  [claimStatusExceptionReasonCategory].Id
    '
    
    ------------------------Where Clause----------------------------------------
    
    SET @SqlWhereClause = N'
    				WHERE 
    					[claimStatusBatchClaim].[IsDeleted] = CAST(0 AS BIT) AND 
    					[claimStatusBatchClaim].[IsSupplanted] = CAST(0 AS BIT)
    					'
        
    IF(@clientId IS NOT NULL OR @clientId > 0)
        BEGIN
            SET @clientIdWhereClause+=N' 
                    AND [claimStatusBatchClaim].[ClientId] = @ClientId'--+CONVERT(NVARCHAR(MAX), @ClientId)          
        END
    
    
    IF(@ClaimStatusType IS NOT Null)
        BEGIN
            --Case for handling Calim status Types
           IF(@ClaimStatusType = @PaidClaimStatusType)
                Begin
					---filter data based on claimLineItemStatus.ClaimStatusTypeId = 1
					     SET @ClaimStatusTypeWhereClause = N'
					         AND ([claimLineItemStatus].ClaimStatusTypeId = @PaidClaimStatusType)
					'
				End
        
           IF(@ClaimStatusType = @DeniedClaimStatusType)
                Begin
                    IF(@ClaimStatusTypeStatus IS NOT NULL AND @ClaimStatusTypeStatus = @writeOffStatus)
                        BEGIN
                             ---filter data based on claimLineItemStatus.ClaimStatusTypeId = 2
                                 SET @ClaimStatusTypeWhereClause = N'
                                     AND ([claimLineItemStatus].ClaimStatusTypeId = @DeniedClaimStatusType 
                                            AND (claimStatusTransaction.WriteoffAmount IS NULL AND claimStatusTransaction.WriteoffAmount > 0)) 
                                            OR (claimStatusTransaction.ClaimLineItemStatusId = @writeOffStatusVal)
                                 '
                        END
                    ELSE
                        BEGIN
                             ---filter data based on claimLineItemStatus.ClaimStatusTypeId = 2
                             SET @ClaimStatusTypeWhereClause = N'
                                 AND ([claimLineItemStatus].ClaimStatusTypeId = @DeniedClaimStatusType AND (claimStatusTransaction.WriteoffAmount IS NULL OR    claimStatusTransaction.WriteoffAmount = 0))
                             '
                        END
                End
        
           IF(@ClaimStatusType = @OpenClaimStatusType)
                Begin
                    ---filter data based on claimLineItemStatus.ClaimStatusTypeId = 3
                    SET @ClaimStatusTypeWhereClause = N'
                        AND [claimLineItemStatus].ClaimStatusTypeId = @OpenClaimStatusType
                    '
                End
        
           IF(@ClaimStatusType = @OtherAdjudicatedClaimStatusType)
                Begin
                    ---filter data based on claimLineItemStatus.ClaimStatusTypeId = 4
                    SET @ClaimStatusTypeWhereClause = N'
                        AND [claimLineItemStatus].ClaimStatusTypeId = @OtherAdjudicatedClaimStatusType
                    '
                End
        
           IF(@ClaimStatusType = @OtherOpenClaimStatusType)
                Begin
                    ---filter data based on claimLineItemStatus.ClaimStatusTypeId = 5
                    SET @ClaimStatusTypeWhereClause = N'
                        AND [claimLineItemStatus].ClaimStatusTypeId = @OtherOpenClaimStatusType
                    '
                End
           
        END
    
    ELSE IF(@ClaimStatusTypeStatus IS NOT NULL AND @ClaimStatusTypeStatus = @OpenStatus)
        BEGIN
            --claimLineItemStatus.ClaimStatusTypeId in (3,5)
            SET @ClaimStatusTypeWhereClause = N'
                AND [claimLineItemStatus].ClaimStatusTypeId IN (@OpenClaimStatusType,@OtherOpenClaimStatusType)
            '
        END
    
	ELSE IF(@ClaimStatusTypeStatus IS NOT NULL AND @ClaimStatusTypeStatus = @ContractualStatus)
        BEGIN
             SET @ClaimStatusTypeWhereClause = N'
                AND claimStatusTransaction.ClaimLineItemStatusId = @contractualStatusVal
            '
            --SET @ClaimStatusTypeWhereClause = N'
	        			--AND ((claimLineItemStatus.ClaimStatusTypeId = 1 AND ((claimStatusBatchClaim.BilledAmount -       claimStatusTransaction.TotalAllowedAmount) <> 0)) 
	        			--		OR claimStatusTransaction.ClaimLineItemStatusId = @contractualStatusVal)';
                END
    
    ELSE IF ((@DelimitedLineItemStatusIds IS NOT NULL OR @DelimitedLineItemStatusIds <> ''))
        BEGIN
            
            IF(@FlattenedLineItemStatus IS NOT NULL AND LOWER(@FlattenedLineItemStatus) = LOWER(@in_Process))
                BEGIN
                  --Batch Claims without a transaction (ClaimstatusTransactionId is null) OR the claim has a transaction but the Status is error OR transient   error
                     SET @delimitedLineItemStatusIdsWhereClause += N' 
					 AND ([claimLineItemStatus].ClaimStatusTypeId is null OR [claimStatusBatchClaim].ClaimStatusTransactionId is NULL)
    					--AND ([claimStatusTransaction].Id IS NULL OR [claimStatusTransaction].ClaimLineItemStatusId IN (SELECT convert(int, value)
    	        		--FROM string_split(@DelimitedLineItemStatusIds, '','')) OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = '''')) 
					'
                END
            ELSE IF(@DelimitedLineItemStatusIds IS NOT NULL AND (@DelimitedLineItemStatusIds = @DenialStatusIds OR EXISTS (SELECT 1 FROM string_split(@DenialStatusIds, ',') AS DenialStatus WHERE CONVERT(INT, DenialStatus.value) IN (SELECT CONVERT(INT, value) FROM string_split(@DelimitedLineItemStatusIds, ',')))))
                BEGIN
                   SET @delimitedLineItemStatusIdsWhereClause += N'
                    AND 
                        (([claimStatusTransaction].WriteoffAmount IS NULL OR [claimStatusTransaction].WriteoffAmount = 0)
                            AND ([claimStatusTransaction].ClaimLineItemStatusId IN (SELECT convert(int, value)
    		                    FROM string_split(@DelimitedLineItemStatusIds, '','')) OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = ''''))) '
                END
            ELSE
                BEGIN
                    SET @delimitedLineItemStatusIdsWhereClause += N' 
    	                AND ([claimStatusTransaction].ClaimLineItemStatusId IN (SELECT convert(int, value)
    		                FROM string_split(@DelimitedLineItemStatusIds, '','')) OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = '''')) '
                END
        END
    
    
    IF (@PatientId IS NOT NULL OR @PatientId <> '')
        BEGIN
    
           SET @patientWhereClause += N' 
    	   AND (([claimStatusBatchClaim].PatientId = @PatientId) OR (@PatientId IS NULL) OR @PatientId = '''') '
        END
    
    IF (@ClaimStatusBatchId IS NOT NULL OR @ClaimStatusBatchId <> '' OR (@ClaimStatusBatchId = 0))
        BEGIN
    		
           SET @ClaimStatusBatchIdWhereClause += N' 
    	   AND (([claimStatusBatchClaim].ClaimStatusBatchId = @ClaimStatusBatchId) OR (@ClaimStatusBatchId IS NULL)  OR (@ClaimStatusBatchId = 0)) '
        END
    
    IF (@ClientProviderIds IS NOT NULL OR @ClientProviderIds <> '')
        BEGIN
    
           SET @providerWhereClause += N' 
    	   And ([provider].Id in (SELECT convert(int, value)
    										FROM string_split(@ClientProviderIds, '','')) OR (@ClientProviderIds is null OR @ClientProviderIds = '''')) '
        END
    
    IF (@ClientLocationIds IS NOT NULL OR @ClientLocationIds <> '')
        BEGIN
           SET @locationWhereClause += N' 
    	   AND ([claimStatusBatchClaim].[ClientLocationId] IN (SELECT CONVERT(INT, value) FROM STRING_SPLIT(@ClientLocationIds, '','')) 
                    OR (@ClientLocationIds IS NULL OR @ClientLocationIds = '''')) '
        END
    
    IF (@ClientInsuranceIds IS NOT NULL OR @ClientInsuranceIds <> '')
        BEGIN
            SET @InsuranceWhereClause += N' 
                AND ([claimStatusBatch].[ClientInsuranceId] IN (SELECT CONVERT(INT, value) FROM STRING_SPLIT(@ClientInsuranceIds, '',''))
                    OR (@ClientInsuranceIds IS NULL OR @ClientInsuranceIds = '''')) '
        END
    	
    IF (@ClientProcedureCodes IS NOT NULL OR @ClientProcedureCodes <> '')
        BEGIN
            SET @ProcedureCodesWhereClause += N' 
                And ([claimStatusBatchClaim].[ProcedureCode] IN (SELECT convert(nvarchar(MAX), value)
    			FROM string_split(@ClientProcedureCodes, '','')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''''))'
        END
    	
    IF (@ClientExceptionReasonCategoryIds IS NOT NULL OR @ClientExceptionReasonCategoryIds <> '')
        BEGIN
            SET @ExceptionReasonCategoryWhereClause += N' 
                And ((@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = '''') OR ([claimStatusTransaction].[ClaimStatusExceptionReasonCategoryId]
    				IN (SELECT convert(int, value) FROM string_split(@ClientExceptionReasonCategoryIds, '','')))) '
        END
    	
    IF (@ClientAuthTypeIds IS NOT NULL OR @ClientAuthTypeIds <> '')
        BEGIN
    
            SET @AuthTypeWhereClause += N' 
                And ([claimStatusBatch].[AuthTypeId] IN (SELECT convert(int, value)
    			FROM string_split(@ClientAuthTypeIds, '','')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = '''' ))'
        END
    
    IF((@ReceivedFrom IS NOT NULL OR @ReceivedFrom <> '') OR (@ReceivedTo IS NOT NULL OR @ReceivedTo <> ''))
    BEGIN
    	
    	SET @receivedDateWhereClause = N' 	
    	AND (([claimStatusBatchClaim].CreatedOn >= @ReceivedFrom) OR @ReceivedFrom IS NULL)
    										AND (([claimStatusBatchClaim].CreatedOn <= @ReceivedTo) OR @ReceivedTo IS NULL) '
    
    END
    
    IF((@ClaimBilledFrom IS NOT NULL OR @ClaimBilledFrom <> '') OR (@ClaimBilledTo IS NOT NULL OR @ClaimBilledTo <> ''))
    BEGIN
    	
    	SET @claimBilledDateWhereClause = N' 
    	AND (([claimStatusBatchClaim].ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
    											AND (([claimStatusBatchClaim].ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null) '
    
    END
    
    IF((@TransactionDateFrom IS NOT NULL OR @TransactionDateFrom <> '') OR ( @TransactionDateTo IS NOT NULL OR  @TransactionDateTo <> ''))
    BEGIN
    	
    	SET @transactionDateWhereClause = N' 	
    	AND ((COALESCE([claimStatusTransaction].LastModifiedOn, [claimStatusTransaction].CreatedOn) >= @TransactionDateFrom) OR @TransactionDateFrom is NULL )
    											AND ((COALESCE([claimStatusTransaction].LastModifiedOn, [claimStatusTransaction].CreatedOn) <= @TransactionDateTo) OR   @TransactionDateTo is NULL) '
    
    END
    
    IF((@DateOfServiceFrom IS NOT NULL OR @DateOfServiceFrom <> '') OR (@DateOfServiceTo IS NOT NULL OR @DateOfServiceTo <> ''))
    BEGIN
    	
    	SET @dateOfServiceDateWhereClause = N' 
    	AND (([claimStatusBatchClaim].DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom IS NULL)
    										AND (([claimStatusBatchClaim].DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo IS NULL) '
    
    END
    
    ----------------------------------------------------------------
    
    
    
    Declare @SqlColumns NVARCHAR(MAX)= CONCAT(
    	@selectColumnInitializer
    	,@claimStatusBatchClaimsColumns
    	,@claimStatusBatchesColumns
    	,@claimStatusTransactionsColumns
    	,@person_patientColumns
    	,@authTypeColumns
    	,@insuranceColumns
    	,@claimLineItemStatusesColumns
    	,@providerColumns
        ,@ClientLocationColumns
    	,@claimStatusExceptionReasonColumns  --Commented for now, will take action after discussion
    )
    
    
    DECLARE @dynamicQueryColumns NVARCHAR(MAX)= CONCAT(
        @SqlColumns,
        @additionalPatientSQLColumns,
        @additionalReceivedDateSQLColumns,
        @additionalDateOfServiceDateSQLColumns,
        @additionalTransactionDateSQLColumns,
        @additionalClaimBilledDateSQLColumns,
        @additionalClaimStatusBatchIdSQLColumns,
        @additionalProviderSQLColumns,
        @additionalDelimitedLineItemStatusIdsSQLColumns,
        @additionalLocationSQLColumns,
        @additionalInsuranceSQLColumns,
        @additionalAuthTypeSQLColumns,
        @additionalExceptionReasonCategorySQLColumns,
        @additionalProcedureCodesSQLColumns
    )
    
    
    --PRINT(@dynamicQueryColumns)
    
    ----Comment for testing only----
if(@HasTest = CAST(0 AS BIT))
    BEGIN
	    Set @dynamicQueryColumns = '
        Select 
            SUM([claimStatusBatchClaim].[BilledAmount]) as Charges,
            SUM([claimStatusTransaction].TotalAllowedAmount) as TotalAllowedAmount, 
            COUNT([claimStatusBatchClaim].ClaimLevelMd5Hash) as Actual_LineItems, 
            COUNT(DISTINCT [claimStatusBatchClaim].ClaimLevelMd5Hash) as Disctinct_LineItems
        '+@fromMainEntityInitializer
	END
    
    --IF (@DashboardType IS NULL OR LOWER(@DashboardType) <> LOWER('InitialSummary') OR LOWER(@DashboardType) <> LOWER(@ProceduresDashboard))
    IF (@DashboardType IS NULL OR @DashboardType = '')
     BEGIN
         SET @customDashboardTypeGroupingJoinQuery = CONCAT(@fnGetClaimLevelGroupingJoinClause,N'')
     END
    ELSE IF (@DashboardType IS NOT NULL AND LOWER(@DashboardType) = LOWER(@ProceduresDashboard))
     BEGIN
        SET @customDashboardTypeGroupingJoinQuery=N'';
     END
    ELSE IF (@DashboardType IS NOT NULL AND LOWER(@DashboardType) = LOWER('InitialSummary'))
     BEGIN
        SET @customDashboardTypeGroupingJoinQuery = CONCAT(@initialSummaryDashboardClaimLevelGroupingJoinClause,N'')
     END
    
     --PRINT(@customDashboardTypeGroupingJoinQuery)
    
    DECLARE @dynamicJoinQuery NVARCHAR(MAX) = CONCAT(
        @customDashboardTypeGroupingJoinQuery
        ,@claimStatusTransactionJoinClause
        ,@claimStatusBatchesJoinclause
        ,@authTypeJoinClause
        ,@claimLineItenStatusJoinClause
        ,@clientInsuranceJoinClause
        ,@providerAndPersonCombinedJoinClause
        ,@clientLocationJoinClause
        --,@claimStatusExceptionReasonCategoryJoinClause --Comment for now will take action after discussion with kevin
    )
    
    --PRINT(@dynamicJoinQuery)
    
    DECLARE @dynamicWhereClause NVARCHAR(MAX) =  CONCAT(
    	@SqlWhereClause
        ,@clientIdWhereClause
        ,@patientWhereClause)
    	
    
    --Included Date Based grouping for Procedure Dashboard --
    IF (@DashboardType IS NOT NULL AND LOWER(@DashboardType) = LOWER(@ProceduresDashboard))
    BEGIN
        PRINT('Dashboard Type: '+@DashboardType)

        SET @dynamicJoinQuery = CONCAT(@dynamicJoinQuery,
        @claimStatusExceptionReasonCategoryJoinClause)

        SET @dynamicWhereClause = CONCAT(@dynamicWhereClause,@receivedDateWhereClause
        ,@dateOfServiceDateWhereClause
        ,@transactionDateWhereClause
        ,@claimBilledDateWhereClause)
    END

    SET @dynamicWhereClause = CONCAT(@dynamicWhereClause
        ,@ClaimStatusBatchIdWhereClause
        ,@providerWhereClause
        ,@delimitedLineItemStatusIdsWhereClause
        ,@locationWhereClause
        ,@InsuranceWhereClause
        ,@AuthTypeWhereClause
        ,@ExceptionReasonCategoryWhereClause
        ,@ProcedureCodesWhereClause
    	,@ExceptionReasonCategoryWhereClause
        ,@ClaimStatusTypeWhereClause
    )

    
    IF(@HasIncludeClaimStatusTransactionLineItemStatusChange = CAST(1 AS BIT))--If true then add.
    BEGIN
       SET @dynamicQueryColumns = CONCAT(@dynamicQueryColumns, @claimStatusTransactionLineItemStatusChangeSQLColumns)
       SET @dynamicJoinQuery = CONCAT(@dynamicJoinQuery, @claimStatusTransactionLineItemStatusChangeJoinQuery)
       SET @dynamicWhereClause = CONCAT(@dynamicWhereClause, @claimStatusTransactionLineItemStatusChangeWhereClause)
    END

    SET @dynamicQueryColumns = CONCAT(@dynamicQueryColumns, @fromMainEntityInitializer)

    DECLARE @DynamicSQL NVARCHAR(MAX) = CONCAT(@dynamicQueryColumns,@dynamicJoinQuery,@dynamicWhereClause)
    
    Print('Dynamic SQL Query Executed')
    --PRINT(@DynamicSQL)
    
    EXEC sp_executesql @DynamicSQL, 
                       N'@ClientId INT, 
                         @ClientLocationIds NVARCHAR(MAX), 
                         @ClientInsuranceIds NVARCHAR(MAX),
                         @ClientAuthTypeIds NVARCHAR(MAX),
                         @ClientExceptionReasonCategoryIds NVARCHAR(MAX),
                         @ClientProcedureCodes NVARCHAR(MAX),
                         @DelimitedLineItemStatusIds NVARCHAR(MAX),
                         @ClientProviderIds NVARCHAR(MAX),
                         @PatientId INT,
                         @ClaimStatusBatchId INT,
                         @ReceivedFrom DATETIME,
                         @ReceivedTo DATETIME,
                         @DateOfServiceFrom DATETIME,
                         @DateOfServiceTo DATETIME,
                         @TransactionDateFrom DATETIME,
                         @TransactionDateTo DATETIME,
                         @ClaimBilledFrom DATETIME,
                         @ClaimBilledTo DATETIME,
                         @FlattenedLineItemStatus NVARCHAR(MAX),
                         @DashboardType NVARCHAR(MAX),
                         @ClaimStatusType int,
                         @PaidClaimStatusType INT = 1,
                         @DeniedClaimStatusType INT = 2,
                         @OpenClaimStatusType INT = 3,
                         @OtherAdjudicatedClaimStatusType INT = 4,
                         @OtherOpenClaimStatusType INT = 5,
                         @ClaimStatusTypeStatus NVARCHAR(MAX),
                         @writeOffStatus NVARCHAR(MAX) = ''WriteOff'',
                         @OpenStatus NVARCHAR(MAX)= ''Open'',
                         @ContractualStatus NVARCHAR(MAX)= ''Contractual'',
                         @writeOffStatusVal int = 20,
                         @contractualStatusVal int = 22',
                       @ClientId, 
                       @ClientLocationIds, 
                       @ClientInsuranceIds,
                       @ClientAuthTypeIds,
                       @ClientExceptionReasonCategoryIds,
                       @ClientProcedureCodes,
                       @DelimitedLineItemStatusIds,
                       @ClientProviderIds,
                       @PatientId,
                       @ClaimStatusBatchId,
                       @ReceivedFrom,
                       @ReceivedTo,
                       @DateOfServiceFrom,
                       @DateOfServiceTo,
                       @TransactionDateFrom,
                       @TransactionDateTo,
                       @ClaimBilledFrom,
                       @ClaimBilledTo,
                       @FlattenedLineItemStatus,
                       @DashboardType,
                       @ClaimStatusType,
                       @PaidClaimStatusType,
                       @DeniedClaimStatusType,
                       @OpenClaimStatusType,
                       @OtherAdjudicatedClaimStatusType,
                       @OtherOpenClaimStatusType,
                       @ClaimStatusTypeStatus,
                       @writeOffStatus,
                       @OpenStatus,
                       @ContractualStatus,
                       @writeOffStatusVal,
                       @contractualStatusVal
    
    
END

