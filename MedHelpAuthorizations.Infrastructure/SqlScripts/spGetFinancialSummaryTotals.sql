SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [IntegratedServices].[spGetFinancialSummaryTotals]
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
		SUM(c1.BilledAmount) as 'ChargedTotals'
		,COUNT(DISTINCT(c1.ClaimLevelMd5Hash)) as 'Quantity'
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
                    WHEN  (cst.ClaimStatusTypeId = 1 AND (c1.BilledAmount - t.TotalAllowedAmount <> 0))  -- Paid Types or contractual Status
                    THEN (c1.BilledAmount - t.TotalAllowedAmount)
				ELSE
					CASE
						WHEN (t.ClaimLineItemStatusId = 22)  -- Paid Types or contractual Status
						THEN (c1.BilledAmount) 
					END
                END
                ) AS 'ContractualTotals'
		,COUNT(DISTINCT(
			CASE 
				WHEN  ((cst.ClaimStatusTypeId = 1 AND (c1.BilledAmount - t.TotalAllowedAmount <> 0)) OR t.ClaimLineItemStatusId = 22)  -- Paid Types or contractual Status
				THEN c1.ClaimLevelMd5Hash
			END
			)) AS 'ContractualVisits'
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
		,SUM(
			CASE
				WHEN (((c1.ClaimStatusTransactionId is NULL OR cst.ClaimStatusTypeId = 1) OR (cst.ClaimStatusTypeId = 1 AND t.TotalAllowedAmount > t.LineItemPaidAmount)) AND (c1.ClaimBilledOn < DATEADD(mm, DATEDIFF(m,0,GETDATE()),0)))
				THEN c1.BilledAmount
			END
			) AS 'ArBeginning'
		,SUM(
			CASE
				WHEN (((c1.ClaimStatusTransactionId is NULL OR cst.ClaimStatusTypeId = 1) OR (cst.ClaimStatusTypeId = 1 AND t.TotalAllowedAmount > t.LineItemPaidAmount)) AND (c1.ClaimBilledOn < DATEADD(mm, DATEDIFF(m,0,GETDATE())+1,0)))
				THEN c1.BilledAmount
			END
			) AS 'ArEnding'
		,COUNT(DISTINCT(
			CASE
				WHEN (((c1.ClaimStatusTransactionId is NULL OR cst.ClaimStatusTypeId = 1) OR (cst.ClaimStatusTypeId = 1 AND t.TotalAllowedAmount > t.LineItemPaidAmount)) AND (c1.ClaimBilledOn < DATEADD(mm, DATEDIFF(m,0,GETDATE()),0)))
				THEN c1.ClaimLevelMd5Hash
			END
			)) AS 'ArBeginningVisits'
		,COUNT(DISTINCT(
			CASE
				WHEN (((c1.ClaimStatusTransactionId is NULL OR cst.ClaimStatusTypeId = 1) OR (cst.ClaimStatusTypeId = 1 AND t.TotalAllowedAmount > t.LineItemPaidAmount)) AND (c1.ClaimBilledOn < DATEADD(mm, DATEDIFF(m,0,GETDATE())+1,0)))
				THEN c1.ClaimLevelMd5Hash
			END
			)) AS 'ArEndingVisits'

FROM [IntegratedServices].ClaimStatusBatchClaims as c1
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
	) as c2 ON c1.ClaimLevelMd5Hash = c2.ClaimLevelMd5Hash
	LEFT JOIN [IntegratedServices].ClaimStatusTransactions as t ON t.ClaimStatusBatchClaimId = c1.Id
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
		--AND ((c1.CreatedOn >= @ReceivedFrom) OR @ReceivedFrom IS NULL)
		--AND ((c1.CreatedOn <= @ReceivedTo) OR @ReceivedTo IS NULL)
		--AND ((c1.DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom IS NULL)
		--AND ((c1.DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo IS NULL)
		--AND ((c1.ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
		--AND ((c1.ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)
		--AND ((COALESCE(t.LastModifiedOn, t.CreatedOn) >= @TransactionDateFrom) OR @TransactionDateFrom is NULL )
		--AND ((COALESCE(t.LastModifiedOn, t.CreatedOn) <= @TransactionDateTo) OR @TransactionDateTo is NULL)
		AND c1.IsDeleted = 0
		AND c1.IsSupplanted = 0
		AND ((c1.PatientId = @PatientId) OR (@PatientId IS NULL))
	    AND ((c1.ClaimStatusBatchId = @ClaimStatusBatchId) OR (@ClaimStatusBatchId IS NULL)  OR (@ClaimStatusBatchId = 0))	
	END