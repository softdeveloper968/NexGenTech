SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [IntegratedServices].[spGetProvidersDetails]
     @ClientId int 
    ,@ClientExceptionReasonCategoryIds nvarchar(MAX) = NULL 
	,@ClientInsuranceIds nvarchar(MAX) = NULL
	,@ClientAuthTypeIds nvarchar(MAX) = NULL 
	,@ClientProcedureCodes nvarchar(MAX) = NULL 
	,@ClientProviderIds nvarchar(MAX) = NULL 
    ,@ClientLocationIds nvarchar(MAX) = NULL 
    ,@DateOfServiceFrom DateTime = NULL 
	,@DateOfServiceTo DateTime = NULL 
	,@ClaimBilledFrom DateTime = NULL 
	,@ClaimBilledTo DateTime = NULL 
	,@EstablishedPtCodes NVARCHAR(MAX) = NULL
	,@NewPtCodes NVARCHAR(MAX) = NULL

AS 
BEGIN

   SELECT         
        csbc.ClientProviderId as 'ProviderId'
		,SUBSTRING(TRIM(ProcedureCode),1,5) as 'ProcedureCode'
        ,COUNT(DISTINCT(csbc.ClaimLevelMD5Hash)) as 'Quantity'
        ,SUM(csbc.BilledAmount) as 'ChargedSum'
        ,Per.LastName + ',' + per.FirstName as 'ProviderName'
		,COUNT( DISTINCT(
			CASE 
				WHEN (cs.ClaimStatusTypeId = 2)
				THEN csbc.ClaimLevelMd5Hash
			END
			)) AS 'DenialVisits'
		,SUM( 
			CASE 
				WHEN (cs.ClaimStatusTypeId = 2)
				THEN csbc.BilledAmount
				ELSE 0
			END
			) AS 'DenialTotals'

    FROM [IntegratedServices].[ClaimStatusBatchClaims] as csbc
    LEFT JOIN [IntegratedServices].ClaimStatusBatches as b  ON csbc.ClaimStatusBatchId = b.Id
    LEFT JOIN IntegratedServices.ClaimStatusTransactions as t on t.Id=csbc.ClaimStatusTransactionId
    LEFT JOIN IntegratedServices.ClaimLineItemStatuses as cs on cs.Id=t.ClaimLineItemStatusId
    LEFT JOIN [IntegratedServices].ClaimStatusExceptionReasonCategories as er ON t.ClaimStatusExceptionReasonCategoryId = er.Id
    LEFT JOIN [dbo].Providers as Prv ON csbc.ClientProviderId = Prv.Id
    LEFT JOIN [dbo].Persons as Per On Prv.PersonId = Per.Id

 WHERE csbc.ClientId = @ClientId
		AND (t.IsDeleted = 0 AND csbc.IsDeleted = 0)
        AND TRIM(ProcedureCode) LIKE '99%'
        And (b.ClientInsuranceId in (SELECT convert(int, value)
            FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
        And (t.ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value)
            FROM string_split(@ClientExceptionReasonCategoryIds, ',')) OR (@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = ''))
        And (b.AuthTypeId in (SELECT convert(int, value)
            FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
        And (csbc.ProcedureCode in (SELECT convert(nvarchar(16), value)
            FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))
        And (csbc.ClientLocationId in (SELECT convert(int, value)
            FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))
        And (csbc.ClientProviderId in (SELECT convert(int, value)
            FROM string_split(@ClientProviderIds, ',')) OR (@ClientProviderIds is null OR @ClientProviderIds = ''))		
		AND ((csbc.DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom is NULL)
		AND ((csbc.DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo is NULL)			
		AND ((csbc.ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
		AND ((csbc.ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)   
        AND csbc.IsSupplanted = 0
		AND csbc.ClientProviderId IS NOT NULL
	GROUP BY  csbc.ClientProviderId, SUBSTRING(TRIM(ProcedureCode),1,5) ,(Per.LastName + ',' + per.FirstName)

    END
GO
