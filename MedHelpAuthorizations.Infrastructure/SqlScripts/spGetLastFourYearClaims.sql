SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [IntegratedServices].[spGetLastFourYearClaims]
AS
BEGIN
    SELECT 
        c1.ClientId as 'ClientId',
        cpt.Id as 'ClientCodeId',
        t.ClaimStatusExceptionReasonCategoryId as 'ClaimStatusExceptionReasonCategoryId',
        SUM(c1.BilledAmount) AS 'ChargedSum',
        c1.ClientLocationId AS 'ClientLocationId',
        c1.ClientProviderId AS 'ClientProviderId',
        c1.ClientInsuranceId AS 'ClientInsuranceId',
        SUM(t.TotalAllowedAmount) AS 'AllowedAmountSum',
        COUNT(c1.ClaimLevelMD5Hash) AS 'Quantity',
        s.Id AS 'ClaimLineItemStatusId',
        SUM(t.TotalNonAllowedAmount) AS 'NonAllowedAmountSum',
        SUM(t.WriteoffAmount) AS 'WriteOffAmountSum',
        SUM(t.LineItemPaidAmount) AS 'PaidAmountSum'
		,CONVERT(Date, c1.ClaimBilledOn) AS 'BilledOnDate'
		,CONVERT(Date, c1.DateOfServiceFrom) AS 'DateOfServiceFrom'
		,CONVERT(Date, c1.DateOfServiceTo) AS 'DateOfServiceTo'
	   ,COALESCE(CONVERT(date, t.LastModifiedOn), CONVERT(date, t.CreatedOn)) AS 'TransactionDate',
	   CONVERT(Date, c1.CreatedOn) AS 'AitClaimReceivedDate'
    FROM [IntegratedServices].ClaimStatusBatchClaims AS c1
	LEFT JOIN dbo.ClientCptCodes cpt ON cpt.Id = c1.ClientCptCodeId
    LEFT JOIN [IntegratedServices].ClaimStatusTransactions AS t ON t.ClaimStatusBatchClaimId = c1.Id
    LEFT JOIN [IntegratedServices].ClaimLineItemStatuses AS s ON t.ClaimLineItemStatusId = s.Id
	
   WHERE 
    ((c1.ClaimBilledOn >= DATEADD(YEAR, -4, GETDATE())) )
    AND ((c1.ClaimBilledOn <= GETDATE()))
    AND c1.IsDeleted = 0
    AND c1.IsSupplanted = 0
    GROUP BY  
    c1.ClientId,
    cpt.Id,
    t.ClaimStatusExceptionReasonCategoryId,
    c1.ClientLocationId,
    c1.ClientProviderId,
    s.Id,
    CONVERT(date, c1.ClaimBilledOn),
    CONVERT(date, c1.DateOfServiceFrom),
    CONVERT(date, c1.DateOfServiceTo),
    COALESCE(CONVERT(date, t.LastModifiedOn), CONVERT(date, t.CreatedOn)),
    CONVERT(date, c1.CreatedOn),
    c1.ClientInsuranceId
END
