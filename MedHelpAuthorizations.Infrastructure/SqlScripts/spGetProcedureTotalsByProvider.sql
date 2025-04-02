SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [IntegratedServices].[spGetProcedureTotalsByProvider]
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
        Prv.Id AS 'ClientProviderId',
        SUM(c1.BilledAmount) AS 'ChargedSum',
        Per.LastName + ',' + Per.FirstName AS 'ClientProviderName',
        COUNT(DISTINCT(c1.ClaimLevelMd5Hash)) AS 'Quantity'
    FROM [IntegratedServices].ClaimStatusBatchClaims AS c1
    JOIN (
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
    ) AS c2 ON c1.ClaimLevelMd5Hash = c2.ClaimLevelMd5Hash
    LEFT JOIN [IntegratedServices].ClaimStatusTransactions AS t ON t.ClaimStatusBatchClaimId = c1.Id
    JOIN [IntegratedServices].ClaimStatusBatches as b ON c1.ClaimStatusBatchId = b.Id
    LEFT JOIN [dbo].Providers AS Prv ON c1.ClientProviderId = Prv.Id
    LEFT JOIN [dbo].Persons AS Per ON Prv.PersonId = Per.Id
    WHERE C1.ClientId = @ClientId
        AND (t.ClaimLineItemStatusId IN (SELECT CONVERT(INT, value)
            FROM string_split(@DelimitedLineItemStatusIds, ',')) OR (@DelimitedLineItemStatusIds IS NULL OR @DelimitedLineItemStatusIds = ''))
       And (b.ClientInsuranceId in (SELECT convert(int, value)
			FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
        AND ((@ClientExceptionReasonCategoryIds IS NULL OR @ClientExceptionReasonCategoryIds = '') 
            OR (t.ClaimStatusExceptionReasonCategoryId IN (SELECT CONVERT(INT, value)
            FROM string_split(@ClientExceptionReasonCategoryIds, ','))))
        And (c1.ClientLocationId in (SELECT convert(int, value)
			FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
        AND (Prv.Id IN (SELECT CONVERT(INT, value)
            FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds IS NULL OR @ClientProviderIds = ''))
       And (b.AuthTypeId in (SELECT convert(int, value)
			FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
        AND (c1.ProcedureCode IN (SELECT CONVERT(NVARCHAR(16), value)
            FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes IS NULL OR @ClientProcedureCodes = ''))
        AND c1.IsDeleted = 0
        AND c1.IsSupplanted = 0
        AND ((c1.PatientId = @PatientId) OR (@PatientId IS NULL))
        AND ((c1.ClaimStatusBatchId = @ClaimStatusBatchId) OR (@ClaimStatusBatchId IS NULL)  OR (@ClaimStatusBatchId = 0))
    GROUP BY  Prv.Id, (Per.LastName + ',' + Per.FirstName)
END

