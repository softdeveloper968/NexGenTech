
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   PROCEDURE [IntegratedServices].[spGetClaimStatusUploadedTotals]
	@ClientId int
    ,@DelimitedLineItemStatusIds nvarchar(MAX) = NULL
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
   
        COUNT([claimStatusBatchClaim].ClaimLevelMD5Hash) as 'Quantity',
        SUM([claimStatusBatchClaim].BilledAmount) as 'ChargedSum',
        SUM(claimStatusTransaction.TotalAllowedAmount) as 'AllowedAmountSum',
        CASE @filterReportBy WHEN 'ClaimBilledDate' THEN [claimStatusBatchClaim].ClaimBilledOn
                             WHEN 'DateOfService' THEN [claimStatusBatchClaim].DateOfServiceFrom
                             WHEN 'ReceivedDate' THEN [claimStatusBatchClaim].CreatedOn
                             WHEN 'TransactionDate' THEN '0001-01-01'--TAPI-125: As Per discussion with Jim/Kevin.--Set as null/min. Date when getting upload totals
         END AS 'filterReportByDate'

    FROM [IntegratedServices].ClaimStatusBatchClaims as [claimStatusBatchClaim]
     JOIN [IntegratedServices].ClaimStatusBatches as [claimStatusBatch]  ON [claimStatusBatchClaim].ClaimStatusBatchId = [claimStatusBatch].Id
     JOIN [dbo].ClientInsurances as [clientInsurance] ON [claimStatusBatch].ClientInsuranceId = [clientInsurance].Id
     left join IntegratedServices.ClaimStatusTransactions as [claimStatusTransaction] on [claimStatusTransaction].ClaimStatusBatchClaimId = [claimStatusBatchClaim].Id
      
    WHERE [clientInsurance].ClientId = @ClientId

            ------Multi Select Filter-------
		And ([claimStatusBatch].ClientInsuranceId in (SELECT convert(int, value)
			FROM string_split(@ClientInsuranceIds, ',')) OR (@ClientInsuranceIds is null OR @ClientInsuranceIds = ''))
		And ([claimStatusTransaction].ClaimStatusExceptionReasonCategoryId in (SELECT convert(int, value)
			FROM string_split(@ClientExceptionReasonCategoryIds, ',')) OR (@ClientExceptionReasonCategoryIds is null OR @ClientExceptionReasonCategoryIds = ''))
		And ([claimStatusBatch].AuthTypeId in (SELECT convert(int, value)
			FROM string_split(@ClientAuthTypeIds, ',')) OR (@ClientAuthTypeIds is null OR @ClientAuthTypeIds = ''))
		And ([claimStatusBatchClaim].ProcedureCode in (SELECT convert(nvarchar(16), value)
			FROM string_split(@ClientProcedureCodes, ',')) OR (@ClientProcedureCodes is null OR @ClientProcedureCodes = ''))
			
    		AND (([claimStatusBatchClaim].CreatedOn >= @ReceivedFrom) OR @ReceivedFrom IS NULL)
    		AND (([claimStatusBatchClaim].CreatedOn <= @ReceivedTo) OR @ReceivedTo IS NULL)
    		AND (([claimStatusBatchClaim].DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom is NULL)
    		AND (([claimStatusBatchClaim].DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceFrom is NULL)			
    		AND (([claimStatusBatchClaim].ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
    		AND (([claimStatusBatchClaim].ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)     
            AND [claimStatusBatchClaim].IsDeleted = 0 
            AND [claimStatusBatchClaim].IsSupplanted = 0
    
    		AND (@searchString IS NULL OR 
    			[claimStatusBatchClaim].PatientFirstName LIKE '%'+@searchString+'%' OR 
    			[claimStatusBatchClaim].PatientLastName LIKE '%'+@searchString+'%')

    GROUP BY 
        CASE @filterReportBy WHEN 'ClaimBilledDate' THEN [claimStatusBatchClaim].ClaimBilledOn
                             WHEN 'DateOfService' THEN [claimStatusBatchClaim].DateOfServiceFrom
                             WHEN 'ReceivedDate' THEN [claimStatusBatchClaim].CreatedOn
                             WHEN 'TransactionDate' THEN '0001-01-01'
         END 
         ,[claimStatusBatchClaim].ClaimLevelMD5Hash

    ORDER BY 
        CASE 
            WHEN @SortTypeCol ='ASC' 
            THEN CASE @filterReportBy WHEN 'ClaimBilledDate' THEN [claimStatusBatchClaim].ClaimBilledOn
                             WHEN 'DateOfService' THEN [claimStatusBatchClaim].DateOfServiceFrom
                             WHEN 'ReceivedDate' THEN [claimStatusBatchClaim].CreatedOn
                             WHEN 'TransactionDate' THEN '0001-01-01'
                 END  
        END,

        CASE 
            WHEN @SortTypeCol ='DESC' 
            THEN  CASE @filterReportBy WHEN 'ClaimBilledDate' THEN [claimStatusBatchClaim].ClaimBilledOn
                             WHEN 'DateOfService' THEN [claimStatusBatchClaim].DateOfServiceFrom
                             WHEN 'ReceivedDate' THEN [claimStatusBatchClaim].CreatedOn
                             WHEN 'TransactionDate' THEN '0001-01-01'
                 END 
        END DESC
        
        ------------------------UNCOMMENT-BELOW CODE IF PAGINATION REQUIRED--------------------------------------------

            --OFFSET (@PageNumber-1)*@RowsOfPage ROWS
            --FETCH NEXT @RowsOfPage ROWS ONLY

END
GO
