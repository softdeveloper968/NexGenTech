
        declare @__clientId_0 int=3;
        declare @__query_ClaimBilledFrom_Value_Date_1 datetime2(7)='2023-05-19 00:00:00';
        declare @__query_ClaimBilledTo_Value_Date_2 datetime2(7)='2023-05-19 23:59:00';

SELECT 
    [c0].[PatientLastName], 
    [c0].[PatientFirstName], 
    [c0].[DateOfBirth], 
    [c0].[PolicyNumber], 
    [a].[Name], 
    [c3].[LookupName], 
    [c0].[ClaimNumber], 
    [c].[ClaimNumber], 
    [c].[LineItemControlNumber], 
    [c0].[ProcedureCode],
    [c0].[DateOfServiceFrom], 
    [c0].[DateOfServiceTo], 
    [c1].[Description],
    [c].[ClaimLineItemStatusValue], 
    [c].[ExceptionReason], 
    [c].[ExceptionRemark], 
    [c].[ReasonCode], 
    [c0].[ClaimBilledOn], 
    COALESCE([c0].[BilledAmount], 0.0), 
    [c].[LineItemPaidAmount], 
    COALESCE([c].[TotalAllowedAmount], 0.0), 
    COALESCE([c].[TotalNonAllowedAmount], 0.0), 
    [c].[CheckPaidAmount], 
    [c].[CheckDate], 
    [c].[CheckNumber], 
    [c].[ReasonDescription], 
    [c].[RemarkCode], 
    [c].[RemarkDescription], 
    [c].[CoinsuranceAmount], [c].[CopayAmount], [c].[DeductibleAmount], [c].[CobAmount], [c].[PenalityAmount], [c].[EligibilityStatus], [c].[EligibilityInsurance], [c].[EligibilityPolicyNumber], [c].[EligibilityFromDate], [c].[VerifiedMemberId], [c].[CobLastVerified], [c].[LastActiveEligibleDateRange], [c].[PrimaryPayer], [c].[PrimaryPolicyNumber], [c2].[BatchNumber], [c0].[CreatedOn], CASE
    WHEN [c].[LastModifiedOn] IS NULL THEN CAST(1 AS bit)
        ELSE CAST(0 AS bit)
    END, 
    [c].[CreatedOn], [c].[LastModifiedOn]

FROM [IntegratedServices].[ClaimStatusTransactions] AS [c]
INNER JOIN [IntegratedServices].[ClaimStatusBatchClaims] AS [c0] ON [c].[ClaimStatusBatchClaimId] = [c0].[Id]
LEFT JOIN [IntegratedServices].[ClaimLineItemStatuses] AS [c1] ON [c].[ClaimLineItemStatusId] = [c1].[Id]
INNER JOIN [IntegratedServices].[ClaimStatusBatches] AS [c2] ON [c0].[ClaimStatusBatchId] = [c2].[Id]
INNER JOIN [dbo].[AuthTypes] AS [a] ON [c2].[AuthTypeId] = [a].[Id]
INNER JOIN [dbo].[ClientInsurances] AS [c3] ON [c2].[ClientInsuranceId] = [c3].[Id]

WHERE [c].[IsDeleted] = CAST(0 AS bit)
        AND [c].[ClientId] = @__clientId_0 
        AND CONVERT(date, [c0].[ClaimBilledOn]) >= @__query_ClaimBilledFrom_Value_Date_1 
        AND CONVERT(date, [c0].[ClaimBilledOn]) <= @__query_ClaimBilledTo_Value_Date_2 
        AND [c].[IsDeleted] = CAST(0 AS bit)
        AND ([c1].[Id] NOT IN (10, 17) OR ([c1].[Id] IS NULL))
