-----------  DID NOT COMPLETE YET __________________
------NEED TO ADD THIS CRITERIA ______
 -- Criteria = c => true;
            -- Criteria = Criteria.And(c => !c.IsDeleted);
            -- Criteria = Criteria.And(d => !d.ExceptionReason.Contains("Voided"));
            -- Criteria = Criteria.And(d => d.ClaimLineItemStatusId == ClaimLineItemStatusEnum.Denied 
                -- || d.ClaimLineItemStatusId == ClaimLineItemStatusEnum.Rejected
                -- || d.ClaimLineItemStatusId == ClaimLineItemStatusEnum.MemberNotFound
                -- || d.ClaimLineItemStatusId == ClaimLineItemStatusEnum.ClaimNotFound
                -- || d.ClaimLineItemStatusId == ClaimLineItemStatusEnum.UnMatchedProcedureCode
                -- || d.ClaimLineItemStatusId == ClaimLineItemStatusEnum.Error
                -- || d.ClaimLineItemStatusId == ClaimLineItemStatusEnum.Ignored);
				
				
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [IntegratedServices].[spGetInitialClaimStatusDenialTotals]
	@ClientId int
	,@DelimitedLineItemStatusIds nvarchar(48) = NULL 
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
AS
BEGIN
   SELECT i.LookupName as 'ClientInsuranceName'
        ,s.Code as 'ClaimLineItemStatus'
		,er.[Description] as 'ClaimStatusExceptionReasonCategory'
		,TRIM(tbc.ProcedureCode) as 'ProcedureCode'
        ,COUNT(t.Id) as 'Quantity'
        ,SUM(tbc.BilledAmount) as 'ChargedSum'
        ,SUM(t.LineItemPaidAmount) as 'PaidAmountSum'
        ,SUM(t.TotalAllowedAmount) as 'AllowedAmountSum' 
        ,SUM(t.TotalNonAllowedAmount) as 'NonAllowedAmountSum' 
FROM [IntegratedServices].ClaimStatusTransactions as t
JOIN(  
    SELECT  *
        FROM [IntegratedServices].ClaimStatusBatchClaims as c1
        JOIN(
                SELECT min(Id) as 'MinId', EntryMd5Hash as 'HashKey' --, min(CreatedOn) as 'ReceivedDate'
                FROM  [IntegratedServices].ClaimStatusBatchClaims 
                GROUP BY EntryMd5Hash
            ) as c2 ON c1.Id = c2.MinId
    ) as tbc ON t.ClaimStatusBatchClaimId = tbc.Id
JOIN [IntegratedServices].ClaimStatusBatches as b  ON tbc.ClaimStatusBatchId = b.Id
JOIN [dbo].ClientInsurances as i ON b.ClientInsuranceId = i.Id 
JOIN [IntegratedServices].ClaimLineItemStatuses as s ON t.ClaimLineItemStatusId = s.Id
JOIN [IntegratedServices].ClaimStatusExceptionReasonCategories as er ON t.ClaimStatusExceptionReasonCategoryId = er.Id
WHERE i.ClientId = @ClientId
		AND (t.IsDeleted = 0 AND tbc.IsDeleted = 0)
		AND NOT (s.Code = 'Voided')
		AND (s.Code IN ('Denied','Rejected','MemberNotFound','ClaimNotFound','UnMatchedProcedureCode','Error','Ignored'))
		AND ((b.ClientInsuranceId = @ClientInsuranceId) OR @ClientInsuranceId = 0)
		AND (b.AuthTypeId = @AuthTypeId OR @AuthTypeId = 0)
		AND (tbc.ProcedureCode = @ProcedureCode OR (@ProcedureCode = '' OR @ProcedureCode is null))
		AND (t.ClaimStatusExceptionReasonCategoryId = @ExceptionReasonCategory OR @ExceptionReasonCategory = 0)
		AND ((tbc.CreatedOn >= @ReceivedFrom) OR @ReceivedFrom IS NULL)
		AND ((tbc.CreatedOn <= @ReceivedTo) OR @ReceivedTo IS NULL)
		AND ((tbc.DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom IS NULL)
		AND ((tbc.DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo IS NULL)			
		AND ((tbc.ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
		AND ((tbc.ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)
		AND ((t.CreatedOn >= @TransactionDateFrom) OR  @TransactionDateFrom IS NULL) 
		AND ((t.CreatedOn <= @TransactionDateTo) OR  @TransactionDateTo IS NULL) 
GROUP BY i.LookupName, s.Code, tbc.ProcedureCode, er.Description 
END
GO