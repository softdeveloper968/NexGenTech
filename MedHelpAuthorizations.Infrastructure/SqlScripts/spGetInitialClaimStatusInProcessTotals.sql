SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [IntegratedServices].[spGetInitialClaimStatusInProcessTotals]
	@ClientId int
	,@DelimitedLineItemStatusIds nvarchar(MAX) = NULL    
	--,@ClientInsuranceId int = NULL 
	--,@AuthTypeId int = NULL 
	--,@ProcedureCode nvarchar(24) = NULL
	--,@ExceptionReasonCategory int = NULL
	,@ReceivedFrom DateTime = NULL
	,@ReceivedTo DateTime = NULL
	,@DateOfServiceFrom DateTime = NULL
	,@DateOfServiceTo DateTime = NULL
	,@TransactionDateFrom DateTime = NULL
	,@TransactionDateTo DateTime = NULL
	,@ClaimBilledFrom DateTime = NULL
	,@ClaimBilledTo DateTime = NULL
	,@ClientInsuranceIds nvarchar(MAX)= null
	,@ClientExceptionReasonCategoryIds nvarchar(MAX)= null
	,@ClientAuthTypeIds nvarchar(MAX)= null
	,@ClientProcedureCodes nvarchar(MAX)= null
AS
BEGIN
   SELECT  i.LookupName as 'ClientInsuranceName'
        ,'ClaimLineItemStatus' = 'In-Process'
		,'ClaimStatusExceptionReasonCategory' = 'ClaimStatusExceptionReasonCategory'
		,TRIM(c1.[ProcedureCode]) as 'ProcedureCode'
        ,COUNT(c1.ClaimLevelMD5Hash) as 'Quantity'
        ,SUM(c1.BilledAmount) as 'ChargedSum'
        ,'AllowedAmountSum' = 0.00
        ,'NonAllowedAmountSum' = 0.00
		,'PaidAmountSum' = 0.00
        FROM [IntegratedServices].ClaimStatusBatchClaims as c1
        JOIN(
                SELECT min(Id) as 'MinId', EntryMd5Hash as 'HashKey' --, min(CreatedOn) as 'ReceivedDate'
                FROM  [IntegratedServices].ClaimStatusBatchClaims 
                GROUP BY EntryMd5Hash
            ) as c2 ON c1.Id = c2.MinId

 JOIN [IntegratedServices].ClaimStatusBatches as b  ON c1.ClaimStatusBatchId = b.Id
 JOIN [dbo].ClientInsurances as i ON b.ClientInsuranceId = i.Id 
 -----Handle Exception Reason Category Ids-Included Claim status Transaction---
 LEFT JOIN [IntegratedServices].ClaimStatusTransactions as t ON t.ClaimStatusBatchClaimId = c1.Id

 WHERE i.ClientId = @ClientId
 		AND (c1.IsDeleted = 0)
		--AND ((b.ClientInsuranceId = @ClientInsuranceId) OR (@ClientInsuranceId = 0 OR @ClientInsuranceId is null))
		--AND (b.AuthTypeId = @AuthTypeId OR (@AuthTypeId = 0 Or @AuthTypeId is null))
		--AND (c1.ProcedureCode = @ProcedureCode OR (@ProcedureCode = '' OR @ProcedureCode is null))
		 ----AA-120----Multi-Select Filters------
        And (b.ClientInsuranceId in (SELECT convert(int, value)
			FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
		And (t.ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value)
			FROM string_split(@ClientExceptionReasonCategoryIds, ',')) OR (@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = ''))
		And (b.AuthTypeId in (SELECT convert(int, value)
			FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
		And (c1.ProcedureCode in (SELECT convert(nvarchar(16), value)
			FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))
		
		AND ((c1.CreatedOn >= @ReceivedFrom) OR @ReceivedFrom IS NULL)
		AND ((c1.CreatedOn <= @ReceivedTo) OR @ReceivedTo IS NULL)
		AND ((c1.DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom is NULL)
		AND ((c1.DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo is NULL)			
		AND ((c1.ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
		AND ((c1.ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)
        AND c1.ClaimStatusTransactionId is null     
GROUP BY i.LookupName, c1.ProcedureCode, c1.ClaimLevelMD5Hash
END
GO
