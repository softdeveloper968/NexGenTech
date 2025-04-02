SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   PROCEDURE [IntegratedServices].[spVerifyDailyClaimReportTotals]
	@ClientId int
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
	,@ClaimBilledFrom DateTime = NULL
	,@ClaimBilledTo DateTime = NULL
    --,@claimReviewedIds nvarchar(MAX) = '10,17'
    --,@claimNotAdjIds nvarchar(MAX) = '6,10'
    ,@claimPaidApprovedIds nvarchar(MAX) = '1,2,14'

AS
BEGIN

Select 
       t.Id as transactionId
       ,st.Id
       ,st.Code as ClaimLineItemStatusCode
       ,b.BatchNumber
       ,c1.PolicyNumber
       ,c1.ClaimNumber
       ,COALESCE(t.LastModifiedOn, t.CreatedOn) as transactionDate
       ,c1.CreatedOn as ClaimReceivedDate
       ,c1.ClaimBilledOn as ClaimBilledDate
       ,c1.DateOfServiceFrom as ClaimServiceDateFrom
       ,c1.DateOfServiceTo as ClaimServiceDateTo
       , COUNT(c1.ClaimLevelMD5Hash) as Quantity -- EN-56
       
FROM [IntegratedServices].ClaimStatusBatchClaims as c1
    JOIN [IntegratedServices].ClaimStatusBatches as b  ON c1.ClaimStatusBatchId = b.Id
    join IntegratedServices.ClaimStatusTransactions as t on t.ClaimStatusBatchClaimId = c1.Id
    join IntegratedServices.ClaimLineItemStatuses as st on t.ClaimLineItemStatusId=st.Id

Where (t.ClaimLineItemStatusId IN (SELECT convert(int, value) FROM string_split(@claimPaidApprovedIds, ',')) OR (@claimPaidApprovedIds is null OR @claimPaidApprovedIds = ''))     
        AND c1.IsDeleted = 0 AND c1.IsSupplanted = 0

--Todo Exclude reviewed status [Error]
group by t.Id,b.BatchNumber
        ,c1.PolicyNumber
        ,c1.ClaimNumber
        ,st.Code
        ,st.Id
        ,t.LastModifiedOn
        ,t.CreatedOn
        ,c1.CreatedOn
        ,c1.ClaimBilledOn
        ,c1.DateOfServiceFrom
        ,c1.DateOfServiceTo
        ,c1.ClaimLevelMD5Hash

END
GO
