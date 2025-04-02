SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   PROCEDURE [IntegratedServices].[spGetClaimStatusInProcessTotals]
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
	,@ReceivedFrom DateTime = NULL
	,@ReceivedTo DateTime = NULL
	,@DateOfServiceFrom DateTime = NULL
	,@DateOfServiceTo DateTime = NULL
	,@TransactionDateFrom DateTime = NULL
	,@TransactionDateTo DateTime = NULL
	,@ClaimBilledFrom DateTime = NULL
	,@ClaimBilledTo DateTime = NULL
---------PAGINATION--------------
    ,@PageNumber AS INT
    ,@RowsOfPage AS INT
    ,@SortTypeCol AS VARCHAR(50)
    ,@searchString AS NVARCHAR(250)=''    
    ,@filterReportBy AS Varchar(50)=''
AS
BEGIN

    -----------Set Default values-------
    IF @PageNumber<=1
        BEGIN
            SET @PageNumber=1
        END
    IF @RowsOfPage <=0
        BEGIN
            SET @RowsOfPage=10
        END
    IF @SortTypeCol IS NULL OR @SortTypeCol = ''
        BEGIN
            SET @SortTypeCol = 'ASC'--'DESC'
        END
    If @filterReportBy is null or @filterReportBy = ''
        BEGIN
            Set @filterReportBy ='ClaimBilledDate'
        END
        
SELECT
        -- TRIM(c1.[ProcedureCode]) as 'ProcedureCode'
        COUNT(c1.ClaimLevelMD5Hash) as 'Quantity'
        ,SUM(c1.BilledAmount) as 'ChargedSum'
        ,SUM(t.TotalAllowedAmount) as 'AllowedAmountSum'
        ,CASE @filterReportBy WHEN 'ClaimBilledDate' THEN c1.ClaimBilledOn
                             WHEN 'DateOfService' THEN c1.DateOfServiceFrom
                             WHEN 'ReceivedDate' THEN c1.CreatedOn
                             WHEN 'TransactionDate' THEN COALESCE(t.LastModifiedOn, t.CreatedOn)--TAPI-125: As Per discussion with Jim/Kevin.
         END AS 'filterReportByDate'

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
					null,
					null, 
					@ClientInsuranceIds, 
					@ClientExceptionReasonCategoryIds, 
					@ClientAuthTypeIds, 
					@ClientProcedureCodes, 
					null,
					null
				)
	) as c2 ON c1.ClaimLevelMd5Hash = c2.ClaimLevelMd5Hash
        JOIN [IntegratedServices].ClaimStatusBatches as b  ON c1.ClaimStatusBatchId = b.Id
        JOIN [dbo].ClientInsurances as i ON b.ClientInsuranceId = i.Id 
        left Join IntegratedServices.ClaimStatusTransactions as t on c1.ClaimStatusTransactionId=t.Id
 WHERE i.ClientId = @ClientId
 		AND (c1.IsDeleted = 0)
		--AND ((b.ClientInsuranceId = @ClientInsuranceId) OR (@ClientInsuranceId = 0 OR @ClientInsuranceId is null))
		--AND (b.AuthTypeId = @AuthTypeId OR (@AuthTypeId = 0 Or @AuthTypeId is null))
		--AND (c1.ProcedureCode = @ProcedureCode OR (@ProcedureCode = '' OR @ProcedureCode is null))
        
        And (b.ClientInsuranceId in (SELECT convert(int, value)
			FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
		And (t.ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value)
			FROM string_split(@ClientExceptionReasonCategoryIds, ',')) OR (@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = ''))
		And (b.AuthTypeId in (SELECT convert(int, value)
			FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
		And (c1.ProcedureCode in (SELECT convert(nvarchar(16), value)
			FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))
			
        --AND (c1.ClaimStatusTransactionId is null OR t.ClaimLineItemStatusId in (10,17))
		--AND ((c1.CreatedOn >= @ReceivedFrom) OR @ReceivedFrom IS NULL)
		--AND ((c1.CreatedOn <= @ReceivedTo) OR @ReceivedTo IS NULL)
		--AND ((c1.DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom is NULL)
		--AND ((c1.DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceFrom is NULL)			
		--AND ((c1.ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
		--AND ((c1.ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)
        
    	AND (@searchString IS NULL OR 
    		c1.PatientFirstName LIKE '%'+@searchString+'%' OR 
    		c1.PatientLastName LIKE '%'+@searchString+'%')     
        AND c1.IsDeleted = 0 
        AND c1.IsSupplanted = 0
            
GROUP BY 
        CASE @filterReportBy WHEN 'ClaimBilledDate' THEN c1.ClaimBilledOn
                             WHEN 'DateOfService' THEN c1.DateOfServiceFrom
                             WHEN 'ReceivedDate' THEN c1.CreatedOn
                             WHEN 'TransactionDate' THEN COALESCE(t.LastModifiedOn, t.CreatedOn)
         END 
         ,c1.ClaimLevelMD5Hash

ORDER BY 
        CASE 
            WHEN @SortTypeCol ='ASC' 
            THEN CASE @filterReportBy WHEN 'ClaimBilledDate' THEN c1.ClaimBilledOn
                             WHEN 'DateOfService' THEN c1.DateOfServiceFrom
                             WHEN 'ReceivedDate' THEN c1.CreatedOn
                             WHEN 'TransactionDate' THEN COALESCE(t.LastModifiedOn, t.CreatedOn)
                 END  
        END,

        CASE 
            WHEN @SortTypeCol ='DESC' 
            THEN  CASE @filterReportBy WHEN 'ClaimBilledDate' THEN c1.ClaimBilledOn
                             WHEN 'DateOfService' THEN c1.DateOfServiceFrom
                             WHEN 'ReceivedDate' THEN c1.CreatedOn
                             WHEN 'TransactionDate' THEN COALESCE(t.LastModifiedOn, t.CreatedOn)
                 END 
        END DESC

        ------------------------UNCOMMENT-BELOW CODE IF PAGINATION REQUIRED--------------------------------------------

        -- OFFSET (@PageNumber-1)*@RowsOfPage ROWS
        -- FETCH NEXT @RowsOfPage ROWS ONLY
END
GO
