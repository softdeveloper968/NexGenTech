
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


SET @ClientId = 3
--SET @DelimitedLineItemStatusIds = '1,2'
--SET @ClaimStatusType = 2
SET @ClaimBilledFrom = '2024-02-08'
SET @ClaimBilledTo = '2024-03-08'


BEGIN
SELECT   
		COUNT( DISTINCT(
			CASE 
				WHEN (claimLineItemStatus.ClaimStatusTypeId = 1)
				THEN c1.ClaimLevelMd5Hash
			END
			)) AS 'PaidVisits'

		,SUM(
			CASE 
				WHEN (claimLineItemStatus.ClaimStatusTypeId = 1)
				THEN c1.BilledAmount
			END
			) AS 'PaidTotals'

		,SUM(
            CASE 
                WHEN (claimLineItemStatus.ClaimStatusTypeId = 2 AND (t.WriteoffAmount IS NULL OR t.WriteoffAmount = 0)) THEN c1.BilledAmount
            END
        ) AS 'DenialTotals'

		,COUNT(DISTINCT(
            CASE 
                WHEN (claimLineItemStatus.ClaimStatusTypeId = 2 AND (t.WriteoffAmount IS NULL OR t.WriteoffAmount = 0)) THEN c1.ClaimLevelMd5Hash 
            END
        )) AS 'DenialVisits'

		,SUM(
            CASE 
                WHEN ((claimLineItemStatus.ClaimStatusTypeId = 2 AND (t.WriteoffAmount IS NOT NULL AND t.WriteoffAmount > 0)) OR (t.ClaimLineItemStatusId = 20 )) 
				THEN c1.BilledAmount
            END
        ) AS 'WriteOffTotals'
		
		,COUNT(DISTINCT(
            CASE 
                 WHEN ((claimLineItemStatus.ClaimStatusTypeId = 2 AND (t.WriteoffAmount IS NOT NULL AND t.WriteoffAmount > 0)) OR (t.ClaimLineItemStatusId = 20 )) 
				 THEN c1.ClaimLevelMd5Hash 
            END
        )) AS 'WriteOffVisits'

		,SUM(
			CASE 
				WHEN claimLineItemStatus.ClaimStatusTypeId in (3,5)
				THEN c1.BilledAmount
			END
		) AS 'NotAdjudicatedTotals'
		,COUNT(DISTINCT(
			CASE 
				WHEN claimLineItemStatus.ClaimStatusTypeId in (3,5)
				THEN c1.ClaimLevelMd5Hash 
			END
		)) AS 'NotAdjudicatedVisits'

		,SUM(
            CASE 
                   WHEN (t.ClaimLineItemStatusId = 22 ) THEN c1.BilledAmount
            END
        ) AS 'ContractualTotals'
		
		,COUNT(DISTINCT(
            CASE 
                    WHEN (t.ClaimLineItemStatusId = 22 ) THEN c1.ClaimLevelMd5Hash
            END
        )) AS 'ContractualVisits'

		,SUM(
            CASE 
                WHEN (claimLineItemStatus.ClaimStatusTypeId is NULL OR t.ClaimLineItemStatusId is NULL) THEN c1.BilledAmount
            END
        ) AS 'InProcessTotals'

		,COUNT(DISTINCT(
            CASE 
                WHEN (claimLineItemStatus.ClaimStatusTypeId is NULL OR t.ClaimLineItemStatusId is NULL) THEN c1.ClaimLevelMd5Hash 
            END
        )) AS 'InProcessVisits'
	
	
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
	LEFT JOIN IntegratedServices.ClaimLineItemStatuses claimLineItemStatus on claimLineItemStatus.Id = t.ClaimLineItemStatusId
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