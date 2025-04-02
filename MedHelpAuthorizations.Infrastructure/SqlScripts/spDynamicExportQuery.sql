CREATE OR ALTER   PROCEDURE [dbo].[spDynamicExport]
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
WITH RECOMPILE
AS


BEGIN
    SET NOCOUNT ON
  
    DECLARE @SQL NVARCHAR(MAX) = N''
	DECLARE @SQLColumns VARCHAR(MAX) = N''
    DECLARE @claimStatusBatchClaimInnerJoin VARCHAR(MAX)= N''
	DECLARE @whereClause NVARCHAR(MAX) = N''
    DECLARE @fromClause VARCHAR(MAX)= N''
	DECLARE @locationJoinClause NVARCHAR(MAX)= N''
	DECLARE @InsuranceJoinClause NVARCHAR(MAX)= N''
	DECLARE @AuthTypeJoinClause NVARCHAR(MAX)= N''
	DECLARE @ExceptionReasonCategoryJoinClause NVARCHAR(MAX)= N''
    DECLARE @providerJoinClause NVARCHAR(MAX)= N''
    DECLARE @claimStatusBatchIdJoinClause NVARCHAR(MAX)= N''


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

SET @SQLColumns = 'SELECT
                [claimStatusTransactions].[Id] AS ClaimStatusTransactionId, 
				[claimStatusTransactions].[CheckDate] AS ClaimStatusTransaction_CheckDate, 
				[claimStatusTransactions].[CheckNumber] AS ClaimStatusTransaction_CheckNumber, 
				[claimStatusTransactions].[CheckPaidAmount] AS ClaimStatusTransaction_CheckPaidAmount, 
				[claimStatusTransactions].[ClaimLineItemStatusId] AS ClaimStatusTransaction_ClaimLineItemStatusId , 
				[claimStatusTransactions].[ClaimLineItemStatusValue] AS ClaimStatusTransaction_ClaimLineItemStatusValue, 
				[claimStatusTransactions].[ClaimNumber] AS ClaimStatusTransaction_PayerClaimNumber,  
				[claimStatusTransactions].[ClientId] AS ClaimStatusTransaction_ClientId, 
				[claimStatusTransactions].[CobAmount] AS ClaimStatusTransaction_CobAmount, 
				[claimStatusTransactions].[CobLastVerified] AS ClaimStatusTransaction_CobLastVerified, 
				[claimStatusTransactions].[CoinsuranceAmount] AS ClaimStatusTransaction_CoinsuranceAmount, 
				[claimStatusTransactions].[CopayAmount] AS ClaimStatusTransaction_CopayAmount, 
				[claimStatusTransactions].[CreatedBy] AS ClaimStatusTransaction_CreatedBy, 
				[claimStatusTransactions].[CreatedOn] AS ClaimStatusTransaction_CreatedOn, 
				[claimStatusTransactions].[DeductibleAmount] AS ClaimStatusTransaction_DeductibleAmount, 
				[claimStatusTransactions].[EligibilityFromDate] AS ClaimStatusTransaction_EligibilityFromDate, 
				[claimStatusTransactions].[EligibilityInsurance] AS ClaimStatusTransaction_EligibilityInsurance, 
				[claimStatusTransactions].[EligibilityPolicyNumber] AS ClaimStatusTransaction_EligibilityPolicyNumber, 
				[claimStatusTransactions].[EligibilityStatus] AS ClaimStatusTransaction_EligibilityStatus, 
				[claimStatusTransactions].[ExceptionReason] AS ExceptionReason, 
				[claimStatusTransactions].[ExceptionRemark], 
				[claimStatusTransactions].[IsDeleted] AS ClaimStatusTransaction_IsDeleted, 
				[claimStatusTransactions].[LastActiveEligibleDateRange] AS ClaimStatusTransaction_LastActiveEligibleDateRange, 
				[claimStatusTransactions].[LastModifiedBy] AS ClaimStatusTransaction_LastModifiedBy, 
				[claimStatusTransactions].[LastModifiedOn] AS ClaimStatusTransaction_LastModifiedOn, 
				[claimStatusTransactions].[LineItemControlNumber] AS PayerLineItemControlNumber, 
				[claimStatusTransactions].[LineItemPaidAmount] AS ClaimStatusTransaction_LineItemPaidAmount, 
				[claimStatusTransactions].[PenalityAmount] AS ClaimStatusTransaction_PenalityAmount, 
				[claimStatusTransactions].[PrimaryPayer] AS ClaimStatusTransaction_PrimaryPayer, 
				[claimStatusTransactions].[PrimaryPolicyNumber] AS ClaimStatusTransaction_PrimaryPolicyNumber, 
				[claimStatusTransactions].[ReasonCode] AS ClaimStatusTransaction_ReasonCode, 
				[claimStatusTransactions].[ReasonDescription] AS ClaimStatusTransaction_ReasonDescription, 
				[claimStatusTransactions].[RemarkCode] AS ClaimStatusTransaction_RemarkCode, 
				[claimStatusTransactions].[RemarkDescription] AS ClaimStatusTransaction_RemarkDescription, 
				[claimStatusTransactions].[TotalAllowedAmount] AS ClaimStatusTransaction_TotalAllowedAmount, 
				[claimStatusTransactions].[TotalNonAllowedAmount] AS ClaimStatusTransaction_TotalNonAllowedAmount, 
				[claimStatusTransactions].[VerifiedMemberId] AS ClaimStatusTransaction_VerifiedMemberId,
				[claimStatusTransactions].[LineItemControlNumber] as claimStatusTransactions_LineItemControlNumber, 
				[claimStatusTransactions].[ClaimStatusBatchClaimId] as claimStatusTransactions_ClaimStatusBatchClaimId,

				[claimStatusBatchClaims].[Id] AS ClaimStatusBatchClaimId, 				[claimStatusBatchClaims].[BilledAmount], 
				[claimStatusBatchClaims].[ClaimBilledOn], 
				[claimStatusBatchClaims].[ClaimNumber], 
				[claimStatusBatchClaims].[DateOfBirth],
				[claimStatusBatchClaims].[DateOfServiceFrom], 
				[claimStatusBatchClaims].[DateOfServiceTo], 
				[claimStatusBatchClaims].[IsDeleted] AS [claimStatusBatchClaims_IsDeleted],
				[claimStatusBatchClaims].[IsSupplanted],
				[claimStatusBatchClaims].[LastModifiedBy] AS [claimStatusBatchClaims_LastModifiedBy], 
				[claimStatusBatchClaims].[LastModifiedOn] AS [claimStatusBatchClaims_LastModifiedOn],
				[claimStatusBatchClaims].[CreatedBy] AS [claimStatusBatchClaims_CreatedBy], 
				[claimStatusBatchClaims].[CreatedOn] AS [claimStatusBatchClaims_CreatedOn], 
				[claimStatusBatchClaims].[PatientFirstName], 
				[claimStatusBatchClaims].[PatientId],
				[claimStatusBatchClaims].[PatientLastName], 
				[claimStatusBatchClaims].[PolicyNumber],
				[claimStatusBatchClaims].[ProcedureCode],
				[claimStatusBatchClaims].[Quantity],
				[claimStatusBatchClaims].[ClientLocationId],

				[claimStatusBatches].[ClientInsuranceId],
				[claimStatusBatches].[BatchNumber] AS ClaimStatusBatch_BatchNumber, 
				[claimStatusBatches].[CreatedOn] AS ClaimStatusBatch_ClaimReceivedDate'

SET @fromClause = '
					FROM [IntegratedServices].[ClaimStatusTransactions] AS [claimStatusTransactions]'

SET @claimStatusBatchClaimInnerJoin = N'
					INNER JOIN (
					    SELECT [claimStatusBatchClaims].[Id], 
							[claimStatusBatchClaims].[BilledAmount], 
							[claimStatusBatchClaims].[ClaimBilledOn], 
							[claimStatusBatchClaims].[ClaimNumber], 
							[claimStatusBatchClaims].[DateOfBirth],
							[claimStatusBatchClaims].[DateOfServiceFrom], 
							[claimStatusBatchClaims].[DateOfServiceTo], 
							[claimStatusBatchClaims].[IsDeleted], 
							[claimStatusBatchClaims].[IsSupplanted],
							[claimStatusBatchClaims].[LastModifiedBy], 
							[claimStatusBatchClaims].[LastModifiedOn],
							[claimStatusBatchClaims].[CreatedBy], 
							[claimStatusBatchClaims].[CreatedOn], 
							[claimStatusBatchClaims].[PatientFirstName], 
							[claimStatusBatchClaims].[PatientId],
							[claimStatusBatchClaims].[PatientLastName], 
							[claimStatusBatchClaims].[PolicyNumber],
							[claimStatusBatchClaims].[ProcedureCode],
							[claimStatusBatchClaims].[Quantity],
							[claimStatusBatchClaims].[ClaimStatusBatchId],
							[claimStatusBatchClaims].[ClientLocationId]
					    FROM [IntegratedServices].[ClaimStatusBatchClaims] AS [claimStatusBatchClaims]
					    WHERE [claimStatusBatchClaims].[IsDeleted] = CAST(0 AS bit)
					) AS [claimStatusBatchClaims] ON [claimStatusTransactions].[ClaimStatusBatchClaimId] = [claimStatusBatchClaims].[Id]

					INNER JOIN (
					    SELECT [claimStatusBatches].[Id], 
							[claimStatusBatches].[BatchNumber], 
							[claimStatusBatches].[CreatedOn],
							[claimStatusBatches].[LastModifiedOn],
							[claimStatusBatches].[AuthTypeId], 
							[claimStatusBatches].[ClientInsuranceId]
					    FROM [IntegratedServices].[ClaimStatusBatches] AS [claimStatusBatches]
					    WHERE [claimStatusBatches].[IsDeleted] = CAST(0 AS bit)
					) AS [claimStatusBatches] ON [claimStatusBatchClaims].[ClaimStatusBatchId] = [claimStatusBatches].[Id]
					'

               
SET @whereClause = N'
				WHERE [claimStatusBatchClaims].[IsSupplanted] = CAST(0 AS BIT)
                    AND [claimStatusBatchClaims].[IsDeleted] = CAST(0 AS BIT)
                    AND [claimStatusTransactions].[IsDeleted] = CAST(0 AS BIT)
                    AND [claimStatusTransactions].[ClientId] = '+CONVERT(NVARCHAR(MAX), @ClientId)

IF (@DelimitedLineItemStatusIds IS NOT NULL or @DelimitedLineItemStatusIds <> '')
    BEGIN
        
       SET @delimitedLineItemStatusIdsWhereClause += N' 
	   AND ([claimStatusTransactions].ClaimLineItemStatusId IN (SELECT convert(int, value)
		FROM string_split(@DelimitedLineItemStatusIds, '','')) OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = '''')) '
    END

IF (@PatientId IS NOT NULL or @PatientId <> '')
    BEGIN

       SET @patientWhereClause += N' 
	   AND (([claimStatusBatchClaims].PatientId = @PatientId) OR (@PatientId IS NULL) OR @PatientId = '''') '
    END

IF (@ClaimStatusBatchId IS NOT NULL or @ClaimStatusBatchId <> '')
    BEGIN
		
	   SET @claimStatusBatchIdJoinClause = N' 
	   JOIN [IntegratedServices].ClaimStatusBatches as [ClaimStatusBatches] ON [claimStatusBatchClaims].ClaimStatusBatchId = [ClaimStatusBatches].Id '
       SET @ClaimStatusBatchIdWhereClause += N' 
	   AND (([claimStatusBatchClaims].ClaimStatusBatchId = @ClaimStatusBatchId) OR (@ClaimStatusBatchId IS NULL)  OR (@ClaimStatusBatchId = 0)) '
    END


IF (@ClientProviderIds IS NOT NULL or @ClientProviderIds <> '')
    BEGIN
        SET @providerJoinClause += N' 
		LEFT JOIN [dbo].Providers as [provider] ON [claimStatusBatchClaims].ClientProviderId = [provider].Id '

       SET @providerWhereClause += N' 
	   And ([provider].Id in (SELECT convert(int, value)
										FROM string_split(@ClientProviderIds, '','')) OR (@ClientProviderIds is null OR @ClientProviderIds = '''')) '
    END



IF (@ClientLocationIds IS NOT NULL or @ClientLocationIds <> '')
    BEGIN
        SET @locationJoinClause += N' 
		LEFT JOIN [dbo].[ClientLocations] AS [clientLocations] ON [claimStatusBatchClaims].[ClientLocationId] = [clientLocations].[Id] '

       SET @locationWhereClause += N' 
	   AND [claimStatusBatchClaims].[ClientLocationId] IN (SELECT CONVERT(INT, value) FROM STRING_SPLIT(@ClientLocationIds, '','')) 
                OR @ClientLocationIds IS NULL OR @ClientLocationIds = '''' '
    END


IF (@ClientInsuranceIds IS NOT NULL or @ClientInsuranceIds <> '')
    BEGIN
        SET @InsuranceWhereClause += N' 
            AND [claimStatusBatches].[ClientInsuranceId] IN (SELECT CONVERT(INT, value) FROM STRING_SPLIT(@ClientInsuranceIds, '',''))
                OR @ClientInsuranceIds IS NULL OR @ClientInsuranceIds = '' '
    END
	
IF (@ClientProcedureCodes IS NOT NULL or @ClientProcedureCodes <> '')
    BEGIN
        SET @ProcedureCodesWhereClause += N' 
            And (claimStatusBatchClaims.[ProcedureCode] IN (SELECT convert(nvarchar(16), value)			FROM string_split(@ClientProcedureCodes, '','')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''''))'
    END
	
IF (@ClientExceptionReasonCategoryIds IS NOT NULL or @ClientExceptionReasonCategoryIds <> '')
    BEGIN
        SET @ExceptionReasonCategoryWhereClause += N' 
            And ((@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = '''') OR ([claimStatusTransactions].[ClaimStatusExceptionReasonCategoryId]				IN (SELECT convert(int, value) FROM string_split(@ClientExceptionReasonCategoryIds, '','')))) '
    END
	
IF (@ClientAuthTypeIds IS NOT NULL or @ClientAuthTypeIds <> '')
    BEGIN
	    SET @AuthTypeJoinClause += N'									 LEFT JOIN [dbo].[Providers] AS [providers] ON claimStatusBatchClaims.[ClientProviderId] = [providers].[Id]								     LEFT JOIN [dbo].[Persons] AS [p3] ON [providers].[PersonId] = [p3].[Id]'

        SET @AuthTypeWhereClause += N' 
            And ([claimStatusBatches].[AuthTypeId] IN (SELECT convert(int, value)			FROM string_split(@ClientAuthTypeIds, '','')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = '''' ))'
    END

IF((@ReceivedFrom IS NOT NULL OR @ReceivedFrom <> '') OR (@ReceivedTo IS NOT NULL OR @ReceivedTo <> ''))
BEGIN
	
	SET @receivedDateWhereClause = N' 	
	AND (([claimStatusBatchClaims].CreatedOn >= @ReceivedFrom) OR @ReceivedFrom IS NULL)
										AND (([claimStatusBatchClaims].CreatedOn <= @ReceivedTo) OR @ReceivedTo IS NULL) '

END


IF((@ClaimBilledFrom IS NOT NULL OR @ClaimBilledFrom <> '') OR (@ClaimBilledTo IS NOT NULL OR @ClaimBilledTo <> ''))
BEGIN
	
	SET @claimBilledDateWhereClause = N' 
	AND (([claimStatusBatchClaims].ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
											AND (([claimStatusBatchClaims].ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null) '

END


IF((@TransactionDateFrom IS NOT NULL OR @TransactionDateFrom <> '') OR ( @TransactionDateTo IS NOT NULL OR  @TransactionDateTo <> ''))
BEGIN
	
	SET @transactionDateWhereClause = N' 	
	AND ((COALESCE([claimStatusTransactions].LastModifiedOn, [claimStatusTransactions].CreatedOn) >= @TransactionDateFrom) OR @TransactionDateFrom is NULL )
											AND ((COALESCE([claimStatusTransactions].LastModifiedOn, [claimStatusTransactions].CreatedOn) <= @TransactionDateTo) OR @TransactionDateTo is NULL) '

END


IF((@DateOfServiceFrom IS NOT NULL OR @DateOfServiceFrom <> '') OR (@DateOfServiceTo IS NOT NULL OR @DateOfServiceTo <> ''))
BEGIN
	
	SET @dateOfServiceDateWhereClause = N' 
	AND (([claimStatusBatchClaims].DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom IS NULL)
										AND (([claimStatusBatchClaims].DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo IS NULL) '

END



DECLARE @DynamicSQL NVARCHAR(MAX)


SET @DynamicSQL = @SQLColumns + @fromClause + @claimStatusBatchClaimInnerJoin + @locationJoinClause + @InsuranceJoinClause + @AuthTypeJoinClause + @ExceptionReasonCategoryJoinClause +@providerJoinClause +@claimStatusBatchIdJoinClause
				  + @whereClause +@delimitedLineItemStatusIdsWhereClause + @locationWhereClause + @InsuranceWhereClause + @AuthTypeWhereClause + @ExceptionReasonCategoryWhereClause +@ProcedureCodesWhereClause 
				  + @patientWhereClause + @providerWhereClause + @receivedDateWhereClause + @dateOfServiceDateWhereClause +@transactionDateWhereClause +@claimBilledDateWhereClause + @ClaimStatusBatchIdWhereClause

Print('dynamic data')

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
                     @ClaimBilledTo DATETIME',
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
                   @ClaimBilledTo;

END


