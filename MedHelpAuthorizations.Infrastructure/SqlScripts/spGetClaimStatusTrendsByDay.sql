SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE or ALTER PROCEDURE [IntegratedServices].[spGetClaimStatusTrendsByDay]
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
        c1.DateOfServiceFrom
        ,c1.ClaimBilledOn   
        ,COUNT(c1.ClaimLevelMD5Hash) as [Quantity]
        ,SUM(c1.BilledAmount) as [ChargedSum]
        ,SUM(t.LineItemPaidAmount) as 'PaidAmountSum'
        ,t.ClaimLineItemStatusId  as 'ClaimLineItemStatusId'
        ,t.ClaimStatusExceptionReasonCategoryId as 'ExceptionReasonCategoryId'
        ,c1.ProcedureCode
        ,a.Name as 'ServiceType'
        ,a.Id as 'ServiceTypeId'
        ,i.LookupName as 'ClientInsuranceLookupName'
    	,i.Id as 'ClientinsuranceId'
        ,min(DATEADD(ww, DATEDIFF(ww, -1, c1.DateOfServiceFrom), -1)) as FirstDateOfWeekBilledOn
		,min(DATEADD(ww, DATEDIFF(ww, -1, c1.DateOfServiceFrom), -1)) as FirstDateOfWeekServiceDate
		,DATEPART(ww, c1.ClaimBilledOn) as 'WeekNumberBilledOn'
		,DATEPART(ww, c1.DateOfServiceFrom) as 'WeekNumberServiceDate'
		,CASE
			WHEN month(min(c1.ClaimBilledOn)) = 1 
					AND DATEPART(ww, c1.ClaimBilledOn) >= 208
				THEN year(c1.ClaimBilledOn) - 4  
			WHEN month(min(c1.ClaimBilledOn)) = 1 
					AND DATEPART(ww, c1.ClaimBilledOn) >= 156
				THEN year(c1.ClaimBilledOn) - 3 
			WHEN month(min(c1.ClaimBilledOn)) = 1 
					AND DATEPART(ww, c1.ClaimBilledOn) >= 104
				THEN year(c1.ClaimBilledOn) - 2 
			WHEN month(min(c1.ClaimBilledOn)) = 1 
					AND DATEPART(ww, c1.ClaimBilledOn) >= 52
				THEN year(c1.ClaimBilledOn) - 1 
			WHEN month(min(c1.ClaimBilledOn)) = 12
					AND DATEPART(ww, c1.ClaimBilledOn) = 1
				THEN year(c1.ClaimBilledOn) + 1 
			ELSE year(c1.ClaimBilledOn)
		END as 'WeekYearBilledOn'
		,CASE
			WHEN month(min(c1.DateOfServiceFrom)) = 1 
					AND DATEPART(ww, c1.DateOfServiceFrom) >= 208
				THEN year(c1.DateOfServiceFrom) - 4  
			WHEN month(min(c1.DateOfServiceFrom)) = 1 
					AND DATEPART(ww, c1.DateOfServiceFrom) >= 156
				THEN year(c1.DateOfServiceFrom) - 3 
			WHEN month(min(c1.DateOfServiceFrom)) = 1 
					AND DATEPART(ww, c1.DateOfServiceFrom) >= 104
				THEN year(c1.DateOfServiceFrom) - 2 
			WHEN month(min(c1.DateOfServiceFrom)) = 1 
					AND DATEPART(ww, c1.DateOfServiceFrom) >= 52
				THEN year(c1.DateOfServiceFrom) - 1 
			WHEN month(min(c1.DateOfServiceFrom)) = 12
					AND DATEPART(ww, c1.DateOfServiceFrom) = 1
				THEN year(c1.DateOfServiceFrom) + 1 
			ELSE year(c1.DateOfServiceFrom) 
		END as 'WeekYearServiceDate'
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
    --JOIN [IntegratedServices].ClaimLineItemStatuses as cs ON t.ClaimLineItemStatusId = cs.ClaimLineItemStatusId
    --JOIN [IntegratedServices].ClaimStatusExceptionReasonCategories as er ON t.ClaimStatusExceptionReasonCategoryId = er.ClaimStatusExceptionReasonCategoryId

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

    GROUP BY c1.DateOfServiceFrom 
            ,c1.ClaimBilledOn  
            ,t.ClaimLineItemStatusId 
            ,t.ClaimStatusExceptionReasonCategoryId
            ,c1.ProcedureCode
            ,a.Name
            ,a.Id
            ,i.LookupName
            ,i.Id		
            ,c1.ClaimLevelMD5Hash	
    ORDER BY c1.ClaimBilledOn
            , c1.DateOfServiceFrom
            , t.ClaimLineItemStatusId
            , t.ClaimStatusExceptionReasonCategoryId
            , c1.ProcedureCode
            , a.Name
            , i.LookupName
END
GO
