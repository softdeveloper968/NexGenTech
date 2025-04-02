SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER     PROCEDURE [IntegratedServices].[spGetChargesMonthly]
    @ClientId INT
    ,@DelimitedLineItemStatusIds NVARCHAR(MAX) = NULL
    ,@ReceivedFrom DATETIME = NULL
    ,@ReceivedTo DATETIME = NULL
    ,@DateOfServiceFrom DATETIME = NULL
    ,@DateOfServiceTo DATETIME = NULL
    ,@TransactionDateFrom DATETIME = NULL
    ,@TransactionDateTo DATETIME = NULL
    ,@ClaimBilledFrom DATETIME = NULL
    ,@ClaimBilledTo DATETIME = NULL
    ,@ClientProviderIds NVARCHAR(MAX) = NULL
    ,@ClientLocationIds NVARCHAR(MAX) = NULL
    ,@ClientInsuranceIds NVARCHAR(MAX) = NULL
    ,@ClientExceptionReasonCategoryIds NVARCHAR(MAX) = NULL
    ,@ClientAuthTypeIds NVARCHAR(MAX) = NULL
    ,@ClientProcedureCodes NVARCHAR(MAX) = NULL
    ,@PatientId INT = NULL
    ,@ClaimStatusBatchId INT = NULL
AS
BEGIN
    SELECT 
        i.LookupName AS 'PayerName'
        ,i.Id AS 'PayerId'
        ,'DateOfService' = CONVERT(DATE, c1.DateOfServiceFrom)
        ,'BilledOnDate' = CONVERT(DATE, c1.ClaimBilledOn)
        ,SUM(c1.BilledAmount) AS 'ChargedSum'
    FROM [IntegratedServices].ClaimStatusBatchClaims AS c1
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
        LEFT JOIN [IntegratedServices].ClaimStatusTransactions AS t ON t.ClaimStatusBatchClaimId = c1.Id
        JOIN [IntegratedServices].ClaimStatusBatches AS b ON c1.ClaimStatusBatchId = b.Id
        JOIN [dbo].ClientInsurances AS i ON b.ClientInsuranceId = i.Id
        LEFT JOIN [IntegratedServices].ClaimLineItemStatuses AS s ON t.ClaimLineItemStatusId = s.Id
        LEFT JOIN [dbo].Providers AS Prv ON c1.ClientProviderId = Prv.Id
        LEFT JOIN [dbo].Persons AS Per ON Prv.PersonId = Per.Id
        LEFT JOIN [dbo].ClientLocations AS ClientLoc ON c1.ClientLocationId = ClientLoc.Id
        LEFT JOIN [IntegratedServices].ClaimStatusExceptionReasonCategories AS er ON t.ClaimStatusExceptionReasonCategoryId = er.Id
    WHERE C1.ClientId = @ClientId
        AND (t.ClaimLineItemStatusId IN (SELECT CONVERT(INT, value)
                                        FROM STRING_SPLIT(@DelimitedLineItemStatusIds, ',')) OR (@DelimitedLineItemStatusIds IS NULL OR @DelimitedLineItemStatusIds = ''))
        ---Multi Select Filter----
        AND (b.ClientInsuranceId IN (SELECT CONVERT(INT, value)
                                      FROM STRING_SPLIT(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds IS NULL OR @ClientInsuranceIds = ''))
        AND ((@ClientExceptionReasonCategoryIds IS NULL OR @ClientExceptionReasonCategoryIds = '') OR (t.ClaimStatusExceptionReasonCategoryId IN (SELECT CONVERT(INT, value)
                                                                                                                                                FROM STRING_SPLIT(@ClientExceptionReasonCategoryIds, ','))))
        AND (ClientLoc.Id IN (SELECT CONVERT(INT, value)
                              FROM STRING_SPLIT(@ClientLocationIds, ',')) OR (@ClientLocationIds IS NULL OR @ClientLocationIds = ''))
        AND (Prv.Id IN (SELECT CONVERT(INT, value)
                        FROM STRING_SPLIT(@ClientProviderIds, ',')) OR (@ClientProviderIds IS NULL OR @ClientProviderIds = ''))
        AND (b.AuthTypeId IN (SELECT CONVERT(INT, value)
                              FROM STRING_SPLIT(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds IS NULL OR @ClientAuthTypeIds = ''))
        AND (c1.ProcedureCode IN (SELECT CONVERT(NVARCHAR(16), value)
                                  FROM STRING_SPLIT(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes IS NULL OR @ClientProcedureCodes = ''))
        --AND ((c1.CreatedOn >= @ReceivedFrom) OR @ReceivedFrom IS NULL)
        --AND ((c1.CreatedOn <= @ReceivedTo) OR @ReceivedTo IS NULL)
        --AND ((c1.DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom IS NULL)
        --AND ((c1.DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo IS NULL)
        --AND ((c1.ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
        --AND ((c1.ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo IS NULL)
        --AND ((COALESCE(t.LastModifiedOn, t.CreatedOn) >= @TransactionDateFrom) OR @TransactionDateFrom IS NULL )
        --AND ((COALESCE(t.LastModifiedOn, t.CreatedOn) <= @TransactionDateTo) OR @TransactionDateTo IS NULL)
        AND c1.IsDeleted = 0
        AND c1.IsSupplanted = 0
        AND ((c1.PatientId = @PatientId) OR (@PatientId IS NULL))
        AND ((c1.ClaimStatusBatchId = @ClaimStatusBatchId) OR (@ClaimStatusBatchId IS NULL) OR (@ClaimStatusBatchId = 0))    
    GROUP BY c1.ClaimLevelMD5Hash, CONVERT(DATE, c1.DateOfServiceFrom), CONVERT(DATE, ClaimBilledOn), i.LookupName, i.Id
END
GO


