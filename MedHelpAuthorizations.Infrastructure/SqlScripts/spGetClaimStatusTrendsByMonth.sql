
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE or ALTER PROCEDURE [IntegratedServices].[spGetClaimStatusTrendsByMonth]
	@ClientId int
    ,@DelimitedLineItemStatusIds nvarchar(MAX) = NULL
	--,@ClientInsuranceId int = NULL 
	--,@AuthTypeId int  = NULL
	--,@ProcedureCode nvarchar(24) = NULL
	--,@ExceptionReasonCategory int = NULL
	,@ClientInsuranceIds nvarchar(MAX)= null
	,@ClientExceptionReasonCategoryIds nvarchar(MAX)= null
	,@ClientAuthTypeIds nvarchar(MAX)= null
	,@ClientProcedureCodes nvarchar(MAX)= null
	,@DateOfServiceFrom DateTime = NULL
	,@DateOfServiceTo DateTime = NULL
	,@ClaimBilledFrom DateTime = NULL
	,@ClaimBilledTo DateTime = NULL
AS
BEGIN
    SELECT 
		--i.LookupName as 'ClientInsuranceName'
		--,a.Name as 'ServiceType'
        dateadd(month, datediff(month, 0, CASE WHEN @DateOfServiceFrom is NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END),0) as 'TrendingDate'
		--,TRIM(c1.[ProcedureCode]) as 'ProcedureCode'
        ,COUNT(c1.ClaimLevelMD5Hash) as 'Quantity'
        ,SUM(c1.BilledAmount) as 'ChargedSum'        
		,SUM(t.LineItemPaidAmount) as 'PaidAmountSum'
        ,0 as 'Iso_Week'
        ,0 as 'Iso_Year'
		,cs.Code  as 'ClaimLineItemStatus'
        ,cs.Id as 'ClaimLineItemStatusId'
        ,er.Code as 'ExceptionReasonCategory'
        FROM [IntegratedServices].ClaimStatusBatchClaims as c1
        JOIN(
                SELECT min(Id) as 'MinId', EntryMd5Hash as 'HashKey' --, min(CreatedOn) as 'ReceivedDate'
                FROM  [IntegratedServices].ClaimStatusBatchClaims 
                GROUP BY EntryMd5Hash
            ) as c2 ON c1.Id = c2.MinId
 JOIN [IntegratedServices].ClaimStatusTransactions as t ON t.ClaimStatusBatchClaimId = c1.Id
 JOIN [IntegratedServices].ClaimStatusBatches as b  ON c1.ClaimStatusBatchId = b.Id
 JOIN [dbo].AuthTypes as a ON b.AuthTypeId = a.Id 
 JOIN [dbo].ClientInsurances as i ON b.ClientInsuranceId = i.Id 
 JOIN [IntegratedServices].ClaimLineItemStatuses as cs ON t.ClaimLineItemStatusId = cs.Id
 JOIN [IntegratedServices].ClaimStatusExceptionReasonCategories as er ON t.ClaimStatusExceptionReasonCategoryId = er.Id
 WHERE i.ClientId = @ClientId
 		AND (c1.IsDeleted = 0)
		AND (t.ClaimLineItemStatusId IN (SELECT convert(int, value) FROM string_split(@DelimitedLineItemStatusIds, ',')) OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = ''))
		--AND ((b.ClientInsuranceId = @ClientInsuranceId) OR (@ClientInsuranceId = 0 OR @ClientInsuranceId is null))
		--AND (b.AuthTypeId = @AuthTypeId OR (@AuthTypeId = 0 Or @AuthTypeId is null))
		--AND (c1.ProcedureCode = @ProcedureCode OR (@ProcedureCode = '' OR @ProcedureCode is null))		
		------Multi Select Filter-------
		And (b.ClientInsuranceId in (SELECT convert(int, value)
			FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
		And (t.ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value)
			FROM string_split(@ClientExceptionReasonCategoryIds, ',')) OR (@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = ''))
		And (b.AuthTypeId in (SELECT convert(int, value)
			FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
		And (c1.ProcedureCode in (SELECT convert(nvarchar(16), value)
			FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))
			
		AND ((c1.DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom is NULL)
		AND ((c1.DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo is NULL)			
		AND ((c1.ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
		AND ((c1.ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)
		AND c1.IsDeleted = 0 
        AND c1.IsSupplanted = 0
GROUP BY 
		dateadd(month, datediff(month, 0, (CASE WHEN @DateOfServiceFrom is NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END)), 0),
		cs.Code, cs.Id,
		er.Code
        ,c1.ClaimLevelMD5Hash
ORDER BY 'TrendingDate',		
        cs.Code,
        er.Code
--GROUP BY i.LookupName,
--		a.Name, 
--		c1.ProcedureCode, 
--		dateadd(month, datediff(month, 0, (CASE WHEN @DateOfServiceFrom is NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END)), 0),
--		cs.Code, 
--		er.Code 
--ORDER BY 'TrendingDate',
--		i.LookupName,
--		a.Name,
--		c1.ProcedureCode,
--        cs.Code,
--        er.Code
END
GO
