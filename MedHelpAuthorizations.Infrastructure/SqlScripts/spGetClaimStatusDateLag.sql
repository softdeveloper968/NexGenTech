SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- AA-255 : Dashboard Redesign

CREATE OR ALTER PROCEDURE [IntegratedServices].[spGetClaimStatusDateLag]
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
    ,@flag NVARCHAR = 'submission' --payment
AS
BEGIN
    SELECT
		 AVG(ABS(DATEDIFF(DAY, DateOfServiceFrom, ClaimBilledOn))) AS ServiceToBilled
        ,AVG(ABS(DATEDIFF(DAY, DateOfServiceFrom, CheckDate))) AS ServiceToPayment
        ,AVG(ABS(DATEDIFF(DAY, CheckDate, ClaimBilledOn))) AS BilledToPayment
	      --'ServiceToBilled' = ABS(DATEDIFF(DAY, DateOfServiceFrom, ClaimBilledOn))
       -- , 'ServiceToPayment' = ABS(DATEDIFF(DAY, DateOfServiceFrom, CheckDate))
       -- , 'BilledToPayment' = ABS(DATEDIFF(DAY, CheckDate, ClaimBilledOn))

    FROM 
    IntegratedServices.ClaimStatusBatchClaims AS c
	JOIN(
		SELECT LatestClaimBilledOn, ClaimLevelMd5Hash FROM [IntegratedServices].[fnGetClaimLevelGroups](
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
	) as c2 ON c.ClaimBilledOn = c2.LatestClaimBilledOn AND c.ClaimLevelMd5Hash = c2.ClaimLevelMd5Hash
	JOIN [IntegratedServices].ClaimStatusBatches as b ON c.ClaimStatusBatchId = b.Id
    LEFT JOIN IntegratedServices.ClaimStatusTransactions AS t ON t.Id = c.ClaimStatusTransactionId

    -- WHERE c.ClaimNumber IS NOT NULL
    WHERE c.ClientId = @ClientId
        -- AND CheckDate IS NOT NULL
		AND (t.ClaimLineItemStatusId = 1)
			--FROM string_split(@DelimitedLineItemStatusIds, ',')) OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = ''))
		---Multi Select Filter----
		And (c.ClientInsuranceId in (SELECT convert(int, value)
			FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
		And (t.ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value)
			FROM string_split(@ClientExceptionReasonCategoryIds, ',')) OR (@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = ''))
		And (c.ClientLocationId in (SELECT convert(int, value)
			FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
		And (c.ClientProviderId in (SELECT convert(int, value)
			FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))
		And (b.AuthTypeId in (SELECT convert(int, value)
			FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
		And (c.ProcedureCode in (SELECT convert(nvarchar(16), value)
			FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))
		AND ((c.CreatedOn >= @ReceivedFrom) OR @ReceivedFrom IS NULL)
		AND ((c.CreatedOn <= @ReceivedTo) OR @ReceivedTo IS NULL)
		AND ((c.DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom IS NULL)
		AND ((c.DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo IS NULL)
		AND ((c.ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
		AND ((c.ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)
		AND ((COALESCE(t.LastModifiedOn, t.CreatedOn) >= @TransactionDateFrom) OR @TransactionDateFrom is NULL )
		AND ((COALESCE(t.LastModifiedOn, t.CreatedOn) <= @TransactionDateTo) OR @TransactionDateTo is NULL)
		AND c.IsDeleted = 0
		AND c.IsSupplanted = 0
		AND ((c.PatientId = @PatientId) OR (@PatientId IS NULL))
	    AND ((c.ClaimStatusBatchId = @ClaimStatusBatchId) OR (@ClaimStatusBatchId IS NULL)  OR (@ClaimStatusBatchId = 0))
		AND t.CheckDate IS NOT NULL 
    --ORDER BY t.ClaimNumber
END
GO