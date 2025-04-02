
/****** Object:  StoredProcedure [IntegratedServices].[spGetInitialClaimStatusDetails]    Script Date: 12/18/2023 11:37:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create OR ALTER PROCEDURE [IntegratedServices].[spGetInitialClaimStatusDetails]
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
    ,@PatientId int = NULL
    ,@ClientLocationIds nvarchar(MAX)= null

AS
BEGIN
   														
    SELECT  c1.PatientLastName,
            c1.PatientFirstName,
            c1.DateOfBirth,
            c1.PolicyNumber,
            c1.ClaimNumber as 'OfficeClaimNumber',
            i.LookupName as 'PayerName',   
            c1.ClaimNumber as 'PayerClaimNumber',
            t.LineItemControlNumber as 'PayerLineItemControlNumber',
            c1.DateOfServiceFrom,
            c1.DateOfServiceTo,
            c1.ProcedureCode,
            c1.ClaimBilledOn,
            c1.BilledAmount,
            t.TotalAllowedAmount as 'AllowedAmount',
            t.TotalNonAllowedAmount as 'NonAllowedAmount',
            a.Name as 'ServiceType',
            cs.Code as 'ClaimLineItemStatus',
            cs.Id as 'ClaimLineItemStatusId',
            t.ClaimLineItemStatusValue,
            er.Code as 'ExceptionReasonCategory',
            t.ExceptionReason,
            t.ExceptionRemark,
            t.RemarkCode,
            t.RemarkDescription,
            t.ReasonCode,
            t.ReasonDescription,
            t.DeductibleAmount,
            t.CopayAmount,
            t.CoinsuranceAmount,
            t.CobAmount,
            t.PenalityAmount,
            t.LineItemPaidAmount,
            t.CheckNumber,
            t.CheckDate,
            t.CheckPaidAmount,
            t.EligibilityInsurance,
            t.EligibilityPolicyNumber,
            t.EligibilityFromDate,
            t.EligibilityStatus,
            b.BatchNumber,
            c1.CreatedOn as 'AitClaimReceivedDate',
            COALESCE(t.LastModifiedOn, t.CreatedOn) as 'TransactionDate',
            t.VerifiedMemberId,
            t.CobLastVerified,
            t.LastActiveEligibleDateRange,
            t.PrimaryPayer,
            t.PrimaryPolicyNumber,
            t.PartA_EligibilityFrom,
            t.PartA_EligibilityTo,
            t.PartA_DeductibleFrom,
            t.PartA_DeductibleToDate,
            t.PartA_RemainingDeductible,
            t.PartB_EligibilityFrom,
            t.PartB_EligibilityTo,
            t.PartB_DeductibleFrom,
            t.PartB_DeductibleTo,
            t.PartB_RemainingDeductible,
            t.OtCapYearFrom,
            t.OtCapYearTo,
            t.OtCapUsedAmount,
            t.PtCapYearFrom,
            t.PtCapYearTo,
            t.PtCapUsedAmount,
            t.PaymentType as 'PaymentType',
            PrvPer.LastName + ',' + PrvPer.FirstName as 'ProviderName',
            cl.[Name] as 'ClientLocationName',
            cl.Npi as 'ClientLocationNpi',
			--h.CreatedOn as 'LastHistoryCreatedOn',
			c1.Id as 'ClaimStatusBatchClaimId'
        FROM [IntegratedServices].ClaimStatusBatchClaims as c1
        JOIN(
				SELECT min(Id) as 'MinId', EntryMd5Hash as 'HashKey' --, min(CreatedOn) as 'ReceivedDate'
				FROM  [IntegratedServices].ClaimStatusBatchClaims 
				GROUP BY EntryMd5Hash
            ) as c2 ON c1.Id = c2.MinId
 JOIN [IntegratedServices].ClaimStatusTransactions as t ON t.ClaimStatusBatchClaimId = c1.Id
 JOIN [IntegratedServices].ClaimLineItemStatuses as cs ON t.ClaimLineItemStatusId = cs.Id
 JOIN [IntegratedServices].ClaimStatusBatches as b  ON c1.ClaimStatusBatchId = b.Id
 JOIN  [dbo].AuthTypes as a  ON b.AuthTypeId = a.Id
 JOIN [dbo].ClientInsurances as i ON b.ClientInsuranceId = i.Id 
 JOIN [IntegratedServices].ClaimLineItemStatuses as s ON t.ClaimLineItemStatusId = s.Id
 JOIN [IntegratedServices].ClaimStatusExceptionReasonCategories as er ON t.ClaimStatusExceptionReasonCategoryId = er.Id
 JOIN Providers as Prv ON c1.ClientProviderId = Prv.Id
 JOIN Persons as PrvPer ON Prv.PersonId = PrvPer.Id
 JOIN CLientLocations as cl ON c1.ClientLocationId = cl.Id
 -- To get Last Created on order by desc EN-127
  --OUTER APPLY (
  --  SELECT TOP 1 CreatedOn
  --  FROM [IntegratedServices].[ClaimStatusTransactionHistories] AS hx
  --  WHERE hx.ClaimStatusTransactionId = t.Id
  --  ORDER BY hx.CreatedOn DESC
--) AS h
WHERE i.ClientId = @ClientId
		AND (t.IsDeleted = 0 AND c1.IsDeleted = 0)
		AND (t.ClaimLineItemStatusId IN (SELECT convert(int, value) FROM string_split(@DelimitedLineItemStatusIds, ',')) OR (@DelimitedLineItemStatusIds is null OR @DelimitedLineItemStatusIds = ''))
		--AND (b.ClientInsuranceId = @ClientInsuranceId OR (@ClientInsuranceId = 0 OR @ClientInsuranceId is null))
		--AND (b.AuthTypeId = @AuthTypeId OR (@AuthTypeId = 0 Or @AuthTypeId is null))
		--AND (c1.ProcedureCode = @ProcedureCode OR (@ProcedureCode = '' OR @ProcedureCode is null))
		--AND (t.ClaimStatusExceptionReasonCategoryId = @ExceptionReasonCategory OR (@ExceptionReasonCategory = 0 OR @ExceptionReasonCategory is NULL))
        ------Multi Select Filter-------
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
		AND ((c1.DateOfServiceFrom >= @DateOfServiceFrom) OR @DateOfServiceFrom IS NULL)
		AND ((c1.DateOfServiceTo <= @DateOfServiceTo) OR @DateOfServiceTo IS NULL)			    
		AND ((c1.ClaimBilledOn >= @ClaimBilledFrom) OR @ClaimBilledFrom IS NULL)
		AND ((c1.ClaimBilledOn <= @ClaimBilledTo) OR @ClaimBilledTo is null)
		AND ((COALESCE(t.LastModifiedOn, t.CreatedOn) >= @TransactionDateFrom) OR @TransactionDateFrom is NULL ) 
        AND ((COALESCE(t.LastModifiedOn, t.CreatedOn) <= @TransactionDateTo) OR @TransactionDateTo is NULL)     
        AND (c1.PatientId = @PatientId OR (@PatientId IS NULL OR @PatientId = 0))
        And (c1.ClientLocationId in (SELECT convert(int, value)
			FROM string_split(@ClientLocationIds, ',')) OR (@ClientLocationIds is null OR @ClientLocationIds = ''))

END