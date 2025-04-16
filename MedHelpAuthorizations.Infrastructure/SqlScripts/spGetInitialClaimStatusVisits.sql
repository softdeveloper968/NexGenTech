/****** Object:  StoredProcedure [IntegratedServices].[spGetClaimStatusVisits]    Script Date: 4/28/2024 7:47:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [IntegratedServices].[spGetInitialClaimStatusVisits]
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
	COUNT(DISTINCT(c1.ClaimLevelMd5Hash)) as 'Quantity'
	, SUM(c1.BilledAmount) as 'BilledAmt'
	, SUM(t.TotalAllowedAmount) as 'AllowedAmountSum'
	, SUM(t.LineItemPaidAmount) as 'PaidAmountSum'
	,COUNT( DISTINCT(
					CASE 
					WHEN (cst.ClaimStatusTypeId in (1,2,4))  -- Paid, Denied , OtherAdjudicated types
						THEN c1.ClaimLevelMd5Hash
					END
					)) AS 'AdjudicatedVisits'
		,SUM(
					CASE 
					WHEN (cst.ClaimStatusTypeId in (1,2,4)) -- Paid, Denied , OtherAdjudicated types
						THEN c1.BilledAmount
					END
					) AS 'AdjudicatedTotals'
		,COUNT( DISTINCT(
					CASE 
						WHEN (cst.ClaimStatusTypeId = 1) -- Paid Types
						THEN c1.ClaimLevelMd5Hash
					END
					)) AS 'AllowedVisits'
		,SUM(
					CASE 
						WHEN (cst.ClaimStatusTypeId = 1)  -- Paid Types
						THEN t.TotalAllowedAmount
					END
					) AS 'AllowedTotals'
		,COUNT( DISTINCT(
					CASE 
						WHEN (cst.ClaimStatusTypeId = 1)  -- Paid Types
						THEN c1.ClaimLevelMd5Hash
					END
					)) AS 'PaidVisits'
		,SUM(
					CASE 
						WHEN (cst.ClaimStatusTypeId = 1)  -- Paid Types
						THEN t.LineItemPaidAmount 
					END
					) AS 'PaidTotals'
		,SUM(
				CASE 
					WHEN  (cst.ClaimStatusTypeId = 1)  -- Paid Types or contractual Status
					THEN (c1.BilledAmount - t.TotalAllowedAmount)
				END
				) + SUM(CASE 
					WHEN  (t.ClaimLineItemStatusId = 22)  -- Paid Types or contractual Status
					THEN (c1.BilledAmount)
				END)  AS 'ContractualTotals'
	,COUNT(DISTINCT(
				CASE 
					WHEN  (cst.ClaimStatusTypeId = 1 OR t.ClaimLineItemStatusId = 22)  -- Paid Types or contractual Status
					THEN c1.ClaimLevelMd5Hash
				END
				)) AS 'ContractualVisits'
		,COUNT( DISTINCT(
					CASE 
						WHEN  (cst.ClaimStatusTypeId = 2)  -- Denial Types
						THEN c1.ClaimLevelMd5Hash
					END
					)) AS 'DenialVisits'
		,SUM(
					CASE 
						WHEN (cst.ClaimStatusTypeId = 2)  -- Denial Types
						THEN c1.BilledAmount 
					END
					) AS 'DenialTotals'
		,COUNT( DISTINCT(
					CASE 
						WHEN ((cst.ClaimStatusTypeId = 2 AND t.WriteoffAmount > 0) OR t.ClaimLineItemStatusId = 20)  -- Denial Types or WriteOff Status
						THEN c1.ClaimLevelMd5Hash
					END
					)) AS 'WriteOffVisits'
		,SUM(
					CASE 
						WHEN ((cst.ClaimStatusTypeId = 2 AND t.WriteoffAmount > 0) OR t.ClaimLineItemStatusId = 20)  -- Denial Types or WriteOff Status
						THEN c1.BilledAmount 
					END
					) AS 'WriteOffTotals'
		,SUM(t.TotalAllowedAmount)- SUM(t.LineItemPaidAmount) AS 'Sec/Ps'
		,SUM(
					CASE 
						WHEN (cst.ClaimStatusTypeId in (3,5)) -- Open Types or Status is null
						THEN c1.BilledAmount 
					END
					) AS 'TotalOpen'
		,COUNT(DISTINCT(
					CASE 
						WHEN (cst.ClaimStatusTypeId in (3,5)) -- Open Types or Status is null
						THEN c1.ClaimLevelMd5Hash
					END
					)) AS 'OpenVisits'
		,SUM(
					CASE 
						WHEN (cst.ClaimStatusTypeId is null OR c1.ClaimStatusTransactionId is NULL) -- Errored Status or No Transaction
						THEN c1.BilledAmount 
					END
					) AS 'TotalInProcess'
		,COUNT(DISTINCT(
					CASE 
						WHEN (cst.ClaimStatusTypeId is null OR c1.ClaimStatusTransactionId is NULL) -- Errored Status or No Transaction
						THEN c1.ClaimLevelMd5Hash
					END
					)) AS 'InProcessVisits'
		,SUM(
					CASE 
						WHEN (cst.ClaimStatusTypeId is not null) -- claim has a status that is not an error status
						THEN c1.BilledAmount 
					END
					) AS 'TotalProcessed'
FROM IntegratedServices.ClaimStatusBatchClaims as c1 
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
	) as c2 ON c1.ClaimLevelMd5Hash = c2.ClaimLevelMd5Hash -- AND c1.ClaimBilledOn = c2.LatestClaimBilledOn  
	LEFT JOIN IntegratedServices.ClaimStatusTransactions as t on c1.Id = t.ClaimStatusBatchClaimId
	LEFT JOIN IntegratedServices.ClaimLineItemStatuses cst on cst.Id = t.ClaimLineItemStatusId
	JOIN [IntegratedServices].ClaimStatusBatches as b ON c1.ClaimStatusBatchId = b.Id
	WHERE C1.ClientId = @ClientId
	AND (t.ClaimLineItemStatusId IN (SELECT convert(int, value)
			FROM string_split(@DelimitedLineItemStatusIds, ',')) OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = ''))
		---Multi Select Filter----
		And (b.ClientInsuranceId in (SELECT convert(int, value)
			FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
		And ((@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = '') OR (t.ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value)
			FROM string_split(@ClientExceptionReasonCategoryIds, ','))))
		And (c1.ClientLocationId in (SELECT convert(int, value)
			FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
		And (c1.ClientProviderId in (SELECT convert(int, value)
			FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))
		And (b.AuthTypeId in (SELECT convert(int, value)
			FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
		And (c1.ProcedureCode in (SELECT convert(nvarchar(16), value)
			FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))
	--	AND ((c1.CreatedOn >= @ReceivedFrom) OR @ReceivedFrom IS NULL)
	--	AND ((c1.CreatedOn <= @ReceivedTo) OR @ReceivedTo IS NULL)
	--	AND ((c1.DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom IS NULL)
	--	AND ((c1.DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo IS NULL)
	--	AND ((c1.ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
	--	AND ((c1.ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)
	--	AND ((COALESCE(t.LastModifiedOn, t.CreatedOn) >= @TransactionDateFrom) OR @TransactionDateFrom is NULL )
	--	AND ((COALESCE(t.LastModifiedOn, t.CreatedOn) <= @TransactionDateTo) OR @TransactionDateTo is NULL)
		AND c1.IsDeleted = 0
		AND c1.IsSupplanted = 0
		AND ((c1.PatientId = @PatientId) OR (@PatientId IS NULL))
	    AND ((c1.ClaimStatusBatchId = @ClaimStatusBatchId) OR (@ClaimStatusBatchId IS NULL)  OR (@ClaimStatusBatchId = 0))
	END
