SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER   PROCEDURE [IntegratedServices].[spGetInitialClaimsSummaryQuery]
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

AS
BEGIN
SELECT 
c3.ClaimLevelMd5Hash
,c3.BilledOnDate
,c3.DateOfService
,SUM(c3.ChargedTotals)
,COUNT(c3.ClaimLevelMd5Hash) as 'ChargedVisits'
,SUM(
    CASE 
        WHEN ClaimLineItemStatusId in (1, 2, 15)
        THEN LineItemPaidAmount
    END
    ) AS 'PaymentTotals'
,COUNT(
    CASE 
        WHEN ClaimLineItemStatusId in (1, 2, 15)
        THEN ClaimLevelMd5Hash
    END
    ) AS 'PaymentVisits'
,SUM(
    CASE 
        WHEN ClaimLineItemStatusId in (7, 12, 23, 9, 3, 13, 18)
        THEN ChargedTotals
    END
    ) AS 'DenialTotals'
,COUNT(
    CASE 
        WHEN ClaimLineItemStatusId in (7, 12, 23, 9, 3, 13, 18)
        THEN ClaimLevelMd5Hash 
    END
    ) AS 'DenialVisits'
,SUM(
    CASE 
        WHEN (ClaimLineItemStatusId in (null, 10, 17) OR ClaimStatusTransactionId is null)
        THEN ChargedTotals
    END
    ) AS 'InProcessTotals'
,COUNT( 
    CASE 
        WHEN (ClaimLineItemStatusId in (null, 10, 17) OR ClaimStatusTransactionId is null)
        THEN ClaimLevelMd5Hash 
    END
    ) AS 'InProcessVisits'
,SUM(
    CASE 
        WHEN ClaimLineItemStatusId in (null, 10, 17)
        THEN ChargedTotals
    END
    ) AS 'NotAdjudicatedTotals'
,COUNT(
    CASE 
        WHEN ClaimLineItemStatusId in (null, 10, 17)
        THEN ClaimLevelMd5Hash 
    END
    ) AS 'NotAdjudicatedVisits'
FROM
(
    SELECT 
        c1.ClaimLevelMd5Hash as 'ClaimLevelMd5Hash'
        ,CONVERT(DATE, c1.ClaimBilledOn) as 'BilledOnDate'
        ,CONVERT(DATE, c1.DateOfServiceFrom) as 'DateOfService'
        ,c1.BilledAmount as 'ChargedTotals'
        ,t.ClaimLineItemStatusId
        ,t.LineItemPaidAmount
        ,c1.ClaimStatusTransactionId

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
    LEFT JOIN [IntegratedServices].ClaimStatusTransactions as t ON t.ClaimStatusBatchClaimId = c1.Id
    JOIN [IntegratedServices].ClaimStatusBatches as b ON c1.ClaimStatusBatchId = b.Id
    JOIN [dbo].ClientInsurances as i ON b.ClientInsuranceId = i.Id
    LEFT JOIN [IntegratedServices].ClaimLineItemStatuses as s ON t.ClaimLineItemStatusId = s.Id
    LEFT JOIN [dbo].Providers as Prv ON c1.ClientProviderId = Prv.Id
    LEFT JOIN [dbo].Persons as Per On Prv.PersonId = Per.Id
    LEFT JOIN [dbo].ClientLocations as ClientLoc On c1.ClientLocationId = ClientLoc.Id
    LEFT JOIN [IntegratedServices].ClaimStatusExceptionReasonCategories as er ON t.ClaimStatusExceptionReasonCategoryId = er.Id

WHERE C1.ClientId = @ClientId
    AND (t.ClaimLineItemStatusId IN (SELECT convert(int, value)
            FROM string_split(@DelimitedLineItemStatusIds, ',')) OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = ''))
        ---Multi Select Filter----
        And (b.ClientInsuranceId in (SELECT convert(int, value)
            FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
        And ((@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = '') OR (t.ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value)
            FROM string_split(@ClientExceptionReasonCategoryIds, ','))))
        And (ClientLoc.Id in (SELECT convert(int, value)
            FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
        And (Prv.Id in (SELECT convert(int, value)
            FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))
        And (b.AuthTypeId in (SELECT convert(int, value)
            FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
        And (c1.ProcedureCode in (SELECT convert(nvarchar(16), value)
            FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))
        AND ((c1.CreatedOn >= @ReceivedFrom) OR @ReceivedFrom IS NULL)
        AND ((c1.CreatedOn <= @ReceivedTo) OR @ReceivedTo IS NULL)
        AND ((c1.DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom IS NULL)
        AND ((c1.DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo IS NULL)
        AND ((c1.ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
        AND ((c1.ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)
        AND ((COALESCE(t.LastModifiedOn, t.CreatedOn) >= @TransactionDateFrom) OR @TransactionDateFrom is NULL )
        AND ((COALESCE(t.LastModifiedOn, t.CreatedOn) <= @TransactionDateTo) OR @TransactionDateTo is NULL)
        AND c1.IsDeleted = 0
        AND c1.IsSupplanted = 0
        AND ((c1.PatientId = @PatientId) OR (@PatientId IS NULL))
        AND ((c1.ClaimStatusBatchId = @ClaimStatusBatchId) OR (@ClaimStatusBatchId IS NULL)  OR (@ClaimStatusBatchId = 0))  
        --GROUP BY c1.ClaimBilledOn, c1.DateOfServiceFrom
        ) as c3
        GROUP BY CONVERT(DATE, BilledOnDate), CONVERT(DATE, DateOfService), ClaimLevelMd5Hash
    END
GO