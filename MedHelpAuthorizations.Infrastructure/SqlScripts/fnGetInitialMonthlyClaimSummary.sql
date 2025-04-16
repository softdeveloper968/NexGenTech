SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   FUNCTION [IntegratedServices].[fnGetInitialMonthlyClaimSummary](
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
	,@ClaimStatusBatchId int = NULL)

RETURNS TABLE
AS
RETURN
	SELECT 
	SUM(c1.BilledAmount) AS 'ChargedTotals'
	,COUNT(DISTINCT(c1.ClaimLevelMd5Hash)) AS 'TotalVisits'
	,SUM(
			CASE 
				WHEN cst.ClaimStatusTypeId = 1
				THEN t.LineItemPaidAmount
			END
			) AS 'PaymentTotals'
		
		,SUM(
			CASE 
				WHEN (cst.ClaimStatusTypeId = 2 AND (t.WriteoffAmount is NULL OR t.WriteoffAmount = 0))
				THEN c1.BilledAmount
			END
			) AS 'DenialTotals'
		
		,SUM(
			CASE 
				WHEN (cst.ClaimStatusTypeId is null OR c1.ClaimStatusTransactionId IS NULL)
				THEN c1.BilledAmount
			END
			) AS 'InProcessTotals'
		
		,SUM(
			CASE 
				WHEN cst.ClaimStatusTypeId in (3,5)
				THEN c1.BilledAmount
			END
			) AS 'NotAdjudicatedTotals'
		
FROM [IntegratedServices].ClaimStatusBatchClaims as c1
	JOIN(
		SELECT * FROM [IntegratedServices].[fnGetInitialClaimEntry](
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
    ) as c2 ON c1.Id = c2.MinId 
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
	) as c3 ON c1.ClaimLevelMd5Hash = c3.ClaimLevelMd5Hash
    LEFT JOIN [IntegratedServices].ClaimStatusTransactions as t ON t.ClaimStatusBatchClaimId = c1.Id
	LEFT JOIN IntegratedServices.ClaimLineItemStatuses cst on cst.Id = t.ClaimLineItemStatusId
    JOIN [IntegratedServices].ClaimStatusBatches as b ON c1.ClaimStatusBatchId = b.Id
    JOIN [dbo].ClientInsurances as i ON b.ClientInsuranceId = i.Id

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
	    AND ((c1.ClaimStatusBatchId = @ClaimStatusBatchId) OR (@ClaimStatusBatchId IS NULL)  OR (@ClaimStatusBatchId = 0));