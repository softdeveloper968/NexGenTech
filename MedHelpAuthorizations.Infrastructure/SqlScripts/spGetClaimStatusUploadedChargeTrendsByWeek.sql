﻿SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE or ALTER PROCEDURE [IntegratedServices].[spGetClaimStatusUploadedChargeTrendsByWeek]
	@ClientId int
    ,@DelimitedLineItemStatusIds nvarchar(MAX) = NULL
	--,@ClientInsuranceId int = NULL
	--,@AuthTypeId int = NULL 
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
      min(DATEADD(ww, DATEDIFF(ww, -1, CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END), -1)) as TrendingDate
      ,DATEPART(ww, CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END) as [Iso_Week] 
        ,CASE
            WHEN month(min(CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END)) = 1 
                    AND DATEPART(ww, CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END) >= 208
                THEN year(CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END) - 4  
            WHEN month(min(CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END)) = 1 
                    AND DATEPART(ww, CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END) >= 156
                THEN year(CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END) - 3 
            WHEN month(min(CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END)) = 1 
                    AND DATEPART(ww, CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END) >= 104
                THEN year(CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END) - 2 
            WHEN month(min(CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END)) = 1 
                    AND DATEPART(ww, CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END) >= 52
                THEN year(CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END) - 1 
            WHEN month(min(CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END)) = 12
                    AND DATEPART(ww, CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END) = 1
                THEN year(CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END) + 1 
            ELSE year(CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END) 
        END as [Iso_Year]
    , COUNT(c1.ClaimLevelMD5Hash) as [Quantity]
    , SUM(c1.BilledAmount) as [ChargedSum]
    ,'PaidAmountSum' = 0.00
    ,'ClaimLineItemStatus' = ''
    ,'ExceptionReasonCategory'= ''
    FROM [IntegratedServices].ClaimStatusBatchClaims c1
	JOIN(
			SELECT min(Id) as 'MinId', EntryMd5Hash as 'HashKey' --, min(CreatedOn) as 'ReceivedDate'
			FROM  [IntegratedServices].ClaimStatusBatchClaims 
			GROUP BY EntryMd5Hash
		) as c2 ON c1.Id = c2.MinId
	JOIN [IntegratedServices].ClaimStatusBatches as b  ON c1.ClaimStatusBatchId = b.Id
	JOIN [dbo].ClientInsurances as i ON b.ClientInsuranceId = i.Id 	
	 left join IntegratedServices.ClaimStatusTransactions as t on t.ClaimStatusBatchClaimId=c1.Id


	WHERE i.ClientId = @ClientId
		AND (c1.IsDeleted = 0)
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
GROUP BY year(CASE 
					WHEN @DateOfServiceFrom IS NOT NULL 
					THEN c1.DateOfServiceFrom 
					ELSE c1.ClaimBilledOn END), 
					DATEPART(ww,
                            CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END)

                  , DATEPART(ww, CASE 
                        WHEN @DateOfServiceFrom IS NOT NULL 
                        THEN c1.DateOfServiceFrom 
                        ELSE c1.ClaimBilledOn END), 
                        DATEPART(ww,
				                CASE WHEN @DateOfServiceFrom IS NOT NULL THEN c1.DateOfServiceFrom ELSE c1.ClaimBilledOn END)	
                ,c1.ClaimLevelMD5Hash
ORDER BY TrendingDate;
END
GO
