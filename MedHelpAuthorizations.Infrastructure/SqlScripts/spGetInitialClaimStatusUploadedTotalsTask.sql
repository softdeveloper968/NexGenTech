SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   PROCEDURE [IntegratedServices].[spGetInitialClaimStatusUploadedTotalsTask]
 @ClientId int = 3
,@DelimitedLineItemStatusIds nvarchar(MAX) = NULL    
,@ClientInsuranceId int = NULL
,@AuthTypeId int = NULL 
,@ProcedureCode nvarchar(24) = NULL
,@ExceptionReasonCategory int = NULL
,@ReceivedFrom DateTime = NULL
,@ReceivedTo DateTime = NULL
,@DateOfServiceFrom DateTime = NULL
,@DateOfServiceTo DateTime = NULL
,@TransactionDateFrom DateTime = NULL
,@TransactionDateTo DateTime = NULL
--,@ClaimBilledFrom DateTime = '2023-05-01'--Invalid syntax
--,@ClaimBilledTo DateTime = '2023-05-26'--Invalid syntax
,@ClaimBilledFrom DateTime = NULL
,@ClaimBilledTo DateTime = NULL
,@ClientInsuranceIds nvarchar(MAX)= null
,@ClientExceptionReasonCategoryIds nvarchar(MAX)= null
,@ClientAuthTypeIds nvarchar(MAX)= null
,@ClientProcedureCodes nvarchar(MAX)= null
,@PatientId int = NULL
AS
BEGIN
   SELECT  [clientInsurance].LookupName as 'ClientInsuranceName'
        ,'ClaimLineItemStatus' = CASE When ([claimLineItemStatus].code is null) Then 'Received/Upload' Else [claimLineItemStatus].code End 
		,'ClaimStatusExceptionReasonCategory' = CASE When ([claimStatusExceptionReasonCategory].Code is null) Then 'ClaimStatusExceptionReasonCategory' Else [claimStatusExceptionReasonCategory].Code End 
		,TRIM([claimStatusBatchClaim].[ProcedureCode]) as 'ProcedureCode'
        ,COUNT([claimStatusBatchClaim].ClaimLevelMD5Hash) as 'Quantity'
        ,SUM([claimStatusBatchClaim].BilledAmount) as 'ChargedSum'
        ,SUM([claimStatusTransaction].LineItemPaidAmount) as 'PaidAmountSum'
        ,SUM([claimStatusTransaction].TotalAllowedAmount) as 'AllowedAmountSum' 
        ,SUM([claimStatusTransaction].TotalNonAllowedAmount) as 'NonAllowedAmountSum'
FROM [IntegratedServices].ClaimStatusBatchClaims as [claimStatusBatchClaim]
        JOIN(
                SELECT min(Id) as 'MinId', EntryMd5Hash as 'HashKey' --, min(CreatedOn) as 'ReceivedDate'
                FROM  [IntegratedServices].ClaimStatusBatchClaims 
                GROUP BY EntryMd5Hash
            ) as [initialClaimStatusBatchClaim] ON [claimStatusBatchClaim].Id = [initialClaimStatusBatchClaim].MinId

-----Handle Exception Reason Category Ids-Included Claim status Transaction---
 LEFT JOIN [IntegratedServices].ClaimStatusTransactions as [claimStatusTransaction] ON [claimStatusTransaction].ClaimStatusBatchClaimId = [claimStatusBatchClaim].Id
 
 JOIN [IntegratedServices].ClaimStatusBatches as [claimStatusBatch]  ON [claimStatusBatchClaim].ClaimStatusBatchId = [claimStatusBatch].Id
 JOIN [dbo].ClientInsurances as [clientInsurance] ON [claimStatusBatch].ClientInsuranceId = [clientInsurance].Id 
 LEFT JOIN [IntegratedServices].ClaimLineItemStatuses as [claimLineItemStatus] ON [claimStatusTransaction].ClaimLineItemStatusId = [claimLineItemStatus].Id
 LEFT JOIN [IntegratedServices].ClaimStatusExceptionReasonCategories as [claimStatusExceptionReasonCategory] ON [claimStatusTransaction].ClaimStatusExceptionReasonCategoryId = [claimStatusExceptionReasonCategory].Id

 WHERE [clientInsurance].ClientId = @ClientId
 		AND ([claimStatusBatchClaim].IsDeleted = 0)

        ----AA-120----Multi-Select Filters------
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
		AND (([claimStatusBatchClaim].PatientId = @PatientId) OR (@PatientId IS NULL))		
GROUP BY [clientInsurance].LookupName, [claimStatusBatchClaim].ProcedureCode, [claimLineItemStatus].code, [claimStatusExceptionReasonCategory].code , [claimStatusBatchClaim].ClaimLevelMD5Hash
END
GO
