SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER   PROCEDURE [IntegratedServices].[spGetClaimStatusVisitsForProcedureDashboard]
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
Select 
	COUNT(DISTINCT(claimStatusBatchClaim.ClaimLevelMd5Hash)) as 'Quantity'
	, SUM(claimStatusBatchClaim.BilledAmount) as 'BilledAmt'
	, SUM(claimStatusTransaction.TotalAllowedAmount) as 'AllowedAmountSum'
	, SUM(claimStatusTransaction.LineItemPaidAmount) as 'PaidAmountSum'
	,COUNT( DISTINCT(
				CASE 
				WHEN (claimLineItemStatus.ClaimStatusTypeId in (1,2,4))  -- Paid, Denied , OtherAdjudicated types
					THEN claimStatusBatchClaim.ClaimLevelMd5Hash
				END
				)) AS 'AdjudicatedVisits'
	,SUM(
				CASE 
				WHEN (claimLineItemStatus.ClaimStatusTypeId in (1,2,4)) -- Paid, Denied , OtherAdjudicated types
					THEN claimStatusBatchClaim.BilledAmount
				END
				) AS 'AdjudicatedTotals'
	,COUNT( DISTINCT(
				CASE 
					WHEN (claimLineItemStatus.ClaimStatusTypeId = 1) -- Paid Types
					THEN claimStatusBatchClaim.ClaimLevelMd5Hash
				END
				)) AS 'AllowedVisits'
	,SUM(
				CASE 
					WHEN (claimLineItemStatus.ClaimStatusTypeId = 1)  -- Paid Types
					THEN claimStatusTransaction.TotalAllowedAmount
				END
				) AS 'AllowedTotals'
	,COUNT( DISTINCT(
				CASE 
					WHEN (claimLineItemStatus.ClaimStatusTypeId = 1)  -- Paid Types
					THEN claimStatusBatchClaim.ClaimLevelMd5Hash
				END
				)) AS 'PaidVisits'
	,SUM(
				CASE 
					WHEN (claimLineItemStatus.ClaimStatusTypeId = 1)  -- Paid Types
					THEN claimStatusTransaction.LineItemPaidAmount
				END
				) AS 'PaidTotals'
	,SUM(
                CASE 
                    WHEN  (claimLineItemStatus.ClaimStatusTypeId = 1 AND (claimStatusBatchClaim.BilledAmount - claimStatusTransaction.TotalAllowedAmount <> 0))  -- Paid Types or contractual Status
                    THEN (claimStatusBatchClaim.BilledAmount - claimStatusTransaction.TotalAllowedAmount)
				ELSE
					CASE
						WHEN (claimStatusTransaction.ClaimLineItemStatusId = 22)  -- Paid Types or contractual Status
						THEN (claimStatusBatchClaim.BilledAmount) 
					END
                END
                ) AS 'ContractualTotals'
		,COUNT(DISTINCT(
			CASE 
				WHEN  ((claimLineItemStatus.ClaimStatusTypeId = 1 AND (claimStatusBatchClaim.BilledAmount - claimStatusTransaction.TotalAllowedAmount <> 0)) 
						OR claimStatusTransaction.ClaimLineItemStatusId = 22)  -- Paid Types or contractual Status
				THEN claimStatusBatchClaim.ClaimLevelMd5Hash
			END
			)) AS 'ContractualVisits'
	,COUNT( DISTINCT(
				CASE 
					WHEN (claimLineItemStatus.ClaimStatusTypeId = 2 AND (claimStatusTransaction.WriteoffAmount = 0 OR claimStatusTransaction.WriteoffAmount is NULL))  -- Denial Types
					THEN claimStatusBatchClaim.ClaimLevelMd5Hash
				END
				)) AS 'DenialVisits'
	,SUM(
				CASE 
					WHEN (claimLineItemStatus.ClaimStatusTypeId = 2 AND (claimStatusTransaction.WriteoffAmount = 0 OR claimStatusTransaction.WriteoffAmount is NULL))  -- Denial Types
					THEN claimStatusBatchClaim.BilledAmount 
				END
				) AS 'DenialTotals'
	,COUNT( DISTINCT(
				CASE 
					WHEN ((claimLineItemStatus.ClaimStatusTypeId = 2 AND claimStatusTransaction.WriteoffAmount > 0) OR claimStatusTransaction.ClaimLineItemStatusId = 20)  -- Denial Types or WriteOff Status
					THEN claimStatusBatchClaim.ClaimLevelMd5Hash
				END
				)) AS 'WriteOffVisits'
	,SUM(
				CASE 
					WHEN ((claimLineItemStatus.ClaimStatusTypeId = 2 AND claimStatusTransaction.WriteoffAmount > 0) OR claimStatusTransaction.ClaimLineItemStatusId = 20)  -- Denial Types or WriteOff Status
					THEN claimStatusBatchClaim.BilledAmount 
				END
				) AS 'WriteOffTotals'
	,SUM(claimStatusTransaction.TotalAllowedAmount)- SUM(claimStatusTransaction.LineItemPaidAmount) AS 'Sec/Ps'
	,SUM(
				CASE 
					WHEN (claimLineItemStatus.ClaimStatusTypeId in (3,5)) -- Open Types or Status is null
					THEN claimStatusBatchClaim.BilledAmount 
				END
				) AS 'TotalOpen'
	,COUNT(DISTINCT(
				CASE 
					WHEN (claimLineItemStatus.ClaimStatusTypeId in (3,5)) -- Open Types or Status is null
					THEN claimStatusBatchClaim.ClaimLevelMd5Hash
				END
				)) AS 'OpenVisits'
	,SUM(
				CASE 
					WHEN (claimLineItemStatus.ClaimStatusTypeId is null OR claimStatusBatchClaim.ClaimStatusTransactionId is NULL) -- Errored Status or No Transaction
					THEN claimStatusBatchClaim.BilledAmount 
				END
				) AS 'TotalInProcess'
	,COUNT(DISTINCT(
				CASE 
					WHEN (claimLineItemStatus.ClaimStatusTypeId is null OR claimStatusBatchClaim.ClaimStatusTransactionId is NULL) -- Errored Status or No Transaction
					THEN claimStatusBatchClaim.ClaimLevelMd5Hash
				END
				)) AS 'InProcessVisits'
	,SUM(
				CASE 
					WHEN (claimLineItemStatus.ClaimStatusTypeId is not null) -- claim has a status that is not an error status
					THEN claimStatusBatchClaim.BilledAmount 
				END
				) AS 'TotalProcessed'
FROM IntegratedServices.ClaimStatusBatchClaims as claimStatusBatchClaim 
	LEFT JOIN IntegratedServices.ClaimStatusTransactions as claimStatusTransaction on claimStatusBatchClaim.Id = claimStatusTransaction.ClaimStatusBatchClaimId
	LEFT JOIN IntegratedServices.ClaimLineItemStatuses claimLineItemStatus on claimLineItemStatus.Id = claimStatusTransaction.ClaimLineItemStatusId
	JOIN [IntegratedServices].ClaimStatusBatches as claimStatusBatch ON claimStatusBatchClaim.ClaimStatusBatchId = claimStatusBatch.Id

WHERE claimStatusBatchClaim.ClientId = @ClientId
		
		---Multi Select Filter----
		AND (claimStatusTransaction.ClaimLineItemStatusId IN (SELECT convert(int, value)
			FROM string_split(@DelimitedLineItemStatusIds, ',')) OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = ''))
		And (claimStatusBatch.ClientInsuranceId in (SELECT convert(int, value)
			FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
		And ((@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = '') OR (claimStatusTransaction.ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value)
			FROM string_split(@ClientExceptionReasonCategoryIds, ','))))
		And (claimStatusBatchClaim.ClientLocationId in (SELECT convert(int, value)
			FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
		And (claimStatusBatchClaim.ClientProviderId in (SELECT convert(int, value)
			FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))
		And (claimStatusBatch.AuthTypeId in (SELECT convert(int, value)
			FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
		And (claimStatusBatchClaim.ProcedureCode in (SELECT convert(nvarchar(16), value)
			FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))

		AND ((claimStatusBatchClaim.CreatedOn >= @ReceivedFrom) OR @ReceivedFrom IS NULL)
		AND ((claimStatusBatchClaim.CreatedOn <= @ReceivedTo) OR @ReceivedTo IS NULL)
		AND ((claimStatusBatchClaim.DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom IS NULL)
		AND ((claimStatusBatchClaim.DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo IS NULL)
		AND ((claimStatusBatchClaim.ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
		AND ((claimStatusBatchClaim.ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)
		AND ((COALESCE(claimStatusTransaction.LastModifiedOn, claimStatusTransaction.CreatedOn) >= @TransactionDateFrom) OR @TransactionDateFrom is NULL )
		AND ((COALESCE(claimStatusTransaction.LastModifiedOn, claimStatusTransaction.CreatedOn) <= @TransactionDateTo) OR @TransactionDateTo is NULL)

		AND claimStatusBatchClaim.IsDeleted = 0
		AND claimStatusBatchClaim.IsSupplanted = 0
		AND ((claimStatusBatchClaim.PatientId = @PatientId) OR (@PatientId IS NULL))
	    AND ((claimStatusBatchClaim.ClaimStatusBatchId = @ClaimStatusBatchId) OR (@ClaimStatusBatchId IS NULL)  OR (@ClaimStatusBatchId = 0))	
	END